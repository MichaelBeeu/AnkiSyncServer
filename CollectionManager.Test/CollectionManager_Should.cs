using AnkiSyncServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnkiSyncServer.Models.CollectionData;
using System.Collections.Generic;
using AnkiSyncServer.CollectionManager;
using System.Threading.Tasks;

namespace CollectionManager.Test
{
    [TestClass]
    public class CollectionManager_Should
    {
        public TestContext TestContext { get; set; }
        private DbContextOptions<AnkiDbContext> DbContextOptions { get; set; }

        /*
        [ClassInitialize]
        public void SetupTests(TestContext testContext)
        {
            this.TestContext = testContext;
        }
        */

        [TestInitialize]
        public void Initialize()
        {
            DbContextOptions = new DbContextOptionsBuilder<AnkiDbContext>()
                .UseInMemoryDatabase(databaseName: TestContext.TestName)
                .Options;
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
                        }
                    }
                };

                var collectionManager = new AnkiSyncServer.CollectionManager.CollectionManager(context, collection);

                var deck = await collectionManager.GetDeck(1);

                Assert.AreEqual(deck.Name, "Default");
            }
        }
    }
}
