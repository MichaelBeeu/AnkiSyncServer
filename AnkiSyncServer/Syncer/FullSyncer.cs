using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using AnkiSyncServer.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;

namespace AnkiSyncServer.Syncer
{
    public class FullSyncer
    {

        private AnkiDbContext _context { get; set; }

        public FullSyncer(
            AnkiDbContext context
            )
        {
            _context = context;
        }
        
        public async Task<Boolean> upload(string userId, IFormFile input)
        {
            // Because a full sync involves receiving a full Sqlite database file, I need to
            // create a temporary file to save the upload to, then open that file as a Sqlite
            // database, so I can extract the data, and save it to my target database.
            string tempDb = Path.GetTempFileName();
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = tempDb;

            try
            {
                using (var stream = new FileStream(tempDb, FileMode.Create))
                {
                    await input.CopyToAsync(stream);
                }

                using (var db = new SqliteConnection(connectionStringBuilder.ToString()))
                {
                    await db.OpenAsync();

                    await copyColFromClientData(userId, db, _context);
                    await copyCardsFromClientData(userId, db, _context);
                    
                }
            }
            /*
            catch (InvalidOperationException e)
            {
                Debug.WriteLine(e);
                throw e;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw e;
                // Log the error here.
            }
            */
            finally
            {
                File.Delete(tempDb);
            }

            return false;
        }

        protected async Task<Boolean> copyColFromClientData(string userId, SqliteConnection src, AnkiDbContext dest)
        {
            var existingCollection = dest.Collections
                .Where(collection => collection.UserId == userId);
            dest.Collections.RemoveRange(existingCollection);

            using (var cmd = src.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM col;";
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var collections = reader.Cast<IDataRecord>()
                        .Select(dr => new Collection
                        {
                            ClientId = (int)(long)dr["id"],
                            UserId = userId,
                            CreationDate = DateTimeOffset.FromUnixTimeSeconds((long)dr["crt"]).DateTime,
                            Modified = DateTimeOffset.FromUnixTimeMilliseconds((long)dr["mod"]).DateTime,
                            SchemaModified = DateTimeOffset.FromUnixTimeMilliseconds((long)dr["scm"]).DateTime,
                            Version = (int)(long)dr["ver"],
                            Dirty = (int)(long)dr["dty"],
                            UpdateSequenceNumber = (int)(long)dr["usn"],
                            LastSync = dr["ls"] != null ? (DateTime?)DateTimeOffset.FromUnixTimeMilliseconds((long)dr["ls"]).DateTime : null,
                            Conf = (string)dr["conf"],
                            Models = (string)dr["models"],
                            Decks = (string)dr["decks"],
                            DeckConf = (string)dr["dconf"],
                            Tags = (string)dr["tags"],
                        });
                    dest.Collections.AddRange(collections);
                }
            }

            await dest.SaveChangesAsync();
            return true;
        }

        protected async Task<Boolean> copyCardsFromClientData(string userId, SqliteConnection src, AnkiDbContext dest)
        {
            var existingCards = dest.Cards
                .Where(card => card.UserId == userId);
            dest.Cards.RemoveRange(existingCards);

            using (var cmd = src.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM cards;";
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var cards = reader.Cast<IDataRecord>()
                        .Select(dr => new Card
                        {
                            ClientId = (long)dr["id"],
                            UserId = userId,
                            NoteId = (long)dr["nid"],
                            Ordinal = (int)(long)dr["ord"],
                            Modified = DateTimeOffset.FromUnixTimeSeconds((long)dr["mod"]).DateTime,
                            UpdateSequenceNumber = (int)(long)dr["usn"],
                            Type = (CardType)(long)dr["type"],
                            Queue = (QueueType)(long)dr["queue"],
                            Due = (int)(long)dr["due"],
                            Interval = (int)(long)dr["ivl"],
                            Factor = (int)(long)dr["factor"],
                            Repetitions = (int)(long)dr["reps"],
                            Lapses = (int)(long)dr["lapses"],
                            Left = (int)(long)dr["left"],
                            OriginalDue = DateTimeOffset.FromUnixTimeSeconds((long)dr["odue"]).DateTime,
                            OriginalDeckId = (int)(long)dr["odid"],
                            Flags = (int)(long)dr["flags"],
                            Data = (string)dr["data"],
                        });
                    dest.Cards.AddRange(cards);
                }
            }

            await dest.SaveChangesAsync();
            return true;
        }
    }
}
