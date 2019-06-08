using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Syncer
{
    public interface IMediaSyncer
    {
        /// <summary>
        /// Used to upload client media.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <param name="data">Used to provide client media archive.</param>
        /// <returns>Returns number of media files affected.</returns>
        Task<long> Upload(string userId, IFormFile data);

        /// <summary>
        /// Used to create zip archive of media files.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <param name="fileList">Used to indicate the list of requested files.</param>
        /// <returns>Returns a stream of the zip archive.</returns>
        Task<Stream> Download(string userId, List<string> fileList);
    }
}
