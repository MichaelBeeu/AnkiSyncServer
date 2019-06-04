using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnkiSyncServer.Models;
using AnkiSyncServer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnkiSyncServer.Controllers.Msync
{
    [Route("msync/[controller]")]
    [ApiController]
    public class MediaChangesController : ControllerBase
    {
        AnkiDbContext _context { get; set; }
        UserManager<ApplicationUser> _userManager { get; set; }

        public MediaChangesController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> MediaChanges([FromBody] MediaChanges mediaChanges)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var meta = await _context.MediaMeta
                .FirstOrDefaultAsync(m => m.User == user);
            
            if (mediaChanges.LastUpdateSequenceNumber == 0 || mediaChanges.LastUpdateSequenceNumber < meta.LastUpdateSequenceNumber)
            {
                var media = _context.Media
                    .Where(me => me.User == user)
                    .Select(med => new
                    {
                        fname = med.Filename,
                        mtime = ((DateTimeOffset)med.Modified).ToUnixTimeSeconds(),
                        csum = med.Checksum,
                    });

                return Ok(new
                {
                    data = media,
                    err = "",
                });
            }

            return Ok(new
            {
                data = new List<Object>(),
                err = "",
            });
        }
    }
}