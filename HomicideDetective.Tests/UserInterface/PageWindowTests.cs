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

        [Fact]
        public void NewContentTitleTriggersNewPageTest()
        {
            var page = new PageWindow(_width, _height);
            var pcs1 = new PageContentSource("title1", "content1");
            var pcs2 = new PageContentSource("title2", "content2");
            var pcs3 = new PageContentSource("title3", "content3");
            Assert.Equal(0, page.PageNumber);
            page.Write(pcs1);
            Assert.Equal(0, page.PageNumber);
            page.Write(pcs2);
            Assert.Equal(1, page.PageNumber);
            page.Write(pcs3);
            Assert.Equal(2, page.PageNumber);
        }

        [Fact]
        public void RepeatTitleAddsToSamePageTest()
        {
            var page = new PageWindow(_width, _height);
            var pcs1 = new PageContentSource("title1", "content1");
            var pcs2 = new PageContentSource("title1", "content2");
            var pcs3 = new PageContentSource("title1", "content3");
            page.Write(pcs1);
            Assert.Equal(0, page.PageNumber);
            page.Write(pcs2);
            Assert.Equal(0, page.PageNumber);
            page.Write(pcs3);
            Assert.Equal(0, page.PageNumber);
            Assert.Contains(pcs1.Contents[0], page.Contents[0].Contents);
            Assert.Contains(pcs2.Contents[0], page.Contents[0].Contents);
            Assert.Contains(pcs3.Contents[0], page.Contents[0].Contents);
        }
    }
}