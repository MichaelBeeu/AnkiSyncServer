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
        private IMediaSyncer _mediaSyncer { get; set; }
        private UserManager<ApplicationUser> _userManager;
        private AnkiDbContext _context;

        public UploadChangesController(
            IMediaSyncer mediaSyncer,
            UserManager<ApplicationUser> userManager,
            AnkiDbContext context
        ) {
            _mediaSyncer = mediaSyncer;
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> UploadChanges([FromForm] UploadChanges changes)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var meta = await _context.MediaMeta.FirstOrDefaultAsync(m => m.User == user);

            if (meta == null)
            {
                meta = new MediaMeta
                {
                    User = user,
                    DirectoryModified = DateTimeOffset.UtcNow.DateTime,
                    LastUpdateSequenceNumber = 0,
                };
            }

            var processedCount = await _mediaSyncer.Upload(user.Id, changes.Data);

            meta.LastUpdateSequenceNumber += processedCount;

            _context.MediaMeta.Update(meta);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                data = new long[] { processedCount, meta.LastUpdateSequenceNumber },
                err = ""
            });
        }
    }
}