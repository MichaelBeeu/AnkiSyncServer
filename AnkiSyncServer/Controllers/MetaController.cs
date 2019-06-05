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
        private AnkiDbContext _context { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }

        public MetaController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var collection = await _context.Collections
                .FirstOrDefaultAsync(c => c.User == user);
            var media = await _context.MediaMeta
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