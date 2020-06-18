﻿using Engine.Components.UI;
using Engine.UI.Components;
using Engine.Utilities;
using GoRogue;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using SadConsole;
using SadConsole.Input;
using Tests.Mocks;

namespace Tests.UI.Components
{
    class MenuKeyboardControlTests : TestBase
    {
        CSIKeyboardComponent _component;
        MagnifyingGlassComponent _lookingGlass;

        public MenuKeyboardControlTests()
        {
        }

        [Datapoints]
        GameAction[] allActions =
        {
            GameAction.LookAtEverythingInSquare, //test that cursor opens
            GameAction.LookAtPerson, //test that cursor opens
            GameAction.Talk, //test that cursor opens
            GameAction.TakePhotograph, //test that a photograph window opens
            GameAction.GetItem, //test that a cursor opens ???
            GameAction.RemoveItemFromInventory, //should probably not be a game action, and should be an inventory action?
            GameAction.TogglePause, //test that FOV is reduced and that we stop listening to keyboard interaction
            GameAction.DustItemForPrints, //prints and tracks really just work the same way anyways
            GameAction.ToggleNotes, //test that the notepad opens and becomes focused
            GameAction.ToggleInventory, //test that the window opens and becomes focused
            GameAction.ToggleMenu, //test that the menu opens, becomes focused, and pauses the game
            GameAction.RefocusOnPlayer, //switch focus elsewhere, then assert that player has focus again.
        };

        [DatapointSource]
        (GameAction, GameAction)[] newCursorActions =
        {
            (GameAction.LookAtEverythingInSquare, GameAction.LookAtEverythingInSquare),
            (GameAction.LookAtPerson, GameAction.LookAtPerson),
            (GameAction.Talk, GameAction.Talk),
            (GameAction.GetItem, GameAction.GetItem),
        };
        [DatapointSource]
        (GameAction, string)[] buttonsAndWindowsToggled =
        {
            (GameAction.TakePhotograph, "Photograph of "),
            (GameAction.ToggleInventory, "Evidence"),
            (GameAction.DustItemForPrints, "Fingerprints on "),
            (GameAction.ToggleNotes, "Notepad"),
            (GameAction.ToggleMenu, "Paused"), //window toggled
        };

        [SetUp]
        public void SetUp()
        {
            _component = (CSIKeyboardComponent)_game.Player.GetComponent<CSIKeyboardComponent>();
        }

        [Test]
        public void NewMenuKeyBoardComponentTests()
        {
            Assert.NotNull(_component);
        }

        [Test]//skip for now
        public void MovesTest()
        {
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
        }

        [Test]
        public void ListensForKeyBindingsOnPauseOnlyTest()
        {
            _game.SwapUpdate(TogglePause);
            _game.RunOnce();
            Coord position = _game.Player.Position;
            var keyboard = new MockKeyboard();
            keyboard.AddKeyDown(new AsciiKey() { Key = Keys.Right }, Keys.Right);

            _component.ProcessKeyboard(_game.Player, keyboard, out bool _);
            Assert.AreEqual(position, _game.Player.Position);
            //assert that nothing changed?
            Assert.Fail("Test is not sufficient to be reliable");
        }

        private void TogglePause(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (CSIKeyboardComponent)_game.Player.GetComponent<CSIKeyboardComponent>();
            _component.TogglePause();
        }

        [Test] //todo
        public void ToggleMenuTest()
        {
            _component.ToggleMenu();

            Assert.AreEqual(MockGame.Menu, Global.CurrentScreen);
        }

        [Theory] //todo - just takes too long for now
        public void QueriableActionOpensACursorTest((GameAction actionkey, GameAction purpose) dataset)
        {
            _lookingGlass = (MagnifyingGlassComponent)_game.Player.GetComponent<MagnifyingGlassComponent>();
            Assert.Null(_lookingGlass);

            _component.TakeAction(dataset.actionkey);
            if (dataset.actionkey == dataset.purpose)
                Assert.AreEqual(dataset.purpose, _lookingGlass.Purpose);
            else
                Assert.AreNotEqual(dataset.purpose, _lookingGlass.Purpose);
        }

        [Theory]//todo -- just takes too long for now
        public void TogglesWindowsTest((GameAction action, string windowTitle) dataset)
        {
            Assert.Fail();
        }

        [Theory]//todo - just takes too long for now
        public void TakeActionsTest(GameAction action)
        {
            Assert.DoesNotThrow(() => _component.TakeAction(action));
        }
    }
}
