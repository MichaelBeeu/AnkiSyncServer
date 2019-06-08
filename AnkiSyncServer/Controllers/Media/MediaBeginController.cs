using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnkiSyncServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnkiSyncServer.Controllers.Msync
{
    [Route("msync/[controller]")]
    [ApiController]
    public class BeginController : ControllerBase
    {
        private AnkiDbContext context;
        private UserManager<ApplicationUser> userManager;

        public BeginController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager
        ) {
            this.context = context;
            this.userManager = userManager;
        }

        /// <summary>
        /// Begin media upload.
        /// </summary>
        /// <returns>Returns JSON response containing current media metadata, and session key.</returns>
        [HttpPost]
        public async Task<IActionResult> Begin()
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);

            MediaMeta meta = await context.MediaMeta
                .FirstOrDefaultAsync(m => m.User == user);

            if (meta == null)
            {
                meta = new MediaMeta
                {
                    User = user,
                    LastUpdateSequenceNumber = 1,
                };
            }

            string hkey = HttpContext.Request.Form["k"];

            return Ok(new
            {
                data = new
                {
                    sk = hkey,
                    usn = meta.LastUpdateSequenceNumber,
                },
                err = "",
            });
        }
    }
}