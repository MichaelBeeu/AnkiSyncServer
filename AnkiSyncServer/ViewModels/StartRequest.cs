using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AnkiSyncServer.ViewModels
{
    [DataContract]
    public class StartRequest
    {
        [DataMember(Name = "minUsn")]
        public long MinimumUpdateSequenceNumber { get; set; }
        [DataMember(Name = "lnewer")]
        public Boolean LNewer { get; set; }
    }
}
