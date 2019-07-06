using AnkiSyncServer.Models;
using AnkiSyncServer.Models.CollectionData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.CollectionManager
{
    /// <summary>
    /// Manages a user's collection, and associated objects.
    /// </summary>
    public class CollectionManager : ICollectionManager
    {
        private AnkiDbContext context;
        private Collection collection;

        public CollectionManager(
            AnkiDbContext context,
            Collection collection
        ) {
            this.context = context;
            this.collection = collection;
        }

        /// <summary>
        /// Removes the specified note ids from the collection, and logs them in
        /// the graves table.
        /// </summary>
        /// <param name="noteIds">The list of note ids to remove.</param>
        /// <returns>The number of notes removed.</returns>
        public async Task<long> RemoveNotes(List<long> noteIds)
        {
            var notes = context.Notes
                .Where(n => noteIds.Contains(n.ClientId) && n.UserId == collection.UserId);

            await LogRemovals(noteIds, GraveType.Note);

            context.RemoveRange(notes);

            long notesCount = notes.Count();

            await context.SaveChangesAsync();

            return notesCount;
        }

        /// <summary>
        /// Removes the specified card ids and related notes from the
        /// collection, logging the removals into the graves table.
        /// </summary>
        /// <param name="cardIds">The list of card ids to remove.</param>
        /// <returns>The number of cards and notes removed.</returns>
        public async Task<long> RemoveCards(List<long> cardIds)
        {
            var cards = context.Cards
                .Where(c => cardIds.Contains(c.Id) && c.UserId == collection.UserId);

            var noteIds = cards.Select(c => c.NoteId).ToList<long>();

            await LogRemovals(cardIds, GraveType.Card);

            context.RemoveRange(cards);

            long cardsCount = cards.Count();

            long notesCount = await RemoveNotes(noteIds);

            return cardsCount + notesCount;
        }
        
        /// <summary>
        /// Removes the specified deck ids, and associated cards and notes.
        /// </summary>
        /// <param name="deckIds">The list of deck ids to remove.</param>
        /// <returns>The number of cards and notes removed.</returns>
        public async Task<long> RemoveDecks(List<long> deckIds) //, bool cardsToo = false, bool childrenToo = true)
        {
            // Find all cards within this deck
            var cards = context.Cards
                .Where(c => deckIds.Contains(c.DeckId) || deckIds.Contains(c.OriginalDeckId));

            // Separate into card and note ids.
            var cardIds = cards.Select(c => c.Id).ToList<long>();
            var noteIds = cards.Select(n => n.NoteId).ToList<long>();

            // Find all notes within this deck, or owned by these cards.
            var notes = context.Cards
                .Where(n => deckIds.Contains(n.DeckId) || noteIds.Contains(n.Id));

            // Add all note ids into the same list
            noteIds.AddRange(
                notes.Select(n => n.Id).ToList<long>()
            );

            // Finally, remove all cards and notes.
            long numCardsRemoved = await RemoveCards(cardIds);
            long numNotesRemoved = await RemoveNotes(noteIds);

            await LogRemovals(deckIds, GraveType.Deck);

            return numCardsRemoved + numNotesRemoved;
        }

        public async Task<long> RemoveDeck(long deckId, bool cardsToo = false, bool childrenToo = true)
        {
            Deck deck = await GetDeck(deckId);
            // Don't delete the default deck, but rename it instead.
            if (deckId == 1)
            {
                if (deck.Name.Contains("::"))
                {
                    string baseName = deck.Name.Substring(deck.Name.LastIndexOf("::"));
                    int suffix = 0;

                    while (true)
                    {
                        string newName = baseName;
                        if (suffix != 0)
                        {
                            newName.Concat(suffix.ToString());
                        }
                        
                        if (GetDeckByName(newName) == null)
                        {
                            deck.Name = newName;
                            // Save here?
                            break;
                        }

                        suffix++;
                    }
                }
                return 1;
            }

            await LogRemovals(new List<long>() { deckId }, GraveType.Deck);

            if (!collection.Decks.ContainsKey(deckId))
            {
                return 0;
            }

            //Deck deck = GetDeck(deckId);

            return 1;
        }

        /// <summary>
        /// Add grave records for the provided ids, and with the specified type.
        /// </summary>
        /// <param name="ids">The original ids to add to the graves table.</param>
        /// <param name="type">The type of grave.</param>
        /// <returns>The number of graves added.</returns>
        public async Task<long> LogRemovals(List<long> ids, GraveType type)
        {
            var graves = ids.Select(id => new Grave
            {
                UserId = collection.UserId,
                UpdateSequenceNumber = collection.UpdateSequenceNumber,
                OriginalId = id,
                Type = type,
            });

            context.Graves.AddRange(graves);

            await context.SaveChangesAsync();

            return graves.Count();
        }

        public async Task<Deck> GetDeck(long deckId)
        {
            if (collection.Decks.ContainsKey(deckId))
            {
                return collection.Decks[deckId];
            }

            return null;
            /*
            collection.Decks[deckId] = new Deck();

            return collection.Decks[deckId];
            */
        }

        public Deck GetDeckByName(string name)
        {
            return collection.Decks
                .Where(d => d.Value.Name == name)
                .Select(d => d.Value)
                .FirstOrDefault();
        }

        public bool RemoveDeck(long deckId)
        {
            return collection.Decks.Remove(deckId);
        }
    }
}
