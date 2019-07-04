using AnkiSyncServer.Models;
using AnkiSyncServer.Models.CollectionData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.CollectionManager
{
    public interface ICollectionManager
    {
        Task<long> RemoveNotes(List<long> noteIds);
        Task<long> RemoveCards(List<long> cardIds);
        Task<long> RemoveDecks(List<long> deckIds);
        Task<long> LogRemovals(List<long> ids, GraveType type);
        Task<Deck> GetDeck(long deckId);
    }
}
