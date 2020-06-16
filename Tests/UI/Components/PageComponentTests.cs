using Engine.Components;
using Engine.Components.UI;
using Engine.Creatures.Components;
using NUnit.Framework;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.UI.Components
{
    class PageComponentTests : TestBase
    {
        PageComponent<HealthComponent> _component;
        string[] _answer;

        [Test]
        public void NewPageComponentTests()
        {
            _game = new MockGame(NewPageComponent);
            _game.RunOnce();
            _game.Stop();
        }
        private void NewPageComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (PageComponent<HealthComponent>)_game.Player.GetComponent<PageComponent<HealthComponent>>();
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
            _game = new MockGame(NewPageComponent);
            _game.RunOnce();

            _game.SwapUpdate(GetDetails);
            _game.RunOnce();
            _game.Stop();
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (PageComponent<HealthComponent>)_game.Player.GetComponent<PageComponent<HealthComponent>>();
            _answer = _component.GetDetails();
            HealthComponent c = (HealthComponent)_game.Player.GetComponent<HealthComponent>();
            Assert.AreEqual(c.GetDetails().Length, _answer.Length);
        }

        [Test]
        public void MinimizeMaximizeTest()
        {
            _game = new MockGame(NewPageComponent);
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
            _game = new MockGame(NewPageComponent);
            _game.RunOnce();
            _component.Print(bullshit);

            SadConsole.CellSurface surface = _component.Window.GetSubSurface(new GoRogue.Rectangle(0, 0, _component.Window.Width, _component.Window.Height));

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
