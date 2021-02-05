using Xunit;
using HomicideDetective.Things.Marks;

namespace HomicideDetective.Tests.Things.Marks
{
    public class MarkingsTests
    {
        [Fact]
        public void NewMarkingsTest()
        {
            var subject = new Markings();
            Assert.Empty(subject.MarkingsOn);
            Assert.Empty(subject.MarkingsLeftBy);
        }
        [Fact]
        public void AddUnlimitedMarkingsTest()
        {
            var subject = new Markings();
            var fingerprint = new Mark
            {
                Adjective = "greasy",
                Color = "yellow",
                Description = "faint",
                Name = "grease print"
            };
            
            subject.AddUnlimitedMarkings(fingerprint);
            Assert.Single(subject.MarkingsLeftBy);
        }
        [Fact]
        public void AddLimitedMarkingsTest()
        {
            var subject = new Markings();
            var fingerprint = new Mark
            {
                Adjective = "greasy",
                Color = "yellow",
                Description = "faint",
                Name = "grease print"
            };
            
            subject.AddLimitedMarkings(fingerprint, 5);
            Assert.Equal(5, subject.MarkingsLeftBy.Count);
        }
        [Fact]
        public void LeaveMarkTest()
        {
            var subject = new Markings();
            var fingerprint = new Mark
            {
                Adjective = "greasy",
                Color = "yellow",
                Description = "faint",
                Name = "grease print"
            };
            
            subject.AddLimitedMarkings(fingerprint, 5);
            Assert.Equal(5, subject.MarkingsLeftBy.Count);
            Assert.Equal(fingerprint, subject.LeaveMark());
            Assert.Equal(fingerprint, subject.LeaveMark());
            Assert.Equal(fingerprint, subject.LeaveMark());
            Assert.Equal(fingerprint, subject.LeaveMark());
            Assert.Equal(fingerprint, subject.LeaveMark());
            
            subject.AddUnlimitedMarkings(fingerprint);
            Assert.Single(subject.MarkingsLeftBy);
            Assert.Equal(fingerprint, subject.LeaveMark());
            Assert.Equal(fingerprint, subject.LeaveMark());
            Assert.Equal(fingerprint, subject.LeaveMark());
            Assert.Equal(fingerprint, subject.LeaveMark());
            Assert.Equal(fingerprint, subject.LeaveMark());
        }
    }
}