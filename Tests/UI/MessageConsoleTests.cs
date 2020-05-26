using Engine.Maps.Areas;
using Engine.UI;
using GoRogue;
using GameTime = Microsoft.Xna.Framework.GameTime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.UI
{
    //class MessageConsoleTests : TestBase
    //{
    //    static MessageConsole console { get; set; }
    //    private static void NewMessageConsole(GameTime time)
    //    {
    //        console = new MessageConsole(12, 14);
    //    }

    //    [Test]
    //    public void NewMessageConsoleTest()
    //    {
    //        _game = new MockGame(NewMessageConsole);
    //        MockGame.RunOnce();
    //        Assert.AreEqual(12, console.Width);
    //        Assert.AreEqual(14, console.Height);
    //        MockGame.Stop();
    //    }

    //    private static void PrintString(GameTime time)
    //    {
    //        console = new MessageConsole(12, 14);
    //        string[] messages = { "test message", "serenity", "ankylosaurus" };
    //        console.Print(messages);
    //    }
    //    [Test]
    //    public void PrintStringTest()
    //    {
    //        _game = new MockGame(PrintString);
    //        MockGame.RunOnce();
    //        string answer = console.GetString(1, 12);
    //        Assert.AreEqual("test message", answer, "the full answer was distorted somehow.");
    //    }

    //    private static void PrintAreas(GameTime time)
    //    {
    //        console = new MessageConsole(12, 14);
    //        Area a = new Area("area A", new Coord(4, 4), new Coord(4, 0), new Coord(0, 0), new Coord(0, 4));
    //        Area b = new Area("area B", new Coord(4, 4), new Coord(4, 0), new Coord(0, 0), new Coord(0, 4));
    //        Area c = new Area("area C", new Coord(4, 4), new Coord(4, 0), new Coord(0, 0), new Coord(0, 4));
    //        Area d = new Area("area D", new Coord(4, 4), new Coord(4, 0), new Coord(0, 0), new Coord(0, 4));
    //        Area[] areas = { a, b, c, d };
    //        console.Print(areas);
    //    }
    //    [Test]
    //    public void PrintAreaTest()
    //    {
    //        _game = new MockGame(PrintAreas);
    //        MockGame.RunOnce();
    //        string answer = console.GetString(0, 6);
    //        Assert.AreEqual("area A", answer);
    //    }
    //}
}
