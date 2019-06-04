using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Models
{
    public class Media
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Filename { get; set; }
        public string Checksum { get; set; }
        public DateTime Modified { get; set; }
        public int Dirty { get; set; }
    }
}
