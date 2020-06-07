using Engine.Components;
using Engine.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Components
{
    class KeyBoardComponentTests : TestBase
    {
        CSIKeyboardComponent _base;
        string[] _answer;
        [Datapoints]
        GameActions[] actions =
        {
            GameActions.LookAtEverythingInSquare,
            GameActions.LookAtPerson,
            GameActions.Talk,
            GameActions.TakePhotograph,
            GameActions.GetItem,
            GameActions.RemoveItemFromInventory,
            GameActions.DustItemForPrints, //prints and tracks really just work the same way anyways
            GameActions.ToggleNotes,
            GameActions.ToggleInventory,
        };

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void NewKeyBoardComponentTests()
        {
            _game = new MockGame(NewKeyboardComponent);
            _game.RunOnce();
            _game.Stop();
        }
        private void NewKeyboardComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _base = (CSIKeyboardComponent)MockGame.Player.GetComponent<CSIKeyboardComponent>();
            Assert.NotNull(_base);
            //_base.ProcessTimeUnit();
        }
        [Test]
        public void ListensForKeyBindingsOnPauseOnly()
        {
            Assert.Fail();
            _game = new MockGame(TogglePause);
            _game.RunOnce();
            _game.Stop();
        }

        private void TogglePause(Microsoft.Xna.Framework.GameTime time)
        {
            _base = (CSIKeyboardComponent)MockGame.Player.GetComponent<CSIKeyboardComponent>();
            //???
            //Assert.AreEqual(2, _answer.Length);
        }

        [Test]
        public void ToggleMenuTest()
        {
            Assert.Fail();
        }

        [Theory]
        public void TakeActionsTest(GameActions action)
        {
            Assert.Fail();
        }
    }
}
