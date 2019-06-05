using AnkiSyncServer.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AnkiSyncServer.Syncer
{
    public partial class MediaSyncer
    {
        public async Task<long> Upload(string userId, IFormFile data)
        {
            long processed = 0;
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
                            var mediaRecord = await _mediaManager.RemoveFile(userId, fname);
                            await DeleteMediaRecord(mediaRecord);
                        } else
                        {
                            var mediaFile = mediaArchive.GetEntry(index);
                            var mediaRecord = await _mediaManager.AddFile(userId, fname, mediaFile);
                            await UpdateMediaRecord(mediaRecord);
                        }

                        processed++;
                    }
                }
            }
            return processed;
        }

        private async Task<Media> GetPreviousMedia(Media media)
        {
            return await _context.Media
                .FirstOrDefaultAsync(m => m.UserId == media.UserId && m.Filename == media.Filename);
        }

        private async Task<Boolean> UpdateMediaRecord(Media media)
        {
            var previousMedia = await GetPreviousMedia(media);

            if (previousMedia != null)
            {
                media.Id = previousMedia.Id;
            }

            _context.Media.Update(media);

            return true;
        }

        private async Task<Boolean> DeleteMediaRecord(Media media)
        {
            var previousMedia = await GetPreviousMedia(media);

            if (previousMedia != null)
            {
                _context.Media.Remove(previousMedia);
            }

            return true;
        }
    }
}
