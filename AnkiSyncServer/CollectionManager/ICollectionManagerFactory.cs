using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnkiSyncServer.CollectionManager
{
    public interface ICollectionManagerFactory
    {
        Task<ICollectionManager> Create(string user_id);
    }
}
