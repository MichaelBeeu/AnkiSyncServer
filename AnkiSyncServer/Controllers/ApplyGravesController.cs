using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnkiSyncServer.CollectionManager;
using AnkiSyncServer.Models;
using AnkiSyncServer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnkiSyncServer.Controllers
{
    [Route("sync/[controller]")]
    [ApiController]
    public class ApplyGravesController : ControllerBase
    {
        private AnkiDbContext context;
        private UserManager<ApplicationUser> userManager;
        private ICollectionManagerFactory collectionManagerFactory;

        public ApplyGravesController(
            AnkiDbContext context,
            UserManager<ApplicationUser> userManager,
            ICollectionManagerFactory collectionManagerFactory
        ) {
            this.context = context;
            this.userManager = userManager;
            this.collectionManagerFactory = collectionManagerFactory;
        }

        public UserManager<ApplicationUser> UserManager { get; }

        public async Task<IActionResult> ApplyGraves([FromBody] ApplyGravesViewModel applyGraves)
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);

            CollectionManager.CollectionManager collectionMgr = (CollectionManager.CollectionManager)await collectionManagerFactory.Create(user.Id);

            await collectionMgr.RemoveNotes(applyGraves.Chunk.Notes);
            await collectionMgr.RemoveCards(applyGraves.Chunk.Cards);
            await collectionMgr.RemoveDecks(applyGraves.Chunk.Decks);

            /*

            Collection collection = await context.Collections
                    .Where(c => c.User == user)
                    .FirstOrDefaultAsync();

            List<long> gravedNoteIds = applyGraves.Chunk.Notes;
            List<long> gravedCardIds = applyGraves.Chunk.Cards;
            List<long> gravedDeckIds = applyGraves.Chunk.Decks;

            // Remove notes
            var gravedNotes = context.Notes
                .Where(n => n.User == user && gravedNoteIds.Contains(n.Id));

            context.Notes.RemoveRange(gravedNotes);

            // Remove cards
            var gravedCards = context.Cards
                .Where(c => c.User == user && gravedCardIds.Contains(c.Id));

            context.Cards.RemoveRange(gravedCards);

            // Handle deck removals by searching for the associated cards
            // and notes for the given decks.
            var gravedDeckCards = context.Cards
                .Where(c => c.User == user && gravedDeckIds.Contains(c.Id));

            var gravedDeckNoteIds = gravedDeckCards
                .Select(c => c.NoteId)
                .ToList<long>();

            var gravedDeckNotes = context.Notes
                .Where(n => gravedDeckNoteIds.Contains(n.Id) && n.User == user);

            context.Cards.RemoveRange(gravedDeckCards);
            context.Notes.RemoveRange(gravedDeckNotes);

            // Save the grave records
            context.AddRange(
                gravedNoteIds
                    .Select(noteId => new Grave
                    {
                        User = user,
                        UpdateSequenceNumber = collection.UpdateSequenceNumber,
                        OriginalId = noteId,
                        Type = GraveType.Note,
                    })
                );

            context.AddRange(
                gravedCardIds
                    .Select(cardId => new Grave
                    {
                        User = user,
                        UpdateSequenceNumber = collection.UpdateSequenceNumber,
                        OriginalId = cardId,
                        Type = GraveType.Card,
                    })
                );

            context.AddRange(
                gravedDeckIds
                    .Select(deckId => new Grave
                    {
                        User = user,
                        UpdateSequenceNumber = collection.UpdateSequenceNumber,
                        OriginalId = deckId,
                        Type = GraveType.Deck,
                    })
                );

            context.SaveChanges();
            */

            return Ok(new List<long>());
        }
    }
}