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
        MagnifyingGlassComponent _lookingGlass;
        
        public CSIKeyBoardCotrolTests()
        {
        }

        [Datapoints]
        GameActions[] allActions =
        {
            GameActions.LookAtEverythingInSquare, //test that cursor opens
            GameActions.LookAtPerson, //test that cursor opens
            GameActions.Talk, //test that cursor opens
            GameActions.TakePhotograph, //test that a photograph window opens
            GameActions.GetItem, //test that a cursor opens ???
            GameActions.RemoveItemFromInventory, //should probably not be a game action, and should be an inventory action?
            GameActions.TogglePause, //test that FOV is reduced and that we stop listening to keyboard interaction
            GameActions.DustItemForPrints, //prints and tracks really just work the same way anyways
            GameActions.ToggleNotes, //test that the notepad opens and becomes focused
            GameActions.ToggleInventory, //test that the window opens and becomes focused
            GameActions.ToggleMenu, //test that the menu opens, becomes focused, and pauses the game
            GameActions.RefocusOnPlayer, //switch focus elsewhere, then assert that player has focus again.
        };

        [DatapointSource]
        (GameActions, GameActions)[] newCursorActions =
        {
            (GameActions.LookAtEverythingInSquare, GameActions.LookAtEverythingInSquare),
            (GameActions.LookAtPerson, GameActions.LookAtPerson),
            (GameActions.Talk, GameActions.Talk),
            (GameActions.GetItem, GameActions.GetItem),
        };
        [DatapointSource]
        (GameActions, string)[] buttonsAndWindowsToggled =
        {
            (GameActions.TakePhotograph, "Photograph of "),
            (GameActions.ToggleInventory, "Evidence"),
            (GameActions.DustItemForPrints, "Fingerprints on "),
            (GameActions.ToggleNotes, "Notepad"),
            (GameActions.ToggleMenu, "Paused"), //window toggled
        };

        [SetUp]
        public void SetUp()
        {
            _game = new MockGame(NewKeyboardComponent);
            _game.RunOnce();
        }
        [TearDown]
        public void TearDown()
        {
            _game.Stop();
        }
        [Test]
        public void NewKeyBoardComponentTests()
        {
            _game.Stop();
        }
        private void NewKeyboardComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (CSIKeyboardComponent)_game.Player.GetComponent<CSIKeyboardComponent>();
            _lookingGlass = (MagnifyingGlassComponent)_game.Player.GetComponent<MagnifyingGlassComponent>();
            Assert.NotNull(_component);
        }
        //[Test]//skip for now
        public void MovesTest()
        {
            _game = new MockGame(NewKeyboardComponent);
            _game.RunOnce();
            Coord startingPosition = _game.Player.Position;
            Coord position = _game.Player.Position;
            position += new Coord(1, 0);
            MockKeyboard keyboard = new MockKeyboard();
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Right }, Keys.Right);
            _component.ProcessKeyboard(_game.Player, keyboard, out bool _);
            Assert.AreEqual(position, _game.Player.Position);
            keyboard.Clear();
            position += new Coord(0, 1);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Down }, Keys.Down);
            _component.ProcessKeyboard(_game.Player, keyboard, out bool _);
            Assert.AreEqual(position, _game.Player.Position);
            keyboard.Clear();
            position += new Coord(-1, 0);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Left }, Keys.Left);
            _component.ProcessKeyboard(_game.Player, keyboard, out bool _);
            Assert.AreEqual(position, _game.Player.Position);
            keyboard.Clear();
            position += new Coord(0, -1);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Up }, Keys.Up);
            _component.ProcessKeyboard(_game.Player, keyboard, out bool _);
            Assert.AreEqual(position, _game.Player.Position);
            Assert.AreEqual(startingPosition, _game.Player.Position);
            _game.Stop();
        }

        [Test]
        public void ListensForKeyBindingsOnPauseOnlyTest()
        {
            _game = new MockGame(NewKeyboardComponent);
            _game.RunOnce();
            _game.SwapUpdate(TogglePause);
            _game.RunOnce();
            Coord position = _game.Player.Position;
            var keyboard = new MockKeyboard();
            keyboard.AddKeyDown(new AsciiKey() { Key = Keys.Right }, Keys.Right);

            _component.ProcessKeyboard(_game.Player, keyboard, out bool _);
            Assert.AreEqual(position, _game.Player.Position);
            //assert that nothing changed?

            _game.Stop();
        }

        private void TogglePause(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (CSIKeyboardComponent)_game.Player.GetComponent<CSIKeyboardComponent>();
            _component.TogglePause();
        }

        //[Test] //todo
        public void ToggleMenuTest()
        {
            Assert.Fail();
        }

        //[Theory] //todo
        public void QueriableActionOpensACursorTest((GameActions actionkey, GameActions purpose) dataset)
        {
            _game = new MockGame(NewKeyboardComponent);
            _game.RunOnce();
            _lookingGlass = (MagnifyingGlassComponent)_game.Player.GetComponent<MagnifyingGlassComponent>();
            Assert.Null(_lookingGlass);

            _component.TakeAction(dataset.actionkey);
            if(dataset.actionkey == dataset.purpose)
                Assert.AreEqual(dataset.purpose, _lookingGlass.Purpose);
            else
                Assert.AreNotEqual(dataset.purpose, _lookingGlass.Purpose);
        }

        //[Theory]//todo
        public void TogglesWindowsTest((GameActions action, string windowTitle) dataset)
        {
            Assert.Fail();
        }

        [Theory]
        public void TakeActionsTest(GameActions action)
        {
            Assert.DoesNotThrow(() => _component.TakeAction(action));
        }
    }
}
