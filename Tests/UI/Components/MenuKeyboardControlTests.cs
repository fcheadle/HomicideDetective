using Engine.Components.UI;
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
        MenuKeyboardComponent _component;
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
            _component = (MenuKeyboardComponent)MockGame.Menu.Player.GetComponent<MenuKeyboardComponent>();
        }

        [Test]
        public void NewMenuKeyBoardComponentTests()
        {
            Assert.NotNull(_component);
        }

        [Test]
        public void MovesTest()
        {
            var cursor = MockGame.Menu.Player;
            Coord startingPosition = cursor.Position;
            _component.MoveDown();
            Assert.AreNotEqual(startingPosition, MockGame.Menu.Player.Position);
        }
    }
}
