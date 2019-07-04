using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnkiSyncServer.Models;
using AnkiSyncServer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnkiSyncServer.Controllers
{
    [Route("sync/[controller]")]
    [ApiController]
    public class StartController : ControllerBase
    {
        private AnkiDbContext context;
        private UserManager<ApplicationUser> userManager;

        public StartController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager
        ) {
            this.context = context;
            this.userManager = userManager;
        }

        /// <summary>
        /// Initiate start process, and communicate graved objects.
        /// </summary>
        /// <param name="startRequest">Used to indicate the minumim client USN.</param>
        /// <returns>Returns a list of graved objects.</returns>
        public async Task<IActionResult> Start([FromBody] StartRequest startRequest)
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);

            Collection collection = context.Collections
                .FirstOrDefault(c => c.User == user);

            long maxUsn = collection.UpdateSequenceNumber;
            long minUsn = startRequest.MinimumUpdateSequenceNumber;

            // Assemble list of 'graved' objects
            // These are any graves with a USN greater or equal to the min usn
            var graves = new
            {
                cards = context.Graves.Where(g => g.User == user && g.UpdateSequenceNumber >= minUsn && g.Type == GraveType.Card).Select(g => g.OriginalId),
                notes = context.Graves.Where(g => g.User == user && g.UpdateSequenceNumber >= minUsn && g.Type == GraveType.Note).Select(g => g.OriginalId),
                decks = context.Graves.Where(g => g.User == user && g.UpdateSequenceNumber >= minUsn && g.Type == GraveType.Deck).Select(g => g.OriginalId),
            };

            return Ok(graves);
        }
    }
}