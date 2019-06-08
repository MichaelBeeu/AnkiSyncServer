using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnkiSyncServer.Models;
using AnkiSyncServer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnkiSyncServer.Controllers.Media
{
    [Route("msync/[controller]")]
    [ApiController]
    public class MediaSanityController : ControllerBase
    {
        private AnkiDbContext context;
        private UserManager<ApplicationUser> userManager;

        public MediaSanityController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager
        ) {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> MediaSanity([FromBody] MediaSanity mediaSanity)
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);
            long count = context.Media
                .Where(m => m.Checksum != null && m.User == user)
                .Count();

            return Ok(new
            {
                data = count == mediaSanity.Local ? "OK" : "FAILED",
                err = "",
            });
        }
    }
}