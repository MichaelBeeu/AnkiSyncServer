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
        /// <summary>
        /// Provides an zip archive of the requested media files.
        /// </summary>
        /// <param name="userId">Used to indicate owning user.</param>
        /// <param name="fileList">Used to indicate the list of requested files.</param>
        /// <returns>Returns a stream of the media archive.</returns>
        public async Task<Stream> Download(string userId, List<string> fileList)
        {
            MemoryStream zipStream = new MemoryStream();
            using (ZipArchive zipFile = new ZipArchive(zipStream, ZipArchiveMode.Create, true)) {
                Dictionary<long, string> meta = new Dictionary<long, string>();

                foreach ((string filename, long idx) in fileList.Select((value, i) => (value, i)))
                {
                    ZipArchiveEntry entry = zipFile.CreateEntry(idx.ToString());
                    (Stream media, DateTime modifyTime) = await mediaManager.GetFile(userId, filename);

                    entry.LastWriteTime = (DateTimeOffset)modifyTime.ToLocalTime();

                    using (Stream entryStream = entry.Open())
                    {
                        await media.CopyToAsync(entryStream);
                        meta[idx] = filename;
                    }

                    media.Close();
                }

                JsonSerializer jsonSerializer = new JsonSerializer();

                ZipArchiveEntry metaEntry = zipFile.CreateEntry("_meta");
                using (Stream entryStream = metaEntry.Open())
                using (StreamWriter entryStreamWriter = new StreamWriter(entryStream))
                {
                    jsonSerializer.Serialize(entryStreamWriter, meta);
                }
            }

            zipStream.Seek(0, SeekOrigin.Begin);

            return zipStream;
        }
    }
}
