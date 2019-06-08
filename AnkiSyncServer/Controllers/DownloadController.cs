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
        private AnkiDbContext context;
        private UserManager<ApplicationUser> userManager;
        private IFullSyncer fullSyncer;

        public DownloadController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager,
            IFullSyncer fullSyncer
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.fullSyncer = fullSyncer;
        }
        
        /// <summary>
        /// Download SQLite database of client data.
        /// </summary>
        /// <returns>Returns FileStreamResult of SQLite database of client data.</returns>
        [HttpPost]
        public async Task<IActionResult> Download()
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);

            string dbFile = await fullSyncer.Download(user.Id);

            FileStream stream = new FileStream(dbFile, FileMode.Open);

            return File(stream, "application/octet-stream");
        }
    }
}