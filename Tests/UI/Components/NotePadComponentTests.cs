using Engine.Components.UI;
using NUnit.Framework;
using SadConsole.Input;

namespace Tests.UI.Components
{
    class NotePadComponentTests : TestBase
    {
        NotePadComponent _component;
        string[] _answer;

        [Test]
        public void NewNotePadComponentTests()
        {
            _game = new MockGame(NewNotePadComponent);
            _game.RunOnce();
            _game.Stop();
        }
        private void NewNotePadComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (NotePadComponent)_game.Player.GetComponent<NotePadComponent>();
            Assert.NotNull(_component);
            Assert.NotNull(_component.Window);
            Assert.False(_component.Window.IsVisible);
            Assert.NotNull(_component.MaximizeButton);
            Assert.True(_component.MaximizeButton.IsVisible);
            _component.ProcessTimeUnit();
        }
        [Test]
        public void GetDetailsTest()
        {
            _game = new MockGame(NewNotePadComponent);
            _game.RunOnce();

            _game.SwapUpdate(GetDetails);
            _game.RunOnce();
            _game.Stop();
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (NotePadComponent)_game.Player.GetComponent<NotePadComponent>();
            _answer = _component.GetDetails();
        }

        [Test]
        public void MinimizeMaximizeTest()
        {
            _game = new MockGame(NewNotePadComponent);
            _game.RunOnce();
            _game.SwapUpdate(GetDetails);
            _component.MinimizeMaximize(this, new MouseEventArgs(new MouseConsoleState(Engine.Game.UIManager, new Mouse() { RightClicked = true })));
            Assert.True(_component.Window.IsVisible);
            Assert.True(_component.MaximizeButton.IsVisible);
            _component.MinimizeMaximize(this, new MouseEventArgs(new MouseConsoleState(Engine.Game.UIManager, new Mouse() { RightClicked = true })));
            Assert.False(_component.Window.IsVisible);
            Assert.True(_component.MaximizeButton.IsVisible);
            _game.Stop();
        }

        //[Test]
        public void BackButtonTest()
        {
            //print a bunch of bullshit so that we're like three or four pages in

            //get the current top line and index

            //hit the back button

            //assert on the current top text and index
            Assert.Fail();
        }

        //[Test]
        public void NextButtonTest()
        {
            //print a bunch of bullshit so that we're like three or four pages in

            //get the current top line and index

            //hit the back button

            //assert on the current top text and index
            Assert.Fail();
        }
    }
}
