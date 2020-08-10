using Engine;
using GoRogue;
using NUnit.Framework;
using System.Linq;

namespace Tests.Utilities
{
    class SettingsTests
    {
        int size = 360;
        Settings _settings = new Settings();
        [Test]
        [Category("NonGraphical")]
        public void GetSettingsTest()
        {

            Assert.AreEqual(size, _settings.MapWidth);
            Assert.AreEqual(size, _settings.MapHeight);
            Assert.AreEqual(120, _settings.GameWidth);
            Assert.AreEqual(40, _settings.GameHeight);
            Assert.AreEqual(false, _settings.IsPaused);
            Assert.AreEqual(false, _settings.ShowingMenu);
            Assert.Less(0, _settings.Random.Next(5, 10));
            Assert.AreEqual(Radius.CIRCLE, _settings.FovRadius);
            Assert.AreEqual(16, _settings.MovementKeyBindings.Count());
        }
    }
}
