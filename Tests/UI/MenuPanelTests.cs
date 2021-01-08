using HomicideDetective.Old.UI;
using GoRogue;
using NUnit.Framework;
using SadConsole.Controls;

namespace Tests.UI
{
    class MenuPanelTests : TestBase
    {
        [Test]
        public void NewMenuPanelTest()
        {
            MenuPanel panel = new MenuPanel(10, 10);
            Assert.AreEqual(10, panel.Width);
            Assert.AreEqual(10, panel.Height);
            Assert.AreEqual(0, panel.Controls.Count);

            Assert.NotNull(panel.Selector);
            Assert.AreEqual(1, panel.Components.Count);
        }
        [Test]
        public void SlideLeftTest()
        {
            MenuPanel panel = new MenuPanel(10, 10);
            panel.Position = new Coord(HomicideDetective.Old.Game.Settings.GameWidth / 3, 0);
            panel.SlideLeft();
            Assert.AreEqual(new Coord(HomicideDetective.Old.Game.Settings.GameWidth / 6 + 1, 0), (Coord)panel.Position);
        }
        [Test]
        public void SlideRightTest()
        {
            MenuPanel panel = new MenuPanel(10, 10);
            panel.SlideRight();
            Assert.AreEqual(new Coord(HomicideDetective.Old.Game.Settings.GameWidth / 6, 0), (Coord)panel.Position);
        }
        [Test]
        public void SlideUpTest()
        {
            MenuPanel panel = new MenuPanel(10, 10);
            panel.SlideUp();
            Assert.AreEqual(new Coord(0, -HomicideDetective.Old.Game.Settings.GameHeight / 6), (Coord)panel.Position);
        }
        [Test]
        public void SlideDownTest()
        {

            MenuPanel panel = new MenuPanel(10, 10);
            panel.SlideDown();
            Assert.AreEqual(new Coord(0, HomicideDetective.Old.Game.Settings.GameHeight / 6), (Coord)panel.Position);
        }
        //[Test]
        public void FadeInTest()
        {
            Assert.Fail();
        }
        //[Test]
        public void FadeOutTest()
        {
            Assert.Fail();
        }
        [Test]
        public void ArrangeTest()
        {
            MenuPanel panel = new MenuPanel(10, 10);
            panel.Add(new Button(6) { Text = "button1" });
            panel.Add(new Button(6) { Text = "button2" });
            panel.Add(new Button(6) { Text = "button3" });
            panel.Add(new Button(6) { Text = "button4" });
            panel.Add(new Button(6) { Text = "button5" });
            panel.Arrange();

            Assert.AreEqual(2, panel.Controls[0].Position.Y);
            Assert.AreEqual(4, panel.Controls[1].Position.Y);
            Assert.AreEqual(6, panel.Controls[2].Position.Y);
            Assert.AreEqual(8, panel.Controls[3].Position.Y);
            Assert.AreEqual(10, panel.Controls[4].Position.Y);
        }
    }
}
