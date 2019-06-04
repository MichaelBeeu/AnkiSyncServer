using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AnkiSyncServer.Models
{
    public class AnkiDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Grave> Graves { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<ReviewLog> ReviewLogs { get; set; }
        public DbSet<MediaMeta> MediaMeta { get; set; }
        public DbSet<Media> Media { get; set; }

        public AnkiDbContext(DbContextOptions<AnkiDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }
}
