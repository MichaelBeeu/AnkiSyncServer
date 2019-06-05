using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Syncer
{
    public interface IMediaSyncer
    {
        Task<Boolean> Upload(IFormFile data);
    }
}
