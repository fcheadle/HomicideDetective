using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    class MockState
    {
        [Test]
        public void Fail()
        {
            Assert.Fail();
        }
    }
}
