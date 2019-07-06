using AnkiSyncServer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Test
{
    [TestClass]
    public class CollectionManager_LogRemovalsShould : AbstractIntegrationTest<AnkiDbContext>
    {
        [TestMethod]
        public async Task SaveRemovals()
        {
            using (var context = new AnkiDbContext(DbContextOptions))
            {
                Collection collection = new Collection
                {
                    Id = 1,
                    ClientId = 1,
                    UserId = "foo",
                    UpdateSequenceNumber = 42,
                };
                var collectionManager = new AnkiSyncServer.CollectionManager.CollectionManager(context, collection);

                Assert.AreEqual(
                    2,
                    await collectionManager.LogRemovals(new List<long>(){ 1,2 }, GraveType.Card)
                );

                Assert.IsNotNull(
                    context.Graves.SingleOrDefault(g => g.UserId == "foo" && g.UpdateSequenceNumber == 42 && g.OriginalId == 1 && g.Type == GraveType.Card)
                );

                Assert.IsNotNull(
                    context.Graves.SingleOrDefault(g => g.UserId == "foo" && g.UpdateSequenceNumber == 42 && g.OriginalId == 2 && g.Type == GraveType.Card)
                );
            }
        }

    }
}
