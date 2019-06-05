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
        private AnkiDbContext _context { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }
        private IFullSyncer _fullSyncer { get; set; }

        public UploadController(
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
        public async Task<IActionResult> Upload([FromForm] DbUploadViewModel upload)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            await _fullSyncer.Upload(user.Id, upload.Data);

            return Ok();
        }
    }
}