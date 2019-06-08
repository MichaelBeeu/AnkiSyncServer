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

        private string GetUserMediaDirectory(string userId)
        {
            return Path.Combine(environment.WebRootPath, config["Media:Directory"], userId);
        }

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

        public async Task<Tuple<Stream, DateTime>> GetFile(string userId, string filename)
        {
            string mediaFilename = Path.Combine(GetUserMediaDirectory(userId), filename);

            return new Tuple<Stream, DateTime>(new FileStream(mediaFilename, FileMode.Open), File.GetLastWriteTimeUtc(mediaFilename));
        }

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
