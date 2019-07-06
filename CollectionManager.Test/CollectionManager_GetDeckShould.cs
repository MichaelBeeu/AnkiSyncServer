using AnkiSyncServer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnkiSyncServer.Models.CollectionData;
using System.Collections.Generic;
using AnkiSyncServer.CollectionManager;
using System.Threading.Tasks;

namespace CollectionManager.Test
{
    [TestClass]
    public class CollectionManager_GetDeckShould : AbstractIntegrationTest<AnkiDbContext>
    {
        public static IEnumerable<object> DataFixture()
        {
            yield return new Collection
                {
                    Id = 1,
                    ClientId = 1,
                    UserId = "1",
                    Decks = new Dictionary<long, Deck>()
                    {
                        {
                            1,
                            new Deck {
                                Name = "Default",
                            }
                        },
                        {
                            2,
                            new Deck {
                                Name = "Default::foo",
                            }
                        }
                    }
                };
        }

        [TestMethod]
        public async Task ReturnDeckViaId()
        {
            using (var context = new AnkiDbContext(DbContextOptions))
            {
                Collection collection = new Collection
                {
                    Id = 1,
                    ClientId = 1,
                    UserId = "1",
                    Decks = new Dictionary<long, Deck>()
                    {
                        {
                            1,
                            new Deck {
                                Name = "Default",
                            }
                        },
                        {
                            2,
                            new Deck {
                                Name = "Default::foo",
                            }
                        }
                    }
                };

                var collectionManager = new AnkiSyncServer.CollectionManager.CollectionManager(context, collection);

                var deck = await collectionManager.GetDeck(1);

                Assert.AreEqual(deck.Name, "Default");
            }
        }

        [TestMethod]
        public async Task ReturnNullIfInvalidId()
        {
            using (var context = new AnkiDbContext(DbContextOptions))
            {
                Collection collection = new Collection
                {
                    Id = 1,
                    ClientId = 1,
                    UserId = "1",
                    Decks = new Dictionary<long, Deck>()
                    {
                        {
                            1,
                            new Deck {
                                Name = "Default",
                            }
                        },
                        {
                            2,
                            new Deck {
                                Name = "Default::foo",
                            }
                        }
                    }
                };

                var collectionManager = new AnkiSyncServer.CollectionManager.CollectionManager(context, collection);

                var deck = await collectionManager.GetDeck(42);

                Assert.IsNull(deck);
            }
        }

    }
}
