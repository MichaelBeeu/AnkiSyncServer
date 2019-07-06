using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTest
{
    public abstract class AbstractDataFixture : IDisposable
    {
        virtual public void AddData(DbContext context)
        {
            context.AddRange(GetData());
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
