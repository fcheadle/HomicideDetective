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
        [SetUp]
        public void SetUp()
        {
            ui = MockGame.UIManager;
        }
        [Test]
        public void NewCsiUiTest()
        {
            Assert.IsNotNull(ui.Map);
            Assert.IsNotNull(ui.Display);
            Assert.IsNotNull(ui.Controls);
            Assert.IsNotNull(ui.LookingGlass);
            Assert.IsNotNull(ui.ControlledGameObject);
            Assert.IsNotNull(ui.Actor);
            Assert.IsNotNull(ui.KeyBoardComponent);
            Assert.IsNotNull(ui.Thoughts);
            Assert.IsNotNull(ui.Health);
        }
    }
}
