using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTest
{
    [AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class WithFixture : Attribute
    {
        public System.Type fixture;

        public WithFixture(System.Type fixture)
        {
            this.fixture = fixture;
        }
    }
}
