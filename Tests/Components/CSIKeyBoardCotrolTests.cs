using Engine;
using Engine.Components;
using Engine.Components.UI;
using Engine.Utilities;
using GoRogue;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tests.Mocks;

namespace Tests.Components
{
    class CSIKeyBoardCotrolTests : TestBase
    {
        CSIKeyboardComponent _component;
        CellSurface cursor = new CellSurface(3, 3);
        
        public CSIKeyBoardCotrolTests()
        {
        }

        [Datapoints]
        GameActions[] newCursorActions =
        {
            GameActions.LookAtEverythingInSquare,
            GameActions.LookAtPerson,
            GameActions.Talk,
            GameActions.GetItem,
        };
        
        (GameActions, BasicEntity)[] actionsAndExpectedConditions =
        {
            (GameActions.TakePhotograph, MockGame.Player),
            //(GameActions.RemoveItemFromInventory, 
            //GameActions.DustItemForPrints, //prints and tracks really just work the same way anyways
            //GameActions.ToggleNotes,
            //GameActions.ToggleInventory,

        };

        [SetUp]
        public void SetUp()
        {
            _game = new MockGame(NewKeyboardComponent);
            _game.RunOnce();
        }

        [Test]
        public void NewKeyBoardComponentTests()
        {
            _game.Stop();
        }
        private void NewKeyboardComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (CSIKeyboardComponent)MockGame.Player.GetComponent<CSIKeyboardComponent>();
            Assert.NotNull(_component);
        }
        //[Test]//todo: figure out how to send fake keystrokes
        public void MovesTest()
        {
            _game = new MockGame(NewKeyboardComponent);
            _game.RunOnce();
            Coord startingPosition = MockGame.Player.Position;
            Coord position = MockGame.Player.Position;
            position += new Coord(1, 0);
            MockKeyboard keyboard = new MockKeyboard();
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Right }, Keys.Right);
            _component.ProcessKeyboard(MockGame.Player, keyboard, out bool _);
            Assert.AreEqual(position, MockGame.Player.Position);
            keyboard.Clear();
            position += new Coord(0, 1);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Down }, Keys.Down);
            _component.ProcessKeyboard(MockGame.Player, keyboard, out bool _);
            Assert.AreEqual(position, MockGame.Player.Position);
            keyboard.Clear();
            position += new Coord(-1, 0);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Left }, Keys.Left);
            _component.ProcessKeyboard(MockGame.Player, keyboard, out bool _);
            Assert.AreEqual(position, MockGame.Player.Position);
            keyboard.Clear();
            position += new Coord(0, -1);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Up }, Keys.Up);
            _component.ProcessKeyboard(MockGame.Player, keyboard, out bool _);
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

            _component.ProcessKeyboard(MockGame.Player, keyboard, out bool _);
            Assert.AreEqual(position, MockGame.Player.Position);
            //assert that nothing changed?

            _game.Stop();
        }

        private void TogglePause(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (CSIKeyboardComponent)MockGame.Player.GetComponent<CSIKeyboardComponent>();
            _component.TogglePause();
        }

        //[Test] //todo
        public void ToggleMenuTest()
        {
            Assert.Fail();
        }

        [Theory] //todo
        public void OpensACursorTest(GameActions actionThatOpensACursor)
        {
            _game = new MockGame(NewKeyboardComponent);
            _game.RunOnce();
            MagnifyingGlassComponent glass = (MagnifyingGlassComponent)MockGame.Player.GetComponent<MagnifyingGlassComponent>();
            Assert.Null(glass);

            _component.TakeAction(actionThatOpensACursor);

            //assert that our currently focused object is the cursor.
            DrawingSurface cursor = glass.Surface;

            //Assert.True(cursor.IsVisible);
            //Assert.True(cursor.IsEnabled);
            //Assert.AreEqual(cursor.Position, MockGame.Player.Position);
            //Assert.AreEqual()
        }
    }
}
