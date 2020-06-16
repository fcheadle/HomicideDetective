using Engine.UI;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using SadConsole;

namespace Tests.UI
{
    class MenuUiTests : TestBase
    {
        MenuUi ui;
        [Test]
        public void NewMenuUiTest()
        {
            _game = new MockGame(NewUI);
            _game.RunOnce();
            Assert.IsNotNull(ui.Selector);
            Assert.IsNotNull(ui.MainOptionsConsole);
            Assert.IsNotNull(ui.HelpOptionsConsole);
            Assert.IsNotNull(ui.NewGameOptionsConsole);
            Assert.IsNotNull(ui.NewGameAdvancedOptionsConsole);
            Assert.IsNotNull(ui.SettingsOptionsConsole);
            Assert.False(ui.IsVisible);
            Assert.False(ui.IsFocused);
        }
        [Test]
        public void TitleConsoleTest()
        {
            _game = new MockGame(NewUI);
            _game.RunOnce();
        }
        [Test]
        public void MainOptionsTest()
        {
            _game = new MockGame(NewUI);
            _game.RunOnce();
            Assert.AreEqual(4, ui.MainOptionsConsole.Controls.Count);
        }
        [Test]
        public void HelpOptionsTest()
        {
            _game = new MockGame(NewUI);
            _game.RunOnce();
            Assert.AreEqual(0, ui.HelpOptionsConsole.Children.Count);//0 for now
        }
        [Test]
        public void SettingsOptionsTest()
        {
            _game = new MockGame(NewUI);
            _game.RunOnce();
            Assert.AreEqual(0, ui.SettingsOptionsConsole.Children.Count);//0 for now
        }
        [Test]
        public void HideTest()
        {
            _game = new MockGame(NewUI);
            _game.RunOnce();
            Assert.False(ui.IsVisible);
            Assert.False(ui.IsFocused);
            Assert.AreNotEqual(Global.CurrentScreen, ui);

            ui.IsVisible = true;
            ui.IsFocused = true;
            Global.CurrentScreen = ui;

            ui.Hide();
            Assert.False(ui.IsVisible);
            Assert.False(ui.IsFocused);
        }
        [Test]
        public void ShowTest()
        {
            _game = new MockGame(NewUI);
            _game.RunOnce();
            Assert.False(ui.IsVisible);
            Assert.False(ui.IsFocused);
            Assert.AreNotEqual(Global.CurrentScreen, ui);

            ui.Show();
            Assert.True(ui.IsVisible);
            Assert.True(ui.IsFocused);
            Assert.AreEqual(Global.CurrentScreen, ui);
        }

        [Test]
        public void ToggleTest()
        {
            _game = new MockGame(NewUI);
            _game.RunOnce();
            Assert.False(ui.IsVisible);
            Assert.False(ui.IsFocused);
            Assert.AreNotEqual(Global.CurrentScreen, ui);

            ui.Toggle();
            Assert.True(ui.IsVisible);
            Assert.True(ui.IsFocused);
            Assert.AreEqual(Global.CurrentScreen, ui);

            ui.Toggle();
            Assert.False(ui.IsVisible);
            Assert.False(ui.IsFocused);
            Assert.AreNotEqual(Global.CurrentScreen, ui);
        }

        private void NewUI(GameTime obj)
        {
            ui = MockGame.Menu;
        }
    }
}
