using HomicideDetective.UserInterface;
using Xunit;

namespace HomicideDetective.Tests.UserInterface
{
    public class PageContentSourceTests
    {
        [Fact]
        public void GetPrintableStringTest()
        {
            var title = "Conversation with Anne Hathaway";
            var content = "her: 'What are you doing, detective?'\r\nMe:'Don't worry about it'";
            var source = new PageContentSource(title, content);
            Assert.Contains(title, source.GetPrintableString());
            Assert.Contains(content, source.GetPrintableString());
        }
    }
}