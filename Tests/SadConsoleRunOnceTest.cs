using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Tests
{
    class SadConsoleRunOnceTest
    {
        [Test]
        public void Go()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            SadConsole.Game.Create("font-sample.json", Engine.Settings.GameWidth, Engine.Settings.GameHeight);
            watch.Stop();
            var create = watch.ElapsedMilliseconds;
            watch.Reset();
            SadConsole.Game.OnInitialize = Init;
            watch.Start();
            SadConsole.Game.Instance.RunOneFrame();
            watch.Stop();
            var firstRun = watch.ElapsedMilliseconds;
            watch.Reset();
            watch.Start();
            SadConsole.Game.Instance.RunOneFrame();
            watch.Stop();
            var secondRun = watch.ElapsedMilliseconds;
            watch.Reset();
            watch.Start();
            SadConsole.Game.Instance.RunOneFrame();
            watch.Stop();
            var thirdRun = watch.ElapsedMilliseconds;
            watch.Reset();
            watch.Start();
            SadConsole.Game.Instance.RunOneFrame();
            watch.Stop();
            var fourthRun = watch.ElapsedMilliseconds;
            watch.Reset();
            watch.Start();
            SadConsole.Game.Instance.RunOneFrame();
            watch.Stop();
            var fifthRun = watch.ElapsedMilliseconds;
            watch.Reset();

            SadConsole.Game.Instance.Exit();
            SadConsole.Game.Instance.Dispose();
        }

        private void Init() { }
    }
}
