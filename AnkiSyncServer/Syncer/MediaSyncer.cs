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
        private IMediaManager mediaManager;
        private AnkiDbContext context;

        public MediaSyncer(
            IMediaManager mediaManager,
            AnkiDbContext context
        ) {
            this.mediaManager = mediaManager;
            this.context = context;
        }
    }
}
