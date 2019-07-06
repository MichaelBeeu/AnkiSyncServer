using AnkiSyncServer.Models;
using CollectionManager.Test.Fixtures;
using IntegrationTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Test
{
    [TestClass]
    public class CollectionManager_RemoveNotesShould : AbstractIntegrationTest<AnkiDbContext>
    {
        [TestMethod]
        [WithFixture(typeof(NotesFixture))]
        public async Task DeleteNotesById()
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

                var notesRemoved = await collectionManager.RemoveNotes(new List<long>() { 1, 2 });

                Assert.AreEqual(2, notesRemoved);

                Assert.IsNull(
                    context.Notes.SingleOrDefault( n => n.ClientId == 1 && n.UserId == "foo" )
                );

                Assert.IsNull(
                    context.Notes.SingleOrDefault( n => n.ClientId == 2 && n.UserId == "foo" )
                );

                Assert.IsNotNull(
                    context.Notes.SingleOrDefault(n => n.ClientId == 3 && n.UserId == "foo")
                );

                Assert.IsNotNull(
                    context.Notes.SingleOrDefault(n => n.ClientId == 1 && n.UserId == "bar")
                );

                Assert.IsNotNull(
                    context.Notes.SingleOrDefault(n => n.ClientId == 2 && n.UserId == "bar")
                );
            }
        }

        [TestMethod]
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

                var notesRemoved = await collectionManager.RemoveNotes(new List<long>() { 1, 2 });

                Assert.IsNotNull(
                    context.Graves.SingleOrDefault(g => g.OriginalId == 1 && g.UserId == "foo" && g.Type == GraveType.Note)
                );

                Assert.IsNotNull(
                    context.Graves.SingleOrDefault(g => g.OriginalId == 2 && g.UserId == "foo" && g.Type == GraveType.Note)
                );
            }
        }
    }
}
