using AnkiSyncServer.Models;
using AnkiSyncServer.Models.CollectionData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionManager.Test
{
    [TestClass]
    public class CollectionManager_GetDeckByNameShould : AbstractIntegrationTest<AnkiDbContext>
    {
        [TestMethod]
        public void ReturnCorrectDeck()
        {
            using (var context = new AnkiDbContext(DbContextOptions))
            {
                Collection collection = new Collection()
                {
                    Decks = new Dictionary<long, Deck>() {
                        {
                            1,
                            new Deck
                            {
                                Name = "Default",
                            }
                        },
                        {
                            2,
                            new Deck
                            {
                                Name = "Default::foo"
                            }
                        }
                    },
                };

                var collectionManager = new AnkiSyncServer.CollectionManager.CollectionManager(context, collection);

                var deck = collectionManager.GetDeckByName("Default");

                Assert.AreEqual("Default", deck.Name);

                deck = collectionManager.GetDeckByName("Default::foo");

                Assert.AreEqual("Default::foo", deck.Name);
            }
        }
    }
}
