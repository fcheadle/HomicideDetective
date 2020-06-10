using Engine;
using Engine.Components;
using Engine.Utilities;
using GoRogue;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tests.Mocks;

namespace Tests.Components
{
    class CSIKeyBoardCotrolTests : TestBase
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
        //[Test]//todo: figure out how to send fake keystrokes
        public void ListensForKeyTest()
        {
            _game = new MockGame(NewKeyboardComponent);
            _game.RunOnce();
            Coord startingPosition = MockGame.Player.Position;
            Coord position = MockGame.Player.Position;
            position += new Coord(1, 0);
            var keyboard = new MockKeyboard();
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Right }, Keys.Right);
            _base.ProcessKeyboard(MockGame.Player, keyboard, out bool _);
            Assert.AreEqual(position, MockGame.Player.Position);
            keyboard.Clear();
            position += new Coord(0, 1);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Down }, Keys.Down);
            _base.ProcessKeyboard(MockGame.Player, keyboard, out bool _);
            Assert.AreEqual(position, MockGame.Player.Position);
            keyboard.Clear();
            position += new Coord(-1, 0);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Left }, Keys.Left);
            _base.ProcessKeyboard(MockGame.Player, keyboard, out bool _);
            Assert.AreEqual(position, MockGame.Player.Position);
            keyboard.Clear();
            position += new Coord(0, -1);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Up }, Keys.Up);
            _base.ProcessKeyboard(MockGame.Player, keyboard, out bool _);
            Assert.AreEqual(position, MockGame.Player.Position);
            Assert.AreEqual(startingPosition, MockGame.Player.Position);
            _game.Stop();
        }
        [Test]
        public void ListensForKeyBindingsOnPauseOnlyTest()
        {
            _game = new MockGame(NewKeyboardComponent);
            _game.RunOnce();
            _game.SwapUpdate(TogglePause);
            _game.RunOnce();
            Coord position = MockGame.Player.Position;
            var keyboard = new MockKeyboard();
            keyboard.AddKeyDown(new AsciiKey() { Key = Keys.Right }, Keys.Right);

            _base.ProcessKeyboard(MockGame.Player, keyboard, out bool _);
            Assert.AreEqual(position, MockGame.Player.Position);
            //assert that nothing changed?

            _game.Stop();
        }

        private void TogglePause(Microsoft.Xna.Framework.GameTime time)
        {
            _base = (CSIKeyboardComponent)MockGame.Player.GetComponent<CSIKeyboardComponent>();
            _base.TogglePause();
        }

        //[Test] //todo
        public void ToggleMenuTest()
        {
            Assert.Fail();
        }

        //[Theory] //todo
        public void TakeActionsTest(GameActions action)
        {
            Assert.Fail();
        }
    }
}
