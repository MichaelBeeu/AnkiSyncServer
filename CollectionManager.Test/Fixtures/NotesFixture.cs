using AnkiSyncServer.Models;
using IntegrationTest;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionManager.Test.Fixtures
{
    class NotesFixture : AbstractDataFixture
    {

        override public IEnumerable<object> GetData()
        {
            /**
             * Note data
             */
            yield return new Note
            {
                Id = 1,
                ClientId = 1,
                UserId = "foo",
            };

            yield return new Note
            {
                Id = 2,
                ClientId = 2,
                UserId = "foo",
            };

            yield return new Note
            {
                Id = 3,
                ClientId = 3,
                UserId = "foo",
            };

            yield return new Note
            {
                Id = 4,
                ClientId = 1,
                UserId = "bar",
            };

            yield return new Note
            {
                Id = 5,
                ClientId = 2,
                UserId = "bar",
            };
        }
    }
}
