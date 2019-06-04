using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.ViewModels
{
    public class MediaSanity
    {
        [FromForm(Name = "local")]
        public long Local { get; set; }
    }
}
