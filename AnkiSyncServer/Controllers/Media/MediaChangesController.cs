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
        AnkiDbContext context;
        UserManager<ApplicationUser> userManager;

        public MediaChangesController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            this.context = context;
            this.userManager = userManager;
        }


        /// <summary>
        /// Returns a list of changed media files.
        /// </summary>
        /// <param name="mediaChanges">Used to indicate the client's current media status.</param>
        /// <returns>Returns JSON response containing the list of changed media files.</returns>
        [HttpPost]
        public async Task<IActionResult> MediaChanges([FromBody] MediaChanges mediaChanges)
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);
            MediaMeta meta = await context.MediaMeta
                .FirstOrDefaultAsync(m => m.User == user);
            
            if (mediaChanges.LastUpdateSequenceNumber == 0 || mediaChanges.LastUpdateSequenceNumber < meta.LastUpdateSequenceNumber)
            {
                var media = context.Media
                    .Where(me => me.User == user)
                    .Select(med => new object[]
                    {
                        med.Filename,
                        meta.LastUpdateSequenceNumber,
                        med.Checksum,
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