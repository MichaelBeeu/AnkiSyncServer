using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.ViewModels
{
    public class UploadChanges
    {
        [FromForm(Name = "data")]
        public IFormFile Data { get; set; }
    }
}
