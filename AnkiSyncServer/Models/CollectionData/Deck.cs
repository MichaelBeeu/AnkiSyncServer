using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AnkiSyncServer.Models.CollectionData
{
    [DataContract]
    [JsonObject]
    public class Deck
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "extendedRev")]
        public long ExtendedReview { get; set; }

        [DataMember(Name = "usn")]
        public long UpdateSequenceNumber { get; set; }

        [DataMember(Name = "collapsed")]
        public bool Collapsed { get; set; }

        [DataMember(Name = "browserCollapsed")]
        public bool BrowserCollapsed { get; set; }

        [DataMember(Name = "newToday")]
        public IList<long> NewToday { get; set; }

        [DataMember(Name = "revToday")]
        public IList<long> ReviewToday { get; set; }

        [DataMember(Name = "lrnToday")]
        public IList<long> LearnToday { get; set; }

        [DataMember(Name = "timeToday")]
        public IList<long> TimeToday { get; set; }

        [DataMember(Name = "dyn")]
        public bool Dynamic { get; set; }

        [DataMember(Name = "extendNew")]
        public long ExtendNew { get; set; }

        [DataMember(Name = "conf")]
        public long ConfId { get; set; }

        [DataMember(Name = "id")]
        public long DeckId { get; set; }

        [DataMember(Name = "mod")]
        public long Modified { get; set; }

        [DataMember(Name = "desc")]
        public string Description { get; set; }
    }
}
