using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using AnkiSyncServer.Syncer;
using AnkiSyncServer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AnkiSyncServer.Controllers.Media
{
    [Route("msync/[controller]")]
    [ApiController]
    public class UploadChangesController : ControllerBase
    {
        private IMediaSyncer _mediaSyncer { get; set; }

        public UploadChangesController(
            IMediaSyncer mediaSyncer
            )
        {
            _mediaSyncer = mediaSyncer;
        }
        public async Task<IActionResult> UploadChanges([FromForm] UploadChanges media)
        {
            await _mediaSyncer.Upload(media.Data);

            return Ok();
        }
    }
}