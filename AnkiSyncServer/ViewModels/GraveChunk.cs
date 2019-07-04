using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AnkiSyncServer.ViewModels
{
    [DataContract]
    public class GraveChunk
    {
        [DataMember(Name = "notes")]
        public List<long> Notes { get; set; }

        [DataMember(Name = "cards")]
        public List<long> Cards { get; set; }

        [DataMember(Name = "decks")]
        public List<long> Decks { get; set; }
    }
}