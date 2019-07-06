using AnkiSyncServer.Models;
using IntegrationTest;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionManager.Test.Fixtures
{
    public class CardsFixture : AbstractDataFixture
    {
        override public IEnumerable<object> GetData()
        {
            yield return new Card
            {
                Id = 1,
                ClientId = 1,
                UserId = "foo",
                NoteId = 1,
                DeckId = 1,
            };

            yield return new Card
            {
                Id = 2,
                ClientId = 2,
                UserId = "foo",
                NoteId = 2,
                DeckId = 1,
            };

            yield return new Card
            {
                Id = 3,
                ClientId = 3,
                UserId = "foo",
                NoteId = 3,
                DeckId = 1,
            };

            yield return new Card
            {
                Id = 4,
                ClientId = 1,
                UserId = "bar",
                NoteId = 1,
                DeckId = 1,
            };

            yield return new Card
            {
                Id = 5,
                ClientId = 2,
                UserId = "bar",
                NoteId = 2,
                DeckId = 1,
            };
        }
    }
}
