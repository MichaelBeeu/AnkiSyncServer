using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using AnkiSyncServer.Models;
using AnkiSyncServer.Syncer;
using AnkiSyncServer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AnkiSyncServer.Controllers.Media
{
    [Route("msync/[controller]")]
    [ApiController]
    public class UploadChangesController : ControllerBase
    {
        private IMediaSyncer mediaSyncer;
        private UserManager<ApplicationUser> userManager;
        private AnkiDbContext context;

        public UploadChangesController(
            IMediaSyncer mediaSyncer,
            UserManager<ApplicationUser> userManager,
            AnkiDbContext context
        ) {
            this.mediaSyncer = mediaSyncer;
            this.userManager = userManager;
            this.context = context;
        }
        
        /// <summary>
        /// Process media upload form client.
        /// </summary>
        /// <param name="changes">Used to upload client media files.</param>
        /// <returns>Returns the count of media files processed, and the current update sequence number.</returns>
        public async Task<IActionResult> UploadChanges([FromForm] UploadChanges changes)
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);
            MediaMeta meta = await context.MediaMeta.FirstOrDefaultAsync(m => m.User == user);

            if (meta == null)
            {
                meta = new MediaMeta
                {
                    User = user,
                    DirectoryModified = DateTimeOffset.UtcNow.DateTime,
                    LastUpdateSequenceNumber = 0,
                };
            }

            long processedCount = await mediaSyncer.Upload(user.Id, changes.Data);

            meta.LastUpdateSequenceNumber += processedCount;

            context.MediaMeta.Update(meta);

            await context.SaveChangesAsync();

            return Ok(new
            {
                data = new long[] { processedCount, meta.LastUpdateSequenceNumber },
                err = ""
            });
        }
    }
}