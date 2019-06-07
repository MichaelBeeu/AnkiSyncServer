using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnkiSyncServer.Models;
using AnkiSyncServer.Syncer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnkiSyncServer.Controllers
{
    [Route("sync/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private AnkiDbContext _context { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }
        private IFullSyncer _fullSyncer { get; set; }

        public DownloadController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager,
            IFullSyncer fullSyncer
            )
        {
            _context = context;
            _userManager = userManager;
            _fullSyncer = fullSyncer;
        }
        
        [HttpPost]
        public async Task<IActionResult> Download()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var dbFile = await _fullSyncer.Download(user.Id);

            var stream = new FileStream(dbFile, FileMode.Open);

            return File(stream, "application/octet-stream");
        }
    }
}