using Engine.Items;
using GoRogue;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Items
{
    class ItemFactoryTests : TestBase
    {
        DefaultItemFactory factory = new DefaultItemFactory();
        [Test]
        public void NewGenericItemTest()
        {
            var item = factory.Generic(new Coord(0, 0), "hotdog");
            Assert.NotNull(item);
            Assert.AreEqual(new Coord(0, 0), item.Position);
            Assert.AreEqual("hotdog", item.Name);
        }
    }
}
