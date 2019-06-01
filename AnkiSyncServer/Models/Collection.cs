using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Models
{
    public class Collection
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime Modified { get; set; }
        public DateTime SchemaModified { get; set; }
        public int Version { get; set; }
        public int Dirty { get; set; }
        public long UpdateSequenceNumber { get; set; }
        public DateTime? LastSync { get; set; }
        public string Conf { get; set; }
        public string Models { get; set; }
        public string Decks { get; set; }
        public string DeckConf { get; set; }
        public string Tags { get; set; }

        public Collection()
        {
            CreationDate = DateTime.Now;
            Modified = DateTime.Now;
            SchemaModified = DateTime.Now;
            Version = 9;
            Dirty = 1;
            UpdateSequenceNumber = 0;
            LastSync = null;
            Conf = "[]";
            Models = "[]";
            Decks = "[]";
            DeckConf = "[]";
            Tags = "[]";
        }
    }
}
