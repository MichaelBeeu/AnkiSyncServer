using AnkiSyncServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AnkiSyncServer.MediaManager
{
    public class LocalMediaManager : IMediaManager
    {
        private IConfiguration config;
        private IHostingEnvironment environment;

        public LocalMediaManager(
            IConfiguration configuration,
            IHostingEnvironment environment)
        {
            config = configuration;
            this.environment = environment;
        }

        /// <summary>
        /// Get the path on disk for the requested user.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <returns>Returns a string containing the file path the to user's media directory</returns>
        private string GetUserMediaDirectory(string userId)
        {
            return Path.Combine(environment.WebRootPath, config["Media:Directory"], userId);
        }

        /// <summary>
        /// Add a media file to the user's collection of media.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <param name="filename">Used to indicate the media filename.</param>
        /// <param name="mediaEntry">Used to provide the media contents.</param>
        /// <returns>Returns a Media object to be inserted into the databse.</returns>
        public async Task<Media> AddFile(string userId, string filename, ZipArchiveEntry mediaEntry)
        {
            string mediaDir = GetUserMediaDirectory(userId);
            string mediaFilename = Path.Combine(mediaDir, filename);

            Directory.CreateDirectory(mediaDir);

            using (Stream stream = mediaEntry.Open())
            using (FileStream dest = new FileStream(mediaFilename, FileMode.OpenOrCreate))
            using (SHA1Managed hash = new SHA1Managed())
            {
                await stream.CopyToAsync(dest);
                dest.Seek(0, SeekOrigin.Begin);
                byte[] sha1 = hash.ComputeHash(dest);
                return new Media
                {
                    UserId = userId,
                    Filename = filename,
                    Checksum = string.Concat(hash.Hash.Select(b => b.ToString("x2"))),
                    Modified = mediaEntry.LastWriteTime.DateTime.ToUniversalTime(),
                    Dirty = 0,
                };
            }
        }

        /// <summary>
        /// Get the media file stream.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <param name="filename">Used to indicate the media file.</param>
        /// <returns>Returns a tuple of the media stream, and last modify time.</returns>
        public async Task<Tuple<Stream, DateTime>> GetFile(string userId, string filename)
        {
            string mediaFilename = Path.Combine(GetUserMediaDirectory(userId), filename);

            return new Tuple<Stream, DateTime>(new FileStream(mediaFilename, FileMode.Open), File.GetLastWriteTimeUtc(mediaFilename));
        }

        /// <summary>
        /// Remoes a file from the user's media colleciton.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <param name="filename">Used to indicate the media filename.</param>
        /// <returns>Returns a Media record that can be used to delte the file.</returns>
        public async Task<Media> RemoveFile(string userId, string filename)
        {
            string mediaFilename = Path.Combine(GetUserMediaDirectory(userId), filename);

            File.Delete(mediaFilename);

            return new Media
            {
                UserId = userId,
                Filename = filename,
            };
        }
    }
}
