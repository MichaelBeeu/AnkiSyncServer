using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Models
{
    public class Note
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid Guid { get; set; }
        public long ModelId { get; set; }
        public DateTime Modified { get; set; }
        public long UpddateSequenceNumber { get; set; }
        public string Tags { get; set; }
        public string Fields { get; set; }
        public string SortField { get; set; }
        public long Checksum { get; set; }
        public long Flags { get; set; }
        public string Data { get; set; }
    }
}
