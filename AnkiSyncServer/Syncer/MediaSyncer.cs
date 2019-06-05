using AnkiSyncServer.MediaManager;
using AnkiSyncServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Syncer
{
    public partial class MediaSyncer : IMediaSyncer
    {
        private IMediaManager _mediaManager;
        private AnkiDbContext _context;

        public MediaSyncer(
            IMediaManager mediaManager,
            AnkiDbContext context
        ) {
            _mediaManager = mediaManager;
            _context = context;
        }
    }
}
