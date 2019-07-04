using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AnkiSyncServer.ViewModels
{
    [DataContract]
    public class ApplyGravesViewModel
    {
        [DataMember(Name = "chunk")]
        public GraveChunk Chunk { get; set; }
    }
}
