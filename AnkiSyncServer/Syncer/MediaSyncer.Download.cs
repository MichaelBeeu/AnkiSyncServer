using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Syncer
{
    public partial class MediaSyncer
    {
        public async Task<Stream> Download(string userId, List<string> fileList)
        {
            var zipStream = new MemoryStream();
            using (var zipFile = new ZipArchive(zipStream, ZipArchiveMode.Create, true)) {
                var meta = new Dictionary<int, string>();

                foreach ((var filename, var idx) in fileList.Select((value, i) => (value, i)))
                {
                    var entry = zipFile.CreateEntry(idx.ToString());
                    (var media, var modifyTime) = await _mediaManager.GetFile(userId, filename);

                    entry.LastWriteTime = (DateTimeOffset)modifyTime.ToLocalTime();

                    using (var entryStream = entry.Open())
                    {
                        await media.CopyToAsync(entryStream);
                        meta[idx] = filename;
                    }

                    media.Close();
                }

                var jsonSerializer = new JsonSerializer();

                var metaEntry = zipFile.CreateEntry("_meta");
                using (var entryStream = metaEntry.Open())
                using (var entryStreamWriter = new StreamWriter(entryStream))
                {
                    jsonSerializer.Serialize(entryStreamWriter, meta);
                }
            }

            zipStream.Seek(0, SeekOrigin.Begin);

            return zipStream;
        }
    }
}
