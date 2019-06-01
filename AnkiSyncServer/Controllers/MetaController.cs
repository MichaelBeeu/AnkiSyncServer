using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiSyncServer.Models;

namespace AnkiSyncServer.Controllers
{
    [Route("sync/[controller]")]
    [ApiController]
    public class MetaController : ControllerBase
    {
        private AnkiDbContext _context { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }

        public MetaController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var collection = await _context.Collections
                .FirstOrDefaultAsync(c => c.User == user);

            if (collection == null)
            {
                collection = new Collection();
            }

            Console.WriteLine(user);
            Console.WriteLine(user.Id);

            return Ok(new
            {
                scm = ((DateTimeOffset)collection.SchemaModified).ToUnixTimeSeconds(),
                ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                mod = ((DateTimeOffset)collection.Modified).ToUnixTimeSeconds(),
                usn = collection.UpdateSequenceNumber,
                musn = 0,
                msg = "",
                cont = true,
            });
        }
    }
}