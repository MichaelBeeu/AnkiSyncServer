using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Syncer
{
    public partial class MediaSyncer : IMediaSyncer
    {
        public async Task<Boolean> Upload(IFormFile data)
        {
            // Open the Zip archive we were sent, to get the meta file
            // containing the list of files we've been sent.
            using (var mediaStream = data.OpenReadStream())
            using (var mediaArchive = new ZipArchive(mediaStream))
            {
                var metaFile = mediaArchive.GetEntry("_meta");

                // Parse the data as JSON.
                using (var metaStream = new StreamReader(metaFile.Open()))
                using (var metaReader = new JsonTextReader(metaStream))
                {
                    var serializer = new JsonSerializer();

                    var meta = serializer.Deserialize<List<List<string>>>(metaReader);

                    foreach (var entry in meta)
                    {
                        var fname = entry[0];
                        var index = entry[1];

                        // Deleted files will have an empty index entry.
                        if (String.IsNullOrEmpty(index))
                        {
                            // Delete old media entry
                        } else
                        {
                            string tempFile = Path.GetTempFileName();
                            Debug.WriteLine(tempFile);

                            var mediaFile = mediaArchive.GetEntry(index);
                            mediaFile.ExtractToFile(tempFile, true);
                            /*
                            //using (var mediaFileStream = new StreamReader(mediaFile.Open()))
                            using (var mediaFileStream = mediaFile.Open())
                            using (var outputStream = new FileStream(tempFile, FileMode.Open))
                            {
                                await mediaFileStream.CopyToAsync(outputStream);
                            }
                            */
                        }
                    }
                }
            }
            return false;
        }
    }
}
