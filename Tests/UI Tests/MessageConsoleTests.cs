using Engine.Maps;
using Engine.UI;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    class MessageConsoleTests
    {
        //[Test] //can't instantiate new Message Consoles in v8
        public void NewMessageConsoleTest()
        {
            MessageConsole console = new MessageConsole(12, 14);
            Assert.AreEqual(12, console.Width);
            Assert.AreEqual(14, console.Height);
        }
        //[Test]
        public void PrintStringTest()
        {
            MessageConsole console = new MessageConsole(12, 14);
            string[] messages = { "test message 1", "serenity", "ankylosaurus" };
            console.Print(messages);
            string answer = console.GetString(0, 14);
            Assert.AreEqual("test message 1", answer);
        }
        //[Test]
        public void PrintAreaTest()
        {
            MessageConsole console = new MessageConsole(12, 14);
            Area a = new Area("area A", new Coord(4, 4), new Coord(4, 0), new Coord(0, 0), new Coord(0, 4));
            Area b = new Area("area B", new Coord(4, 4), new Coord(4, 0), new Coord(0, 0), new Coord(0, 4));
            Area c = new Area("area C", new Coord(4, 4), new Coord(4, 0), new Coord(0, 0), new Coord(0, 4));
            Area d = new Area("area D", new Coord(4, 4), new Coord(4, 0), new Coord(0, 0), new Coord(0, 4));
            Area[] areas = { a, b, c, d };
            console.Print(areas);
            string answer = console.GetString(2, 6);
            Assert.AreEqual("area C", answer);
        }
    }
}
