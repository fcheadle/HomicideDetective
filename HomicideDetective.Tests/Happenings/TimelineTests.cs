using System;
using HomicideDetective.Happenings;
using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.Happenings
{
    public class TimelineTests
    {
        [Fact]
        public void MostRecentStringTest()
        {
            string startString = "go to work";
            DateTime startTime = new DateTime(1970, 07,04, 08, 30, 0);
            string middleString = "eat lunch";
            DateTime middleTime = new DateTime(1970, 07,04, 12, 05, 0);
            string endString = "leave work";
            DateTime endTime = new DateTime(1970, 07,04, 16, 45, 0);;
            
            var timeline = new Timeline();
            timeline.Add(new Memory(startTime, startString, "office", false));
            timeline.Add(new Memory(middleTime, middleString, "office", false));
            timeline.Add(new Memory(endTime, endString, "office", false));
            
            Assert.Equal(timeline.OccurrenceAtTime(startTime), startString);
            Assert.Equal(timeline.OccurrenceAtTime(new DateTime(1970,7,4,9,00,0)), startString);
            Assert.Equal(timeline.OccurrenceAtTime(new DateTime(1970,7,4,9,00,0)), startString);
            
            Assert.Equal(timeline.OccurrenceAtTime(middleTime), middleString);
            Assert.Equal(timeline.OccurrenceAtTime(new DateTime(1970,7,4,13,20,0)), middleString);
            Assert.Equal(timeline.OccurrenceAtTime(new DateTime(1970,7,4,14,40,0)), middleString);
            
            Assert.Equal(timeline.OccurrenceAtTime(endTime), endString);
            Assert.Equal(timeline.OccurrenceAtTime(new DateTime(1970,7,4,17,20,0)), endString);
            Assert.Equal(timeline.OccurrenceAtTime(new DateTime(1970,7,4,18,20,0)), endString);
        }
    }
}