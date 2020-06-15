using Engine.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Utilities
{
    class FormulaeTests
    {
        [Test]
        public void RandomFunction3dTest()
        {
            var f = Formulae.RandomTerrainGenFormula();
            double answer = f(77, -14);
            Assert.AreNotEqual(0.0001, answer); //I mean, statistically...
        }
        [Test]
        public void RandomFunction4dTest()
        {
            var f = Formulae.RandomWindPattern();
            double answer = f(63, 35, TimeSpan.FromMilliseconds(100));
            Assert.AreNotEqual(0.0001, answer); //I mean, statistically...
        }

        [Test]
        public void BoundedTanTest()
        {
            for (int i = 0; i < 100; i++)
            {
                double fx = Formulae.BoundedTan(i);
                Assert.Greater(1.01, fx);
                Assert.Less(-1.01, fx);
            }
        }
    }
}
