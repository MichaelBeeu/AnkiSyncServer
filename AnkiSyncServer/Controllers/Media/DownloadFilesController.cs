using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private IMediaSyncer _mediaSyncer;
        private UserManager<ApplicationUser> _userManager { get; set; }

        public DownloadFilesController(
            IMediaSyncer mediaSyncer,
            UserManager<ApplicationUser> userManager
        ) {
            _mediaSyncer = mediaSyncer;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> DownloadFiles(MediaDownloadRequest mediaRequest)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var zipStream = await _mediaSyncer.Download(user.Id, mediaRequest.Files);

            return File(zipStream, "application/octet-stream");
        }
    }
}