using Engine.Components.UI;
using NUnit.Framework;
using SadConsole.Input;

namespace Tests.UI.Components
{
    class NotePadComponentTests : TestBase
    {
        NotePadComponent _component;
        string[] _answer;

        [SetUp]
        public void SetUp()
        {
            _component = (NotePadComponent)_game.Player.GetComponent<NotePadComponent>();
        }

        [Test]
        public void NewNotePadComponentTests()
        {
            Assert.NotNull(_component);
            Assert.NotNull(_component.Window);
            Assert.False(_component.Window.IsVisible);
            Assert.NotNull(_component.MaximizeButton);
            Assert.True(_component.MaximizeButton.IsVisible);
            Assert.DoesNotThrow(() => _component.ProcessTimeUnit());
        }
        [Test]
        public void GetDetailsTest()
        {
            _answer = _component.GetDetails();
            _game.SwapUpdate(GetDetails);
            _game.RunOnce();
            Assert.AreEqual(1, _answer.Length); //height is 32 lines per page, times 100 pages
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (NotePadComponent)_game.Player.GetComponent<NotePadComponent>();
        }

        [Test]
        public void MinimizeMaximizeTest()
        {
            _component.MinimizeMaximize(this, new MouseEventArgs(new MouseConsoleState(Engine.Game.UIManager, new Mouse() { RightClicked = true })));
            Assert.True(_component.Window.IsVisible);
            Assert.True(_component.MaximizeButton.IsVisible);
            _component.MinimizeMaximize(this, new MouseEventArgs(new MouseConsoleState(Engine.Game.UIManager, new Mouse() { RightClicked = true })));
            Assert.False(_component.Window.IsVisible);
            Assert.True(_component.MaximizeButton.IsVisible);
        }

        [Test]
        public void BackButtonTest()
        {
            //print a bunch of bullshit so that we're like three or four pages in

            //get the current top line and index

            //hit the back button

            //assert on the current top text and index
            Assert.Fail();
        }

        [Test]
        public void NextButtonTest()
        {
            //print a bunch of bullshit so that we're like three or four pages in

            //get the current top line and index

            //hit the back button

            //assert on the current top text and index
            Assert.Fail();
        }

        [Test]
        public void WriteTest()
        {
            //write some text
            Assert.Fail();
        }
    }
}
