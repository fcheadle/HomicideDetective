using HomicideDetective.Things.Marks;
using Xunit;

namespace HomicideDetective.Tests.Things.Marks
{
    public class MarkingTests
    {
        [Fact]
        public void NewMarkingTest()
        {
            Marking mark = new Marking()
            {
                Name = "footprint",
                Description = "with a zig-zag pattern on the sole",
                Adjective = "long",
                Color = "chartreuse"
            };
            
            Assert.Equal("footprint", mark.Name);
            Assert.Equal("long", mark.Adjective);
            Assert.Equal("chartreuse", mark.Color);
            Assert.Equal("with a zig-zag pattern on the sole", mark.Description);
        }
    }
}