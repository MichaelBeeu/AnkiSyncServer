using AnkiSyncServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.CollectionManager
{
    public class CollectionManagerFactory : ICollectionManagerFactory
    {
        private AnkiDbContext context;

        public CollectionManagerFactory(
            AnkiDbContext context
        ) {
            this.context = context;
        }

        /// <summary>
        /// Create a collection manager for the given user id.
        /// </summary>
        /// <param name="user_id">The user id to create a collection manage for.</param>
        /// <returns>Returns a  collection manager instance.</returns>
        public async Task<ICollectionManager> Create(string user_id)
        {
            Collection collection = await context.Collections
                .SingleAsync(c => c.UserId == user_id);

            return new CollectionManager(context, collection);
        }
    }
}
