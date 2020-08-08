using Engine.Components;
using Engine.Components.UI;
using Engine.Creatures.Components;
using NUnit.Framework;
using SadConsole.Controls;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.UI.Components
{
    class PageComponentTests : TestBase
    {
        PageComponent<ThoughtsComponent> _component;
        string[] _answer;
        [SetUp]
        public void SetUp()
        {
            _component = (PageComponent<ThoughtsComponent>)Game.Player.GetComponent<PageComponent<ThoughtsComponent>>();
        }

        [Test]
        public void NewPageComponentTests()
        {
            Assert.NotNull(_component);
            Assert.NotNull(_component.Window);
            Assert.False(_component.Window.IsVisible);
            Assert.NotNull(_component.MaximizeButton);
            Assert.True(_component.MaximizeButton.IsVisible);
        }
        [Test]
        public void GetDetailsTest()
        {
            Game.SwapUpdate(GetDetails);
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _answer = _component.GetDetails();
            ThoughtsComponent c = (ThoughtsComponent)Game.Player.GetComponent<ThoughtsComponent>();
            Assert.AreEqual(c.GetDetails().Length, _answer.Length);
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

        [Test]//I know that print works, but it resists testing, for now.
        public void ElaborateTest()
        {
            string[] bullshit =
            {
                "parmesan wedge",
                "------* ",
                "cooking oil",
                "I seen Footage",
            };
            _component.Elaborate(bullshit);
            _answer = _component.GetDetails();
            Assert.True(_answer[0].Contains(bullshit[0]));
            Assert.True(_answer[1].Contains(bullshit[1]));
            Assert.True(_answer[2].Contains(bullshit[2]));
            Assert.True(_answer[3].Contains(bullshit[3]));
        }
    }
}
