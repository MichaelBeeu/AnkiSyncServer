using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AnkiSyncServer.Models.AccountViewModels
{
    public class DbUploadViewModel
    {
        [FromForm(Name = "data")]
        public IFormFile Data { get; set; }
        [FromForm(Name = "v")]
        public string Version { get; set; }
    }
}
