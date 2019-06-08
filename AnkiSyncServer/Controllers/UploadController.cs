using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnkiSyncServer.Syncer;
using AnkiSyncServer.ViewModels;
using AnkiSyncServer.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace AnkiSyncServer.Controllers
{
    [Route("sync/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private AnkiDbContext context;
        private UserManager<ApplicationUser> userManager;
        private IFullSyncer fullSyncer;

        public UploadController(
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
        /// Used to upload client database.
        /// </summary>
        /// <param name="upload">Used to communcate client database file.</param>
        /// <returns>Returns Ok</returns>
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] DbUploadViewModel upload)
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);

            await fullSyncer.Upload(user.Id, upload.Data);

            return Ok();
        }
    }
}