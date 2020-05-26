using Engine.UI;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.UI
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
            csi = new CrimeSceneInvestigationState(100, 100, 100, 100);
        }
    }
}
