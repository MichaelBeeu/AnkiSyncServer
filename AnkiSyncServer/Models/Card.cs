using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.Models
{
    public class Card
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public long NoteId { get; set; }
        public long DeckId { get; set; }
        public int Ordinal { get; set; }
        public DateTime Modified { get; set; }
        public long UpdateSequenceNumber { get; set; }
        public CardType Type { get; set; }
        public QueueType Queue { get; set; }
        public int Due { get; set; }
        public int Interval { get; set; }
        public int Factor { get; set; }
        public int Repetitions { get; set; }
        public int Lapses { get; set; }
        public int Left { get; set; }
        public DateTime OriginalDue { get; set; }
        public int OriginalDeckId { get; set; }
        public int Flags { get; set; }
        public string Data { get; set; }
    }
}
