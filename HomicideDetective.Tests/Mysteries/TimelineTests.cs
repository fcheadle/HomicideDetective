using HomicideDetective.Mysteries;
using Xunit;

namespace HomicideDetective.Tests.Mysteries
{
    public class TimelineTests
    {
        [Fact]
        public void MostRecentStringTest()
        {
            string startString = "go to work";
            Time startTime = new Time(08, 30);
            string middleString = "eat lunch";
            Time middleTime = new Time(12, 05);
            string endString = "leave work";
            Time endTime = new Time(16, 45);
            
            var timeline = new Timeline();
            timeline.Add(new Happening(startTime, startString));
            timeline.Add(new Happening(middleTime, middleString));
            timeline.Add(new Happening(endTime, endString));
            
            Assert.Equal(timeline.OccurrenceAtTime(startTime), startString);
            Assert.Equal(timeline.OccurrenceAtTime(new Time(9,00)), startString);
            Assert.Equal(timeline.OccurrenceAtTime(new Time(9,30)), startString);
            
            Assert.Equal(timeline.OccurrenceAtTime(middleTime), middleString);
            Assert.Equal(timeline.OccurrenceAtTime(new Time(13,20)), middleString);
            Assert.Equal(timeline.OccurrenceAtTime(new Time(14,40)), middleString);
            
            Assert.Equal(timeline.OccurrenceAtTime(endTime), endString);
            Assert.Equal(timeline.OccurrenceAtTime(new Time(17,20)), endString);
            Assert.Equal(timeline.OccurrenceAtTime(new Time(18,20)), endString);
        }
    }
}