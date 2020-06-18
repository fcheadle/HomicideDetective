using Engine.UI;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using SadConsole;

namespace Tests.UI
{
    class MenuUiTests : TestBase
    {
        MenuUi ui;
        [SetUp]
        public void Setup()
        {
            ui = MockGame.Menu;
        }
        [Test]
        //[Parallelizable]
        public void NewMenuUiTest()
        {
            ui = MockGame.Menu;
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
        //[Parallelizable]
        public void TitleConsoleTest()
        {
            Assert.Fail();
        }
        [Test]
        //[Parallelizable]
        public void MainOptionsTest()
        {
            Assert.AreEqual(4, ui.MainOptionsConsole.Controls.Count);
        }
        [Test]
        //[Parallelizable]
        public void HelpOptionsTest()
        {
            Assert.AreEqual(0, ui.HelpOptionsConsole.Children.Count);//0 for now
        }
        [Test]
        //[Parallelizable]
        public void SettingsOptionsTest()
        {
            Assert.AreEqual(0, ui.SettingsOptionsConsole.Children.Count);//0 for now
        }
        [Test]
        //[Parallelizable]
        public void HideTest()
        {
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
        //[Parallelizable]
        public void ShowTest()
        {
            Assert.False(ui.IsVisible);
            Assert.False(ui.IsFocused);
            Assert.AreNotEqual(Global.CurrentScreen, ui);

            ui.Show();
            Assert.True(ui.IsVisible);
            Assert.True(ui.IsFocused);
            Assert.AreEqual(Global.CurrentScreen, ui);
        }

        [Test]
        //[Parallelizable]
        public void ToggleTest()
        {
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
    }
}
