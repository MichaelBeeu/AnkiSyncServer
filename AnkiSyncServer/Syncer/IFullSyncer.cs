using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Syncer
{
    public interface IFullSyncer
    {
        /// <summary>
        /// Upload a user's SQLite database.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <param name="input">Used to provide the client database.</param>
        /// <returns>Returns success or failure.</returns>
        Task<Boolean> Upload(string userId, IFormFile input);
        
        /// <summary>
        /// Used to create a SQLite database of client data to provide for download.
        /// </summary>
        /// <param name="userId">Used to indicate the owning user.</param>
        /// <returns>Returns the filename of the temporary database.</returns>
        Task<string> Download(string userId);
    }
}
