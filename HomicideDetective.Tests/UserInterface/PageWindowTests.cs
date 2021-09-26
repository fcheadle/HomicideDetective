using System;
using HomicideDetective.UserInterface;
using SadConsole.Components;
using Xunit;

namespace HomicideDetective.Tests.UserInterface
{
    public class PageWindowTests
    {
        private class Printable : IPrintable
        {
            public string GetPrintableString() => "this is printable, right?";
        }
        
        private int _width = 16;
        private int _height = 4;

        public PageWindowTests()
        {
            new TestHost();
        }
        
        [Fact]
        public void NewPageWindowTest()
        {
            var page = new PageWindow(_width, _height);
            Assert.NotNull(page.BackgroundSurface);
            Assert.NotNull(page.TextSurface);
            Assert.NotNull(page.TextSurface.GetSadComponent<Cursor>());
            Assert.Equal(_width, page.Width);
            Assert.Equal(_width, page.BackgroundSurface.Surface.Width);
            Assert.Equal(_height, page.Height);
            Assert.Equal(_height, page.BackgroundSurface.Surface.Height);
            Assert.Equal(_width - 3, page.TextSurface.Surface.Width);
            Assert.Empty(page.Contents);
            Assert.Equal(0, page.PageNumber);
        }
    }
}