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
            timeline.Add(startTime, startString);
            timeline.Add(middleTime, middleString);
            timeline.Add(endTime, endString);
            
            Assert.Equal(timeline.DoingAtTime(startTime), startString);
            Assert.Equal(timeline.DoingAtTime(new Time(9,00)), startString);
            Assert.Equal(timeline.DoingAtTime(new Time(9,30)), startString);
            
            Assert.Equal(timeline.DoingAtTime(middleTime), middleString);
            Assert.Equal(timeline.DoingAtTime(new Time(13,20)), middleString);
            Assert.Equal(timeline.DoingAtTime(new Time(14,40)), middleString);
            
            Assert.Equal(timeline.DoingAtTime(endTime), endString);
            Assert.Equal(timeline.DoingAtTime(new Time(17,20)), endString);
            Assert.Equal(timeline.DoingAtTime(new Time(18,20)), endString);
        }
    }
}