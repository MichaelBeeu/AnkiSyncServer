using AnkiSyncServer.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Syncer
{
    public partial class FullSyncer
    {
        public async Task<string> Download(string userId)
        {
            var dbFile = Path.GetTempFileName();

            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = dbFile;

            using (SqliteConnection db = new SqliteConnection(connectionStringBuilder.ToString()))
            {
                await db.OpenAsync();

                await InitSchema(db);
                await CopyCollectionsToClientData(userId, db);
                await CopyNotesToClientData(userId, db);
                await CopyCardsToClientData(userId, db);
                await CopyRevLogsToClientData(userId, db);
                await CopyGravesToClientData(userId, db);
            }

            return dbFile;
        }

        private async Task<Boolean> CopyCollectionsToClientData(string userId, SqliteConnection db)
        {
            using (SqliteCommand cmd = db.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO col (id, crt, mod, scm, ver, dty, usn, ls, conf, models, decks, dconf, tags)
                                    VALUES (@id, @crt, @mod, @scm, @ver, @dty, @usn, @ls, @conf, @models, @decks, @dconf, @tags);";

                cmd.Parameters.Add("@id", SqliteType.Integer);
                cmd.Parameters.Add("@crt", SqliteType.Integer);
                cmd.Parameters.Add("@mod", SqliteType.Integer);
                cmd.Parameters.Add("@scm", SqliteType.Integer);
                cmd.Parameters.Add("@ver", SqliteType.Integer);
                cmd.Parameters.Add("@dty", SqliteType.Integer);
                cmd.Parameters.Add("@usn", SqliteType.Integer);
                cmd.Parameters.Add("@ls", SqliteType.Integer);
                cmd.Parameters.Add("@conf", SqliteType.Text);
                cmd.Parameters.Add("@models", SqliteType.Text);
                cmd.Parameters.Add("@decks", SqliteType.Text);
                cmd.Parameters.Add("@dconf", SqliteType.Text);
                cmd.Parameters.Add("@tags", SqliteType.Text);

                var collections = context.Collections
                    .Where(c => c.UserId == userId);

                foreach (Collection collection in collections)
                {
                    cmd.Parameters["@id"].Value = collection.ClientId;
                    cmd.Parameters["@crt"].Value = ((DateTimeOffset)collection.CreationDate.ToLocalTime()).ToUnixTimeSeconds();
                    cmd.Parameters["@mod"].Value = ((DateTimeOffset)collection.Modified.ToLocalTime()).ToUnixTimeMilliseconds();
                    cmd.Parameters["@scm"].Value = ((DateTimeOffset)collection.SchemaModified.ToLocalTime()).ToUnixTimeMilliseconds();
                    cmd.Parameters["@ver"].Value = collection.Version;
                    cmd.Parameters["@dty"].Value = collection.Dirty;
                    cmd.Parameters["@usn"].Value = collection.UpdateSequenceNumber;
                    cmd.Parameters["@ls"].Value = ((DateTimeOffset)((DateTime)collection.LastSync).ToLocalTime()).ToUnixTimeMilliseconds();
                    cmd.Parameters["@conf"].Value = collection.Conf;
                    cmd.Parameters["@models"].Value = collection.Models;
                    cmd.Parameters["@decks"].Value = collection.Decks;
                    cmd.Parameters["@dconf"].Value = collection.DeckConf;
                    cmd.Parameters["@tags"].Value = collection.Tags;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }

        private async Task<Boolean> CopyNotesToClientData(string userId, SqliteConnection db)
        {
            using (SqliteCommand cmd = db.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO notes (id, guid, mid, mod, usn, tags, flds, sfld, csum, flags, data)
                                    VALUES (@id, @guid, @mid, @mod, @usn, @tags, @flds, @sfld, @csum, @flags, @data);";

                cmd.Parameters.Add("@id", SqliteType.Integer);
                cmd.Parameters.Add("@guid", SqliteType.Text);
                cmd.Parameters.Add("@mid", SqliteType.Integer);
                cmd.Parameters.Add("@mod", SqliteType.Integer);
                cmd.Parameters.Add("@usn", SqliteType.Integer);
                cmd.Parameters.Add("@tags", SqliteType.Text);
                cmd.Parameters.Add("@flds", SqliteType.Text);
                cmd.Parameters.Add("@sfld", SqliteType.Text);
                cmd.Parameters.Add("@csum", SqliteType.Integer);
                cmd.Parameters.Add("@flags", SqliteType.Integer);
                cmd.Parameters.Add("@data", SqliteType.Text);

                var notes = context.Notes
                    .Where(n => n.UserId == userId);

                foreach (Note note in notes)
                {
                    cmd.Parameters["@id"].Value = note.ClientId;
                    cmd.Parameters["@guid"].Value = note.Guid;
                    cmd.Parameters["@mid"].Value = note.ModelId;
                    cmd.Parameters["@mod"].Value = ((DateTimeOffset)note.Modified.ToLocalTime()).ToUnixTimeSeconds();
                    cmd.Parameters["@usn"].Value = note.UpdateSequenceNumber;
                    cmd.Parameters["@tags"].Value = note.Tags;
                    cmd.Parameters["@flds"].Value = note.Fields;
                    cmd.Parameters["@sfld"].Value = note.SortField;
                    cmd.Parameters["@csum"].Value = note.Checksum;
                    cmd.Parameters["@flags"].Value = note.Flags;
                    cmd.Parameters["@data"].Value = note.Data;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
        private async Task<Boolean> CopyCardsToClientData(string userId, SqliteConnection db)
        {
            using (SqliteCommand cmd = db.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO cards (id, nid, did, ord, mod, usn, type, queue, due, ivl, factor, reps, lapses, left, odue, odid, flags, data)
                                    VALUES (@id, @nid, @did, @ord, @mod, @usn, @type, @queue, @due, @ivl, @factor, @reps, @lapses, @left, @odue, @odid, @flags, @data)";

                cmd.Parameters.Add("@id", SqliteType.Integer);
                cmd.Parameters.Add("@nid", SqliteType.Integer);
                cmd.Parameters.Add("@did", SqliteType.Integer);
                cmd.Parameters.Add("@ord", SqliteType.Integer);
                cmd.Parameters.Add("@mod", SqliteType.Integer);
                cmd.Parameters.Add("@usn", SqliteType.Integer);
                cmd.Parameters.Add("@type", SqliteType.Integer);
                cmd.Parameters.Add("@queue", SqliteType.Integer);
                cmd.Parameters.Add("@due", SqliteType.Integer);
                cmd.Parameters.Add("@ivl", SqliteType.Integer);
                cmd.Parameters.Add("@factor", SqliteType.Integer);
                cmd.Parameters.Add("@reps", SqliteType.Integer);
                cmd.Parameters.Add("@lapses", SqliteType.Integer);
                cmd.Parameters.Add("@left", SqliteType.Integer);
                cmd.Parameters.Add("@odue", SqliteType.Integer);
                cmd.Parameters.Add("@odid", SqliteType.Integer);
                cmd.Parameters.Add("@flags", SqliteType.Integer);
                cmd.Parameters.Add("@data", SqliteType.Text);

                var cards = context.Cards
                    .Where(c => c.UserId == userId);

                foreach (Card card in cards)
                {
                    cmd.Parameters["@id"].Value = card.ClientId;
                    cmd.Parameters["@nid"].Value = card.NoteId;
                    cmd.Parameters["@did"].Value = card.DeckId;
                    cmd.Parameters["@ord"].Value = card.Ordinal;
                    cmd.Parameters["@mod"].Value = ((DateTimeOffset)card.Modified.ToLocalTime()).ToUnixTimeSeconds();
                    cmd.Parameters["@usn"].Value = card.UpdateSequenceNumber;
                    cmd.Parameters["@type"].Value = card.Type;
                    cmd.Parameters["@queue"].Value = card.Queue;
                    cmd.Parameters["@due"].Value = card.Due;
                    cmd.Parameters["@ivl"].Value = card.Interval;
                    cmd.Parameters["@factor"].Value = card.Factor;
                    cmd.Parameters["@reps"].Value = card.Repetitions;
                    cmd.Parameters["@lapses"].Value = card.Lapses;
                    cmd.Parameters["@left"].Value = card.Left;
                    cmd.Parameters["@odue"].Value = ((DateTimeOffset)card.OriginalDue.ToLocalTime()).ToUnixTimeSeconds();
                    cmd.Parameters["@odid"].Value = card.OriginalDeckId;
                    cmd.Parameters["@flags"].Value = card.Flags;
                    cmd.Parameters["@data"].Value = card.Data;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }

        private async Task<Boolean> CopyRevLogsToClientData(string userId, SqliteConnection db)
        {
            using (SqliteCommand cmd = db.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO revlog (id, cid, usn, ease, ivl, lastIvl, factor, time, type)
                                    VALUES (@id, @cid, @usn, @ease, @ivl, @lastIvl, @factor, @time, @type)";

                cmd.Parameters.Add("@id", SqliteType.Integer);
                cmd.Parameters.Add("@cid", SqliteType.Integer);
                cmd.Parameters.Add("@usn", SqliteType.Integer);
                cmd.Parameters.Add("@ease", SqliteType.Integer);
                cmd.Parameters.Add("@ivl", SqliteType.Integer);
                cmd.Parameters.Add("@lastIvl", SqliteType.Integer);
                cmd.Parameters.Add("@factor", SqliteType.Integer);
                cmd.Parameters.Add("@time", SqliteType.Integer);
                cmd.Parameters.Add("@type", SqliteType.Integer);

                var revLogs = context.ReviewLogs
                    .Where(r => r.UserId == userId);

                foreach (ReviewLog revLog in revLogs)
                {
                    cmd.Parameters["@id"].Value = revLog.ClientId;
                    cmd.Parameters["@cid"].Value = revLog.CardId;
                    cmd.Parameters["@usn"].Value = revLog.UpdateSequenceNumber;
                    cmd.Parameters["@ease"].Value = revLog.Ease;
                    cmd.Parameters["@ivl"].Value = revLog.Interval;
                    cmd.Parameters["@lastIvl"].Value = revLog.LastInterval;
                    cmd.Parameters["@factor"].Value = revLog.Factor;
                    cmd.Parameters["@time"].Value = revLog.Time;
                    cmd.Parameters["@type"].Value = revLog.Type;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }

        private async Task<Boolean> CopyGravesToClientData(string userId, SqliteConnection db)
        {
            using (SqliteCommand cmd = db.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO graves (usn, oid, type)
                                    VALUES (@usn, @oid, @type)";

                cmd.Parameters.Add("@usn", SqliteType.Integer);
                cmd.Parameters.Add("@oid", SqliteType.Integer);
                cmd.Parameters.Add("@type", SqliteType.Integer);

                var graves = context.Graves
                    .Where(g => g.UserId == userId);

                foreach (Grave grave in graves)
                {
                    cmd.Parameters["@usn"].Value = grave.UpdateSequenceNumber;
                    cmd.Parameters["@oid"].Value = grave.OriginalId;
                    cmd.Parameters["@type"].Value = grave.Type;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }

        private async Task<Boolean> InitSchema(SqliteConnection db)
        {
            using (SqliteCommand cmd = db.CreateCommand())
            {
                /* EWWWWWWWW */
                cmd.CommandText = @"
CREATE TABLE col (
    id              integer primary key,
    crt             integer not null,
    mod             integer not null,
    scm             integer not null,
    ver             integer not null,
    dty             integer not null,
    usn             integer not null,
    ls              integer not null,
    conf            text not null,
    models          text not null,
    decks           text not null,
    dconf           text not null,
    tags            text not null
);
CREATE TABLE notes (
    id              integer primary key,   /* 0 */
    guid            text not null,         /* 1 */
    mid             integer not null,      /* 2 */
    mod             integer not null,      /* 3 */
    usn             integer not null,      /* 4 */
    tags            text not null,         /* 5 */
    flds            text not null,         /* 6 */
    sfld            integer not null,      /* 7 */
    csum            integer not null,      /* 8 */
    flags           integer not null,      /* 9 */
    data            text not null          /* 10 */
);
CREATE TABLE cards (
    id              integer primary key,   /* 0 */
    nid             integer not null,      /* 1 */
    did             integer not null,      /* 2 */
    ord             integer not null,      /* 3 */
    mod             integer not null,      /* 4 */
    usn             integer not null,      /* 5 */
    type            integer not null,      /* 6 */
    queue           integer not null,      /* 7 */
    due             integer not null,      /* 8 */
    ivl             integer not null,      /* 9 */
    factor          integer not null,      /* 10 */
    reps            integer not null,      /* 11 */
    lapses          integer not null,      /* 12 */
    left            integer not null,      /* 13 */
    odue            integer not null,      /* 14 */
    odid            integer not null,      /* 15 */
    flags           integer not null,      /* 16 */
    data            text not null          /* 17 */
);
CREATE TABLE revlog (
    id              integer primary key,
    cid             integer not null,
    usn             integer not null,
    ease            integer not null,
    ivl             integer not null,
    lastIvl         integer not null,
    factor          integer not null,
    time            integer not null,
    type            integer not null
);
CREATE TABLE graves (
    usn             integer not null,
    oid             integer not null,
    type            integer not null
);
CREATE INDEX ix_notes_usn on notes (usn);
CREATE INDEX ix_cards_usn on cards (usn);
CREATE INDEX ix_revlog_usn on revlog (usn);
CREATE INDEX ix_cards_nid on cards (nid);
CREATE INDEX ix_cards_sched on cards (did, queue, due);
CREATE INDEX ix_revlog_cid on revlog (cid);
CREATE INDEX ix_notes_csum on notes (csum);
                ";

                await cmd.ExecuteNonQueryAsync();
            }

            return true;
        }
    }
}
