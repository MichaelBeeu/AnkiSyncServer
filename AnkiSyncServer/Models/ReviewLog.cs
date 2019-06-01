using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Models
{
    public class ReviewLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public long CardId { get; set; }
        public long UpdateSequenceNumber { get; set; }
        public int Ease { get; set; }
        public long Interval { get; set; }
        public long LastInterval { get; set; }
        public long Factor { get; set; }
        public long Time { get; set; }
        public ReviewType Type { get; set; }
    }
}
