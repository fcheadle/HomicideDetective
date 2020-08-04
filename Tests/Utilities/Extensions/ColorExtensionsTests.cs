using Engine.Utilities.Extensions;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Tests.Utilities
{
    [TestFixture]
    public class ColorExtensionsTests
    {
        [Test]
        [Category("NonGraphical")]
        public void DarkenTest()
        {
            Color c = Color.White.Darken();
            Assert.LessOrEqual(c.R, Color.White.R);
            Assert.LessOrEqual(c.G, Color.White.G);
            Assert.LessOrEqual(c.B, Color.White.B);
        }
        [Test]
        [Category("NonGraphical")]
        public void BrightenTest()
        {
            Color c = Color.Black.Brighten();
            Assert.GreaterOrEqual(c.R, Color.Black.R);
            Assert.GreaterOrEqual(c.G, Color.Black.G);
            Assert.GreaterOrEqual(c.B, Color.Black.B);
        }
        [Test]
        [Category("NonGraphical")]
        public void FadeOutTest()
        {
            Color c = Color.Black.FadeOut();
            Assert.Less(c.A, Color.Black.A);
        }
        [Test]
        [Category("NonGraphical")]
        public void FadeInTest()
        {
            Color c = Color.Transparent.FadeIn();
            Assert.Greater(c.A, Color.Transparent.A);
        }

        [Test]
        [Category("NonGraphical")]
        public void MutateToIndexTest()
        {
            Color g = Color.Green.MutateToIndex(-9.55);
            Assert.AreEqual(g.B, Color.Green.B);
            Assert.AreEqual(g.R, Color.Green.R);
            Assert.LessOrEqual(g.G, Color.Green.G);

            Color b = Color.Blue.MutateToIndex(8.77);
            Assert.GreaterOrEqual(b.B, Color.Blue.B);
            Assert.AreEqual(b.R, Color.Blue.R);
            Assert.AreEqual(b.G, Color.Blue.G);

            Color r = Color.Red.MutateToIndex(1.22);
            Assert.AreEqual(r.B, Color.Red.B);
            Assert.GreaterOrEqual(r.R, Color.Red.R);
            Assert.AreEqual(r.G, Color.Red.G);
        }

        [Test]
        [Category("NonGraphical")]
        public void MutateByTest()
        {
            Color c = Color.Red.MutateBy(Color.Blue);
            Assert.Greater(c.B, Color.Red.B);
            Assert.Less(c.R, Color.Red.R);
            Assert.Greater(c.B, Color.Red.B);
        }

        [Test]
        [Category("NonGraphical")]
        public void HalfTest()
        {
            Color c = Color.Red.Half();
            Assert.AreEqual(c.B, Color.Red.B);
            Assert.Less(c.R, Color.Red.R);
            Assert.AreEqual(c.G, Color.Red.G);
            Assert.AreEqual(c.B, Color.Red.B);
        }

        [Test]
        [Category("NonGraphical")]
        public void DoubleTest()
        {
            Color c = Color.DarkBlue.Double();
            Assert.AreEqual(c.A, Color.DarkBlue.A);
            Assert.AreEqual(c.R, Color.DarkBlue.R);
            Assert.AreEqual(c.G, Color.DarkBlue.G);
            Assert.Greater(c.B, Color.DarkBlue.B);
        }

        [Test]
        [Category("NonGraphical")]
        public void Greenify()
        {
            Color c = Color.Purple.Greenify();
            Assert.AreEqual(c.A, Color.Purple.A);
            Assert.Less(c.R, Color.Purple.R);
            Assert.Greater(c.G, Color.Purple.G);
            Assert.Less(c.B, Color.Purple.B);
        }

        [Test]
        [Category("NonGraphical")]
        public void Redify()
        {
            Color c = Color.Cyan.Redify();
            Assert.AreEqual(c.A, Color.Cyan.A);
            Assert.Greater(c.R, Color.Cyan.R);
            Assert.Less(c.G, Color.Cyan.G);
            Assert.Less(c.B, Color.Cyan.B);
        }

        [Test]
        [Category("NonGraphical")]
        public void Blueify()
        {
            Color c = Color.Yellow.Blueify();
            Assert.AreEqual(c.A, Color.Yellow.A);
            Assert.Less(c.R, Color.Yellow.R);
            Assert.Less(c.G, Color.Yellow.G);
            Assert.Greater(c.B, Color.Yellow.B);
        }
    }
}
