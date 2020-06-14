using Engine.Components.UI;
using Engine.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Components.UI
{
    class MagnifyingGlassComponentTests : TestBase
    {
        [Datapoints]
        GameActions[] queriableActions = { GameActions.LookAtEverythingInSquare, GameActions.LookAtPerson, GameActions.Talk, GameActions.GetItem };
        MagnifyingGlassComponent _component;
        const int _size = 3;

        //[Test]//todo
        public void NewMagnifyingGlassTest()
        {
            Assert.Fail();
        }

        //[Test]//todo
        public void HoveringDisplaysLookingGlass()
        {
            Assert.Fail();
        }

        //[Theory]//todo
        public void QueryTest(GameActions action)
        {
            Assert.Fail();
        }
    }
}
