using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnkiSyncServer.Models;
using AnkiSyncServer.Syncer;
using AnkiSyncServer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnkiSyncServer.Controllers.Media
{
    [Route("msync/[controller]")]
    [ApiController]
    public class DownloadFilesController : ControllerBase
    {
        private IMediaSyncer mediaSyncer;
        private UserManager<ApplicationUser> userManager;

        public DownloadFilesController(
            IMediaSyncer mediaSyncer,
            UserManager<ApplicationUser> userManager
        ) {
            this.mediaSyncer = mediaSyncer;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> DownloadFiles(MediaDownloadRequest mediaRequest)
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);

            Stream zipStream = await mediaSyncer.Download(user.Id, mediaRequest.Files);

            return File(zipStream, "application/octet-stream");
        }
    }
}