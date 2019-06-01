using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Models
{
    public enum ReviewType: int
    {
        Learn = 0,
        Review = 0,
        Relearn = 2,
        Cram = 3,
    }
}
