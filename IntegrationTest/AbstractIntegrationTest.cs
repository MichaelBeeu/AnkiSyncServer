using AnkiSyncServer.Models;
using IntegrationTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CollectionManager.Test
{
    public abstract class AbstractIntegrationTest<Context> where Context : DbContext
    {
        public TestContext TestContext { get; set; }
        protected static DbContextOptions<Context> DbContextOptions { get; set; }

        [TestInitialize]
        public void InitializeDbContextOptions()
        {
            DbContextOptions = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: $"{TestContext.FullyQualifiedTestClassName}.{TestContext.TestName}")
                .Options;

            // Add any fixtures needed for the test
            using (Context context = (Context)Activator.CreateInstance(typeof(Context), DbContextOptions))
            {
                // Get the attribute data
                MethodBase method = this.GetType().GetMethod(TestContext.TestName);
                WithFixture[] fixtures = (WithFixture[])Attribute.GetCustomAttributes(method, typeof(WithFixture));

                foreach (var fixtureType in fixtures)
                {
                    // Save fixture data to database
                    using (var fixture = (AbstractDataFixture)Activator.CreateInstance(fixtureType.fixture))
                    {
                        fixture.AddData(context);
                    }
                }

                context.SaveChanges();
            }
        }

        virtual public IEnumerable<object> GetFixtureData()
        {
            return new List<object>();
        }
    }
}
