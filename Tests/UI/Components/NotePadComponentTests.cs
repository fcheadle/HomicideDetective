using NUnit.Framework;
using SadConsole.Input;
using Engine.UI.Components;

namespace Tests.UI.Components
{
    class NotePadComponentTests : TestBase
    {
        NotePadComponent _component;
        string[] _answer;

        [SetUp]
        public void SetUp()
        {
            _component = (NotePadComponent)Game.Player.GetComponent<NotePadComponent>();
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
            Game.SwapUpdate(GetDetails);
            Game.RunOnce();
            Assert.AreEqual(1, _answer.Length);
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (NotePadComponent)Game.Player.GetComponent<NotePadComponent>();
        }

        [Test]
        public void MinimizeMaximizeTest()
        {
            _component.MouseButton_Clicked(this, new MouseEventArgs(new MouseConsoleState(Engine.Game.UiManager, new Mouse() { RightClicked = true })));
            Assert.True(_component.Window.IsVisible);
            Assert.True(_component.MaximizeButton.IsVisible);
            _component.MouseButton_Clicked(this, new MouseEventArgs(new MouseConsoleState(Engine.Game.UiManager, new Mouse() { RightClicked = true })));
            Assert.False(_component.Window.IsVisible);
            Assert.True(_component.MaximizeButton.IsVisible);
        }

        [Test]
        public void BackButtonTest()
        {
            //print a bunch of bullshit so that we're like three or four pages in
            for (int i = 0; i < 300; i++)
            {
                _component.WriteLine("Test Statement number " + i);
            }

            //get the current top line and index
            Assert.AreEqual(0, _component.PageNumber);

            //hit the back button

            //assert on the current top text and index
        }

        [Test]
        public void NextButtonTest()
        {
            //print a bunch of bullshit so that we're like three or four pages in
            for (int i = 0; i < 300; i++)
            {
                _component.WriteLine("Test Statement number " + i);
            }

            //get the current top line and index
            Assert.AreEqual(0, _component.PageNumber);

            //hit the next button

            //assert on the current top text and index
        }

        [Test]
        public void WriteTest()
        {
            _component.WriteLine("hot test action");
            Assert.True(_component.GetDetails()[0].Contains("hot test action"));
        }
    }
}
