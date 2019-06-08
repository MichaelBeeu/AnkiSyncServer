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
        /// <summary>
        /// Used to process file uploads from the client.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <param name="data">Used to provide the client media archive.</param>
        /// <returns>Returns the number of affected media files.</returns>
        public async Task<long> Upload(string userId, IFormFile data)
        {
            long processed = 0;
            // Open the Zip archive we were sent, to get the meta file
            // containing the list of files we've been sent.
            using (Stream mediaStream = data.OpenReadStream())
            using (ZipArchive mediaArchive = new ZipArchive(mediaStream))
            {
                ZipArchiveEntry metaFile = mediaArchive.GetEntry("_meta");

                // Parse the data as JSON.
                using (StreamReader metaStream = new StreamReader(metaFile.Open()))
                using (JsonTextReader metaReader = new JsonTextReader(metaStream))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    var meta = serializer.Deserialize<List<List<string>>>(metaReader);

                    foreach (var entry in meta)
                    {
                        string fname = entry[0];
                        string index = entry[1];

                        // Deleted files will have an empty index entry.
                        if (String.IsNullOrEmpty(index))
                        {
                            // Delete old media entry
                            Media mediaRecord = await mediaManager.RemoveFile(userId, fname);
                            await DeleteMediaRecord(mediaRecord);
                        } else
                        {
                            ZipArchiveEntry mediaFile = mediaArchive.GetEntry(index);
                            Media mediaRecord = await mediaManager.AddFile(userId, fname, mediaFile);
                            await UpdateMediaRecord(mediaRecord);
                        }

                        processed++;
                    }
                }
            }
            return processed;
        }

        /// <summary>
        /// Gets the previously entered media file.
        /// </summary>
        /// <param name="media">Used to indicate the media file being requested.</param>
        /// <returns>Returns the Media object of the file, or NULL</returns>
        private async Task<Media> GetPreviousMedia(Media media)
        {
            return await context.Media
                .FirstOrDefaultAsync(m => m.UserId == media.UserId && m.Filename == media.Filename);
        }

        /// <summary>
        /// Create a new media record, or update an existing record.
        /// </summary>
        /// <param name="media">Used to indicate the media record.</param>
        /// <returns>Returns sucess or failure.</returns>
        private async Task<Boolean> UpdateMediaRecord(Media media)
        {
            Media previousMedia = await GetPreviousMedia(media);

            if (previousMedia != null)
            {
                media.Id = previousMedia.Id;
            }

            context.Media.Update(media);

            return true;
        }

        /// <summary>
        /// Delete a given media record.
        /// </summary>
        /// <param name="media">Used to indicate the media record.</param>
        /// <returns>Returns success or failure.</returns>
        private async Task<Boolean> DeleteMediaRecord(Media media)
        {
            Media previousMedia = await GetPreviousMedia(media);

            if (previousMedia != null)
            {
                context.Media.Remove(previousMedia);
            }

            return true;
        }
    }
}
