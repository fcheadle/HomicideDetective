using Engine.Components;
using Engine.Components.UI;
using Engine.Creatures.Components;
using NUnit.Framework;
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
            _component = (PageComponent<ThoughtsComponent>)_game.Player.GetComponent<PageComponent<ThoughtsComponent>>();
        }

        [Test]
        public void NewPageComponentTests()
        {
            Assert.NotNull(_component);
            Assert.NotNull(_component.Window);
            Assert.False(_component.Window.IsVisible);
            Assert.NotNull(_component.MaximizeButton);
            Assert.True(_component.MaximizeButton.IsVisible);
            Assert.DoesNotThrow(() => _game.RunOnce());
        }
        [Test]
        public void GetDetailsTest()
        {
            _game.SwapUpdate(GetDetails);
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _answer = _component.GetDetails();
            ThoughtsComponent c = (ThoughtsComponent)_game.Player.GetComponent<ThoughtsComponent>();
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
        public void PrintTest()
        {
            string[] bullshit =
            {
                "parmesan wedge",
                "------* ",
                "cooking oil",
                "I seen Footage",
            };
            _component.Print(bullshit);
            _answer = _component.GetDetails();
            Assert.True(_answer[0].Contains(bullshit[0]));
            Assert.True(_answer[0].Contains(bullshit[1]));
            Assert.True(_answer[0].Contains(bullshit[2]));
            Assert.True(_answer[0].Contains(bullshit[3]));
            
            //SadConsole.CellSurface surface = _component.Window.GetSubSurface(new GoRogue.Rectangle(0, 0, _component.Window.Width, _component.Window.Height));

            //surface to string?
            //string answer = "";
            //for (int i = 0; i < _component.Window.Width; i++)
            //{
            //    for (int j = 0; j < _component.Window.Height; j++)
            //    {
            //        answer += _component.Window.GetGlyph(i, j);
            //    }
            //}
            //foreach (string shit in bullshit)
            //    Assert.True(answer.Contains(shit));
        }
    }
}
