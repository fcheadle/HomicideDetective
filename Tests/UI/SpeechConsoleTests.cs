using Engine.UI;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.UI
{
    class SpeechConsoleTests : TestBase
    {

        [Test]
        public void NewSpeechConsoleTest()
        {
            //todo: font
            SpeechConsole console = new SpeechConsole(12, 14, new Coord());
            Assert.AreEqual(12, console.Width);
            Assert.AreEqual(14, console.Height);
        }
        //[Test]
        public void FadesOutOnUpdateTest()
        {
            Assert.Fail("Not Implemented.");
        }
    }
}
