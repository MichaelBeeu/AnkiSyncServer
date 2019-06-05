using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Syncer
{
    public interface IMediaSyncer
    {
        Task<long> Upload(string userId, IFormFile data);
    }
}
