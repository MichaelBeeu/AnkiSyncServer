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
        private IConfiguration _config;
        private IHostingEnvironment _environment;

        public LocalMediaManager(
            IConfiguration configuration,
            IHostingEnvironment environment)
        {
            _config = configuration;
            _environment = environment;
        }

        private string GetUserMediaDirectory(string userId)
        {
            Debug.WriteLine(_environment.WebRootPath);
            Debug.WriteLine(_config["Media:Directory"]);
            return Path.Combine(_environment.WebRootPath, _config["Media:Directory"], userId);
        }

        public async Task<Media> AddFile(string userId, string filename, ZipArchiveEntry mediaEntry)
        {
            var mediaDir = GetUserMediaDirectory(userId);
            var mediaFilename = Path.Combine(mediaDir, filename);

            Directory.CreateDirectory(mediaDir);

            using (var stream = mediaEntry.Open())
            using (var dest = new FileStream(mediaFilename, FileMode.OpenOrCreate))
            using (var hash = new SHA1Managed())
            {
                await stream.CopyToAsync(dest);
                var sha1 = hash.ComputeHash(stream);
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

        public async Task<Stream> GetFile(string userId, string filename)
        {
            var mediaFilename = Path.Combine(GetUserMediaDirectory(userId), filename);

            return new FileStream(mediaFilename, FileMode.Open);
        }

        public async Task<Media> RemoveFile(string userId, string filename)
        {
            var mediaFilename = Path.Combine(GetUserMediaDirectory(userId), filename);

            File.Delete(mediaFilename);

            return new Media
            {
                UserId = userId,
                Filename = filename,
            };
        }
    }
}
