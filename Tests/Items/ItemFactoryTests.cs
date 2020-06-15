using Engine.Entities.Items;
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
        MockItemFactory factory = new MockItemFactory();
        [Test]
        public void NewGenericItemTest()
        {
            _game = new MockGame(NewGenericItem);
            _game.RunOnce();
            _game.Stop();
        }
        public void NewGenericItem(GameTime time)
        {
            var item = factory.Generic(new Coord(0, 0), "hotdog");
            Assert.NotNull(item);
            Assert.AreEqual(new Coord(0, 0), item.Position);
            Assert.AreEqual("hotdog", item.Name);
        }
    }
}
