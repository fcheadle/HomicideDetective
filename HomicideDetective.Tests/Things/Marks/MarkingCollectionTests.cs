using HomicideDetective.Things;
using Xunit;

namespace HomicideDetective.Tests.Things.Marks
{
    public class MarkingCollectionTests
    {
        [Fact]
        public void NewMarkingsTest()
        {
            var subject = new MarkingCollection();
            Assert.Empty(subject.MarkingsOn);
            Assert.Empty(subject.MarkingsLeftBy);
        }
        [Fact]
        public void AddUnlimitedMarkingsTest()
        {
            var subject = new MarkingCollection();
            var fingerprint = new Marking
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
            var subject = new MarkingCollection();
            var fingerprint = new Marking
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
            var subject = new MarkingCollection();
            var fingerprint = new Marking
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