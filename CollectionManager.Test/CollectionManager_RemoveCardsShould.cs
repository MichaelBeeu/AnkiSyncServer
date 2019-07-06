using AnkiSyncServer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using CollectionManager.Test.Fixtures;
using System.Linq;
using IntegrationTest;

namespace CollectionManager.Test
{
    [TestClass]
    public class CollectionManager_RemoveCardsShould : AbstractIntegrationTest<AnkiDbContext>
    {
        [TestMethod]
        [WithFixture(typeof(CardsFixture))]
        [WithFixture(typeof(NotesFixture))]
        public async Task DeleteCardsById()
        {
            using (var context = new AnkiDbContext(DbContextOptions))
            {
                Collection collection = new Collection
                {
                    Id = 1,
                    ClientId = 1,
                    UserId = "foo",
                };
                var collectionManager = new AnkiSyncServer.CollectionManager.CollectionManager(context, collection);

                var cardsRemoved = await collectionManager.RemoveCards(new List<long>() { 1, 2 });

                Assert.AreEqual(4, cardsRemoved);

                Assert.IsNull(
                    context.Cards.SingleOrDefault(c => c.ClientId == 1 && c.UserId == "foo")
                );

                Assert.IsNull(
                    context.Cards.SingleOrDefault(c => c.ClientId == 2 && c.UserId == "foo")
                );
            }
        }

        [TestMethod]
        [WithFixture(typeof(CardsFixture))]
        [WithFixture(typeof(NotesFixture))]
        public async Task LogRemovals()
        {
            using (var context = new AnkiDbContext(DbContextOptions))
            {
                Collection collection = new Collection
                {
                    Id = 1,
                    ClientId = 1,
                    UserId = "foo",
                };
                var collectionManager = new AnkiSyncServer.CollectionManager.CollectionManager(context, collection);

                var cardsRemoved = await collectionManager.RemoveCards(new List<long>() { 1, 2 });

                Assert.AreEqual(4, cardsRemoved);

                Assert.IsNotNull(
                    context.Graves.SingleOrDefault(g => g.OriginalId == 1 && g.UserId == "foo" && g.Type == GraveType.Card)
                );

                Assert.IsNotNull(
                    context.Graves.SingleOrDefault(g => g.OriginalId == 2 && g.UserId == "foo" && g.Type == GraveType.Card)
                );
            }
        }

        [TestMethod]
        [WithFixture(typeof(CardsFixture))]
        [WithFixture(typeof(NotesFixture))]
        public async Task DeleteNotes()
        {
            using (var context = new AnkiDbContext(DbContextOptions))
            {
                Collection collection = new Collection
                {
                    Id = 1,
                    ClientId = 1,
                    UserId = "foo",
                };
                var collectionManager = new AnkiSyncServer.CollectionManager.CollectionManager(context, collection);

                var cardsRemoved = await collectionManager.RemoveCards(new List<long>() { 1, 2 });

                Assert.AreEqual(4, cardsRemoved);

                Assert.IsNull(
                    context.Notes.SingleOrDefault(c => c.ClientId == 1 && c.UserId == "foo")
                );

                Assert.IsNull(
                    context.Notes.SingleOrDefault(c => c.ClientId == 2 && c.UserId == "foo")
                );
            }
        }
    }
}
