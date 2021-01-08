using HomicideDetective.Old.UI;
using NUnit.Framework;
using Tests.Mocks;
// ReSharper disable AccessToStaticMemberViaDerivedType

namespace Tests.UI
{
    class CrimeSceneInvestigationUiTests : TestBase
    {
        CrimeSceneInvestigationUi _ui;
        [SetUp]
        public void SetUp()
        {
            _ui = MockGame.UiManager;
        }
        [Test]
        public void NewCsiUiTest()
        {
            Assert.IsNotNull(_ui.Map);
            Assert.IsNotNull(_ui.Display);
            Assert.IsNotNull(_ui.Controls);
            Assert.IsNotNull(_ui.LookingGlass);
            Assert.IsNotNull(_ui.Player);
            Assert.IsNotNull(_ui.Actor);
            Assert.IsNotNull(_ui.KeyBoardComponent);
            Assert.IsNotNull(_ui.Thoughts);
            Assert.IsNotNull(_ui.Health);
        }
    }
}
