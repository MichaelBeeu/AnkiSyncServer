using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTest
{
    public abstract class AbstractDataFixture : IDisposable
    {
        virtual public void SaveData(DbContext context)
        {
            context.AddRange(GetData());
            context.SaveChanges();
        }

        virtual public IEnumerable<object> GetData()
        {
            return new List<object>();
        }

        public void Dispose()
        {

        }
    }
}
