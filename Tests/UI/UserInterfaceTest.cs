using Engine.UI;
using NUnit.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.UI
{
    class UserInterfaceTest : TestBase
    {
        UserInterface _ui;

        [SetUp]
        public void SetUp()
        {
            _ui = (UserInterface)Global.CurrentScreen;
        }
        [Test]
        public void NewUITest()
        {
            Assert.NotNull(_ui);
        }
        [Test]
        public void HideTest()
        {
            Assert.True(_ui.IsVisible);
            Assert.AreEqual(Global.CurrentScreen, _ui);

            _ui.Hide();
            Assert.False(_ui.IsVisible);
            Assert.False(_ui.IsFocused);
        }
        [Test]
        public void ShowTest()
        {
            _ui.IsVisible = false;
            _ui.IsFocused = false;
            
            _ui.Show();
            Assert.True(_ui.IsVisible);
            Assert.True(_ui.IsFocused);
            Assert.AreEqual(Global.CurrentScreen, _ui);
        }
    }
}
