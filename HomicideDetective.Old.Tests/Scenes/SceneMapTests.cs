using HomicideDetective.Old.Scenes;
using HomicideDetective.Old.Tests.Mocks;
using NUnit.Framework;
using SadConsole;

namespace HomicideDetective.Old.Tests.Scenes
{
    class SceneMapTests : TestBase
    {
        SceneMap _map;
        [Test]
        public void NewSceneMapTest()
        {
            // ReSharper disable once AccessToStaticMemberViaDerivedType
            _map = MockGame.UiManager.Map;
            Assert.NotNull(_map);
            for (int i = 0; i < _map.Width; i++)
            {
                for (int j = 0; j < _map.Height; j++)
                {
                    Assert.NotNull(_map.GetTerrain<BasicTerrain>(i, j));
                }
            }
        }
    }
}
