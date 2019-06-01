using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Models
{
    public enum QueueType : int
    {
        SchedBuried = -3,
        UserBuried = -2,
        Suspended = -1,
        New = 0,
        Learning = 1,
        Due = 2,
        InLearning = 3
    }
}
