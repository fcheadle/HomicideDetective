using Engine.States;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Tests.States
{
    class CrimeSceneInvestigationStateTests : TestBase
    {
        CrimeSceneInvestigationState csi;
        //Keys[] keys;
        [SetUp]
        public void SetUp()
        {
            //keys = new Keys[]
            //{
            //    Keys.A, Keys.S, Keys.D, Keys.W,
            //    Keys.Left, Keys.Down,Keys.Right, Keys.Up,
            //    Keys.NumPad4, Keys.NumPad2, Keys.NumPad6, Keys.NumPad8,
            //};
        }
        [Test]
        public void NewCSITest()
        {
            _game = new MockGame(NewCsi);
            MockGame.RunOnce();
            MockGame.Stop();
            Assert.Pass();
        }
        private void NewCsi(GameTime time)
        {
            Rectangle r = new Rectangle(0, 0, 8, 16);

            csi = new CrimeSceneInvestigationState(100, 100, 100, 100);
            Assert.AreEqual(r, csi.AbsoluteArea);
            r = new Rectangle(0, 0, 100, 100);
            Assert.AreEqual(r.Width, csi.Map.Width);
            Assert.AreEqual(r.Height, csi.Map.Height);
            Assert.NotNull(csi.Player);
        }
    }
}
