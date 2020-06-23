using Engine.UI;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using SadConsole;
using System.Collections;
using System.Collections.Generic;

namespace Tests.UI
{
    class MenuUiTests : TestBase
    {
        MenuUi ui;
        [Datapoint] MenuPanel main;
        [Datapoint] HelpPanel help;
        [Datapoint] MenuPanel settings;
        [Datapoint] MenuPanel newGame;
        [Datapoint] MenuPanel newGameAdvanced;
        Stack<MenuPanel> activePanels;
        BasicEntity selector;

        

        [SetUp]
        public void Setup()
        {
            ui = MockGame.Menu;
            main = ui.MainOptions;
            help = ui.HelpOptions;
            settings = ui.SettingsOptions;
            activePanels = ui.ActivePanels;
            selector = ui.ControlledGameObject;
            newGame = ui.NewGameOptions;
            newGameAdvanced = ui.NewGameAdvancedOptions;
        }
        [Test]
        public void NewMenuUiTest()
        {
            Assert.NotNull(ui);
            Assert.NotNull(selector);
            Assert.NotNull(main);
            Assert.NotNull(help);
            Assert.NotNull(settings);
            Assert.NotNull(activePanels);
            Assert.NotNull(newGame);
            Assert.NotNull(newGameAdvanced);
            Assert.False(ui.IsVisible);
            Assert.False(ui.IsFocused);
        }
        [Test]
        public void TitleConsoleTest()
        {
            Assert.NotNull(ui.TitleConsole);
        }
        [Test]
        public void MainOptionsTest()
        {
            Assert.IsNotNull(ui.MainOptions);
            Assert.AreEqual(4, ui.MainOptions.Controls.Count);
        }
        [Test]
        public void HelpOptionsTest()
        {
            Assert.IsNotNull(ui.HelpOptions);
            Assert.AreEqual(2, ui.HelpOptions.Controls.Count);//0 for now
        }
        [Test]
        public void SettingsOptionsTest()
        {
            Assert.IsNotNull(ui.SettingsOptions);
            Assert.AreEqual(13, ui.SettingsOptions.Controls.Count);
        }
        [Test]
        public void NewGameOptionsTest()
        {
            Assert.IsNotNull(ui.NewGameOptions);
            Assert.AreEqual(2, ui.NewGameOptions.Controls.Count);//quickstart / advanced
        }
        [Test]
        public void NewGameAdvancedOptionsTest()
        {
            Assert.IsNotNull(ui.NewGameAdvancedOptions);
            Assert.AreEqual(1, ui.NewGameAdvancedOptions.Controls.Count);//0 for now
        }
        [Test]
        public void ActivePanelsTest()
        {
            Assert.IsNotNull(ui.ActivePanels);
            Assert.AreEqual(1, ui.ActivePanels.Count);//0 for now
            Assert.AreEqual(main, ui.ActivePanels.Peek());//0 for now
        }
        [Theory]
        public void MoveSelectorToTest(MenuPanel panel)
        {
            Assert.IsNotNull(ui.ActivePanels);
            Assert.AreEqual(1, ui.ActivePanels.Count);//0 for now
            Assert.Fail();
        }
        [Theory]
        public void OpenPanelTest(MenuPanel panel)
        {
            Assert.IsNotNull(ui.ActivePanels);
            Assert.AreEqual(1, ui.ActivePanels.Count);//0 for now
            Assert.Fail();
        }
        [Test]
        public void ClosePanelTest()
        {
            Assert.IsNotNull(ui.ActivePanels);
            Assert.AreEqual(1, ui.ActivePanels.Count);//0 for now
            Assert.Fail();

        }

    }
}
