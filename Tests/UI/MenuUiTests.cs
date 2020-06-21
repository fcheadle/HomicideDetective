using Engine.UI;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using SadConsole;

namespace Tests.UI
{
    class MenuUiTests : TestBase
    {
        MenuUi _ui;
        [SetUp]
        public void Setup()
        {
            _ui = MockGame.Menu;
        }
        [Test]
        public void NewMenuUiTest()
        {
            Assert.NotNull(_ui);
            Assert.NotNull(_ui.ControlledGameObject);
            Assert.False(_ui.IsVisible);
            Assert.False(_ui.IsFocused);
        }
        [Test]
        public void TitleConsoleTest()
        {
            Assert.NotNull(_ui);
        }
        [Test]
        public void MainOptionsTest()
        {
            Assert.IsNotNull(_ui.MainOptions);
            Assert.AreEqual(4, _ui.MainOptions.Count);
        }
        [Test]
        public void HelpOptionsTest()
        {
            Assert.IsNotNull(_ui.HelpOptions);
            Assert.AreEqual(2, _ui.HelpOptions.Controls.Count);//0 for now
        }
        [Test]
        public void SettingsOptionsTest()
        {
            Assert.IsNotNull(_ui.SettingsOptions);
            Assert.AreEqual(13, _ui.SettingsOptions.Count);
        }
        [Test]
        public void NewGameOptionsTest()
        {
            Assert.IsNotNull(_ui.NewGameOptions);
            Assert.AreEqual(2, _ui.NewGameOptions.Count);//quickstart / advanced
        }
        [Test]
        public void NewGameAdvancedOptionsTest()
        {
            Assert.IsNotNull(_ui.NewGameAdvancedOptions);
            Assert.AreEqual(0, _ui.NewGameOptions.Count);//0 for now
        }
    }
}
