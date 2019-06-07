using AnkiSyncServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.MediaManager
{
    public interface IMediaManager
    {
        Task<Media> AddFile(string userId, string filename, ZipArchiveEntry mediaEntry);
        Task<Tuple<Stream, DateTime>> GetFile(string userId, string filename);
        Task<Media> RemoveFile(string userId, string filename);
    }
}
