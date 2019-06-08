using AnkiSyncServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.MediaManager
{
    /// <summary>
    /// Provide a common interface for Media storage engines.
    /// </summary>
    public interface IMediaManager
    {
        /// <summary>
        /// Add a file to the storage engine.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <param name="filename">Used to indicate the target filename.</param>
        /// <param name="mediaEntry">Used to provide the media file contents.</param>
        /// <returns>Returns the Media object that can be inserted into the DB.</returns>
        Task<Media> AddFile(string userId, string filename, ZipArchiveEntry mediaEntry);

        /// <summary>
        /// Used to retrieve a media file.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <param name="filename">Used to indicate the media filename.</param>
        /// <returns>Returns a Tuple containing the media stream, and last modify time.</returns>
        Task<Tuple<Stream, DateTime>> GetFile(string userId, string filename);

        /// <summary>
        /// Used to remove a file from the storage engine.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <param name="filename">Used to indicate the media filename.</param>
        /// <returns>Returns a Media object that can be used to delete the DB record.</returns>
        Task<Media> RemoveFile(string userId, string filename);
    }
}
