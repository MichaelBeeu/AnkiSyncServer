using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Models
{
    public enum CardType : int
    {
        New = 0,
        Learning = 1,
        Due = 2,
        Filtered = 3,
    }
}
