using AnkiSyncServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Syncer
{
    public partial class FullSyncer : IFullSyncer
    {
        private AnkiDbContext _context { get; set; }

        public FullSyncer(
            AnkiDbContext context
            )
        {
            _context = context;
        }
    }
}
