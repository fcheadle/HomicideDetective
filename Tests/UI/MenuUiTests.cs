using Engine.UI;
using NUnit.Framework;
using SadConsole;
using System.Collections.Generic;
using Tests.Mocks;
// ReSharper disable AccessToStaticMemberViaDerivedType

namespace Tests.UI
{
    class MenuUiTests : TestBase
    {
        MenuUi _ui;
        static MenuPanel _main;
        static HelpPanel _help;
        static MenuPanel _settings;
        static MenuPanel _newGame;
        static MenuPanel _newGameAdvanced;
        Stack<MenuPanel> _activePanels;
        BasicEntity _selector;

        // ReSharper disable once UnusedMember.Local
        private static MenuPanel[] Panels => new[]
        {
            _main,
            _help,
            _settings,
            _newGame,
            _newGameAdvanced,
        };
        [SetUp]
        public void Setup()
        {
            _ui = MockGame.Menu;
            _main = _ui.MainOptions;
            _help = _ui.HelpOptions;
            _settings = _ui.SettingsOptions;
            _activePanels = _ui.ActivePanels;
            _selector = _ui.Player;
            _newGame = _ui.NewGameOptions;
            _newGameAdvanced = _ui.NewGameAdvancedOptions;

            //Panels = new MenuPanel[] { main, help, settings, newGame, newGameAdvanced };
        }
        [Test]
        public void NewMenuUiTest()
        {
            Assert.NotNull(_ui);
            Assert.NotNull(_selector);
            Assert.NotNull(_main);
            Assert.NotNull(_help);
            Assert.NotNull(_settings);
            Assert.NotNull(_activePanels);
            Assert.NotNull(_newGame);
            Assert.NotNull(_newGameAdvanced);
        }
        [Test]
        public void TitleConsoleTest()
        {
            Assert.NotNull(_ui.TitleConsole);
        }
        [Test]
        public void MainOptionsTest()
        {
            Assert.IsNotNull(_ui.MainOptions);
            Assert.AreEqual(4, _ui.MainOptions.Controls.Count);
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
            Assert.AreEqual(14, _ui.SettingsOptions.Controls.Count);
        }
        [Test]
        public void NewGameOptionsTest()
        {
            Assert.IsNotNull(_ui.NewGameOptions);
            Assert.AreEqual(2, _ui.NewGameOptions.Controls.Count);//quickstart / advanced
        }
        [Test]
        public void NewGameAdvancedOptionsTest()
        {
            Assert.IsNotNull(_ui.NewGameAdvancedOptions);
            Assert.AreEqual(1, _ui.NewGameAdvancedOptions.Controls.Count);//0 for now
        }
        [Test]
        public void ActivePanelsTest()
        {
            Assert.IsNotNull(_ui.ActivePanels);
            Assert.AreEqual(1, _ui.ActivePanels.Count);//0 for now
            Assert.AreEqual(_main, _ui.ActivePanels.Peek());//0 for now
        }

        [Test]
        public void OpenPanelTest()
        {
            int start = _ui.ActivePanels.Count;
            _ui.OpenPanel(_settings);
            Assert.AreEqual(start + 1, _ui.ActivePanels.Count);
            Assert.True(_settings.IsVisible);
            Assert.True(_settings.IsFocused);
        }
        [Test]
        public void ClosePanelTest()
        {
            Assert.IsNotNull(_ui.ActivePanels);
            int startAmount = _ui.ActivePanels.Count;
            _ui.OpenPanel(_newGame);
            _ui.OpenPanel(_newGameAdvanced);

            Assert.AreEqual(startAmount + 2, _ui.ActivePanels.Count);

            _ui.ClosePanel(_newGameAdvanced);
            Assert.AreEqual(startAmount + 1, _ui.ActivePanels.Count);
            Assert.False(_newGameAdvanced.IsVisible);
            Assert.False(_newGameAdvanced.IsFocused);
        }

    }
}
