using AnkiSyncServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Syncer
{
    public partial class FullSyncer : IFullSyncer
    {
        private AnkiDbContext context;

        public FullSyncer(
            AnkiDbContext context
            )
        {
            this.context = context;
        }
    }
}
