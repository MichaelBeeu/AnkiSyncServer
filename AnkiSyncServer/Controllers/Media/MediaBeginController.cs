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
    [Route("msync/begin")]
    [ApiController]
    public class MediaBeginController : ControllerBase
    {
        private AnkiDbContext _context { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }

        public MediaBeginController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager
        ) {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Begin()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var meta = await _context.MediaMeta
                .FirstOrDefaultAsync(m => m.User == user);

            if (meta == null)
            {
                meta = new MediaMeta
                {
                    LastUpdateSequenceNumber = 1,
                };
            }

            var form = HttpContext.Request.Form;
            string hkey = form["k"];

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