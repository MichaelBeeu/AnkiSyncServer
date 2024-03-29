﻿using Microsoft.AspNetCore.Http;
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
using Newtonsoft.Json;
using AnkiSyncServer.Models.CollectionData;

namespace AnkiSyncServer.Syncer
{
    public partial class FullSyncer
    {
        public async Task<Boolean> Upload(string userId, IFormFile input)
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

                    await CopyColFromClientData(userId, db);
                    await CopyCardsFromClientData(userId, db);
                    await CopyGravesFromClientData(userId, db);
                    await CopyNotesFromClientData(userId, db);
                    await CopyReviewLogsFromClientData(userId, db);

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw e;
            }
            finally
            {
                File.Delete(tempDb);
            }

            return false;
        }

        protected async Task<Boolean> CopyColFromClientData(string userId, SqliteConnection src)
        {
            var existingCollection = context.Collections
                .Where(collection => collection.UserId == userId);
            context.Collections.RemoveRange(existingCollection);

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
                            CreationDate = DateTimeOffset.FromUnixTimeSeconds((long)dr["crt"]).UtcDateTime,
                            Modified = DateTimeOffset.FromUnixTimeMilliseconds((long)dr["mod"]).UtcDateTime,
                            SchemaModified = DateTimeOffset.FromUnixTimeMilliseconds((long)dr["scm"]).UtcDateTime,
                            Version = (int)(long)dr["ver"],
                            Dirty = (int)(long)dr["dty"],
                            UpdateSequenceNumber = (int)(long)dr["usn"],
                            LastSync = dr["ls"] != null ? (DateTime?)DateTimeOffset.FromUnixTimeMilliseconds((long)dr["ls"]).UtcDateTime : null,
                            Conf = (string)dr["conf"],
                            Models = (string)dr["models"],
                            Decks = JsonConvert.DeserializeObject<Dictionary<long, Deck>>((string)dr["decks"]),
                            DeckConf = (string)dr["dconf"],
                            Tags = (string)dr["tags"],
                        });
                    context.Collections.AddRange(collections);
                }
            }

            return true;
        }

        protected async Task<Boolean> CopyCardsFromClientData(string userId, SqliteConnection src)
        {
            var existingCards = context.Cards
                .Where(card => card.UserId == userId);
            context.Cards.RemoveRange(existingCards);

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
                            DeckId = (long)dr["did"],
                            Ordinal = (int)(long)dr["ord"],
                            Modified = DateTimeOffset.FromUnixTimeSeconds((long)dr["mod"]).UtcDateTime,
                            UpdateSequenceNumber = (int)(long)dr["usn"],
                            Type = (CardType)(long)dr["type"],
                            Queue = (QueueType)(long)dr["queue"],
                            Due = (int)(long)dr["due"],
                            Interval = (int)(long)dr["ivl"],
                            Factor = (int)(long)dr["factor"],
                            Repetitions = (int)(long)dr["reps"],
                            Lapses = (int)(long)dr["lapses"],
                            Left = (int)(long)dr["left"],
                            OriginalDue = DateTimeOffset.FromUnixTimeSeconds((long)dr["odue"]).UtcDateTime,
                            OriginalDeckId = (int)(long)dr["odid"],
                            Flags = (int)(long)dr["flags"],
                            Data = (string)dr["data"],
                        });
                    context.Cards.AddRange(cards);
                }
            }

            return true;
        }

        protected async Task<Boolean> CopyGravesFromClientData(string userId, SqliteConnection src)
        {
            var existingGraves = context.Graves
                .Where(grave => grave.UserId == userId);
            context.Graves.RemoveRange(existingGraves);

            using (var cmd = src.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM graves;";
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var graves = reader.Cast<IDataRecord>()
                        .Select(dr => new Grave
                        {
                            UserId = userId,
                            UpdateSequenceNumber = (long)dr["usn"],
                            OriginalId = (long)dr["oid"],
                            Type = (GraveType)(long)dr["type"],
                        });
                    context.Graves.AddRange(graves);
                }
            }

            return true;
        }

        protected async Task<Boolean> CopyNotesFromClientData(string userId, SqliteConnection src)
        {
            var existingNotes = context.Notes
                .Where(note => note.UserId == userId);
            context.Notes.RemoveRange(existingNotes);

            using (var cmd = src.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM notes;";
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var notes = reader.Cast<IDataRecord>()
                        .Select(dr => new Note
                        {
                            ClientId = (long)dr["id"],
                            UserId = userId,
                            Guid = (string)dr["guid"],
                            ModelId = (long)dr["mid"],
                            Modified = DateTimeOffset.FromUnixTimeSeconds((long)dr["mod"]).UtcDateTime,
                            UpdateSequenceNumber = (long)dr["usn"],
                            Tags = (string)dr["tags"],
                            Fields = (string)dr["flds"],
                            SortField = (string)dr["sfld"],
                            Checksum = (long)dr["csum"],
                            Flags = (long)dr["flags"],
                            Data = (string)dr["data"],
                        });
                    context.Notes.AddRange(notes);
                }
            }

            return true;
        }

        protected async Task<Boolean> CopyReviewLogsFromClientData(string userId, SqliteConnection src)
        {
            var existingReviewLogs = context.ReviewLogs
                .Where(reviewlog => reviewlog.UserId == userId);
            context.ReviewLogs.RemoveRange(existingReviewLogs);

            using (var cmd = src.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM revlog;";
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var notes = reader.Cast<IDataRecord>()
                        .Select(dr => new ReviewLog
                        {
                            ClientId = (long)dr["id"],
                            UserId = userId,
                            CardId = (long)dr["cid"],
                            UpdateSequenceNumber = (long)dr["usn"],
                            Ease = (int)(long)dr["ease"],
                            Interval = (long)dr["ivl"],
                            LastInterval = (long)dr["lastIvl"],
                            Factor = (long)dr["factor"],
                            Time = (long)dr["time"],
                            Type = (ReviewType)(long)dr["type"],
                        });
                    context.ReviewLogs.AddRange(notes);
                }
            }

            return true;
        }
    }
}
