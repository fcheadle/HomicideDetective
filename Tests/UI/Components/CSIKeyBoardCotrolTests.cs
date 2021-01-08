using HomicideDetective.Old.UI.Components;
using HomicideDetective.Old.Utilities;
using NUnit.Framework;
using SadConsole;

namespace Tests.UI.Components
{
    class CsiKeyboardControlTests : TestBase
    {
        CsiKeyboardComponent _component;
        //MagnifyingGlassComponent _lookingGlass;

        // ReSharper disable once UnusedMember.Local
        [Datapoints] private GameAction[] _allActions =
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

        // ReSharper disable once UnusedMember.Local
        [DatapointSource] private (GameAction, GameAction)[] _newCursorActions =
        {
            (GameAction.LookAtEverythingInSquare, GameAction.LookAtEverythingInSquare),
            (GameAction.LookAtPerson, GameAction.LookAtPerson),
            (GameAction.Talk, GameAction.Talk),
            (GameAction.GetItem, GameAction.GetItem),
        };
        [DatapointSource]
        // ReSharper disable once UnusedMember.Local
        (GameAction, string)[] _buttonsAndWindowsToggled =
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
            _component = (CsiKeyboardComponent)Game.Player.GetComponent<CsiKeyboardComponent>();
        }

        [Test]
        public void NewKeyBoardComponentTests()
        {
            Assert.NotNull(_component);
        }
        //[Test]//skip for now
        /*public void MovesTest()
        {
            Coord startingPosition = Game.Player.Position;
            Coord position = Game.Player.Position;
            position += new Coord(1, 0);
            MockKeyboard keyboard = new MockKeyboard();
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Right }, Keys.Right);
            _component.ProcessKeyboard(Game.Player, keyboard, out bool _);
            Assert.AreEqual(position, Game.Player.Position);
            keyboard.Clear();
            position += new Coord(0, 1);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Down }, Keys.Down);
            _component.ProcessKeyboard(Game.Player, keyboard, out bool _);
            Assert.AreEqual(position, Game.Player.Position);
            keyboard.Clear();
            position += new Coord(-1, 0);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Left }, Keys.Left);
            _component.ProcessKeyboard(Game.Player, keyboard, out bool _);
            Assert.AreEqual(position, Game.Player.Position);
            keyboard.Clear();
            position += new Coord(0, -1);
            keyboard.AddKeyPressed(new AsciiKey() { Key = Keys.Up }, Keys.Up);
            _component.ProcessKeyboard(Game.Player, keyboard, out bool _);
            Assert.AreEqual(position, Game.Player.Position);
            Assert.AreEqual(startingPosition, Game.Player.Position);
        }*/

        //[Test] //todo
        /*public void ListensForKeyBindingsOnPauseOnlyTest()
        {
            Game.SwapUpdate(TogglePause);
            Game.RunOnce();
            Coord position = Game.Player.Position;
            var keyboard = new MockKeyboard();
            keyboard.AddKeyDown(new AsciiKey() { Key = Keys.Right }, Keys.Right);

            _component.ProcessKeyboard(Game.Player, keyboard, out bool _);
            Assert.AreEqual(position, Game.Player.Position);
            //assert that nothing changed?
            Assert.Fail("Test is not sufficient to be reliable");
        }

        private void TogglePause(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (CsiKeyboardComponent)Game.Player.GetComponent<CsiKeyboardComponent>();
            _component.TogglePause();
        }*/

        [Test]
        public void ToggleMenuTest()
        {
            var previousScreen = Global.CurrentScreen;
            _component.ToggleMenu();
            Game.RunOnce();
            Assert.AreNotEqual(previousScreen, Global.CurrentScreen);
        }

        //[Theory] //todo - just takes too long for now
        // public void QueriableActionOpensACursorTest((GameAction actionkey, GameAction purpose) dataset)
        // {
        //     _lookingGlass = (MagnifyingGlassComponent)Game.Player.GetComponent<MagnifyingGlassComponent>();
        //     Assert.Null(_lookingGlass);
        //
        //     _component.TakeAction(dataset.actionkey);
        //     if (dataset.actionkey == dataset.purpose)
        //         Assert.AreEqual(dataset.purpose, _lookingGlass.Purpose);
        //     else
        //         Assert.AreNotEqual(dataset.purpose, _lookingGlass.Purpose);
        // }

        //[Theory]//todo -- just takes too long for now
        public void TogglesWindowsTest((GameAction action, string windowTitle) dataset)
        {
            Assert.Fail();
        }

        //[Theory]//todo - just takes too long for now
        public void TakeActionsTest(GameAction action)
        {
            Assert.DoesNotThrow(() => _component.TakeAction(action));
        }
    }
}
