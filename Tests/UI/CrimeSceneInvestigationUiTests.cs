using Engine.UI;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.UI
{
    class CrimeSceneInvestigationUiTests : TestBase
    {
        CrimeSceneInvestigationUi ui;
        [Test]
        public void NewCsiUiTest()
        {
            _game = new MockGame(NewUI);
            _game.RunOnce();
            ui = MockGame.UIManager;
            Assert.IsNotNull(ui.Map);
            Assert.IsNotNull(ui.MapRenderer);
            Assert.IsNotNull(ui.Controls);
            Assert.IsNotNull(ui.LookingGlass);
            Assert.IsNotNull(ui.Player);
            Assert.IsNotNull(ui.Actor);
            Assert.IsNotNull(ui.KeyBoardComponent);
            Assert.IsNotNull(ui.Thoughts);
            Assert.IsNotNull(ui.Health);
        }

        private void NewUI(GameTime obj)
        {
        }
    }
}
