using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiSyncServer.Models;

namespace AnkiSyncServer.Controllers
{
    [Route("sync/[controller]")]
    [ApiController]
    public class MetaController : ControllerBase
    {
        private AnkiDbContext context;
        private UserManager<ApplicationUser> userManager;

        public MetaController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);
            Collection collection = await context.Collections
                .FirstOrDefaultAsync(c => c.User == user);
            MediaMeta media = await context.MediaMeta
                .FirstOrDefaultAsync(m => m.User == user);

            if (collection == null)
            {
                collection = new Collection();
            }
            if (media == null)
            {
                media = new MediaMeta
                {
                    LastUpdateSequenceNumber = 1,
                };
            }

            return Ok(new
            {
                scm = ((DateTimeOffset)collection.SchemaModified.ToLocalTime()).ToUnixTimeMilliseconds(),
                ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                mod = ((DateTimeOffset)collection.Modified.ToLocalTime()).ToUnixTimeMilliseconds(),
                usn = collection.UpdateSequenceNumber,
                musn = media.LastUpdateSequenceNumber,
                msg = "",
                cont = true,
            });
        }
    }
}