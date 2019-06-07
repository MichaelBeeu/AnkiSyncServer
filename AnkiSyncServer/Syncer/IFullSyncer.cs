using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Syncer
{
    public interface IFullSyncer
    {
        Task<Boolean> Upload(string userId, IFormFile input);
        Task<string> Download(string userId);
    }
}
