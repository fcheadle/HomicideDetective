using System;
using System.Collections.Generic;
using System.Linq;
using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class MemoriesTests
    {
        [Fact]
        public void MemoriesConstructorTest()
        {
            var thoughts = new Memories();
            Assert.NotNull(thoughts.CurrentThought);
            Assert.NotNull(thoughts.ShortTermMemory);
            Assert.NotNull(thoughts.MidTermMemory);
            Assert.NotNull(thoughts.LongTermMemory);
            Assert.NotNull(thoughts.FalseNarrative);
            
            Assert.Empty(thoughts.ShortTermMemory);
            Assert.Empty(thoughts.MidTermMemory);
            Assert.Empty(thoughts.LongTermMemory);
            Assert.Empty(thoughts.FalseNarrative);
        }
        
        [Fact]//think a single thought, passed in as happening
        public void ThinkSingleThoughtTest()
        {
            var thoughts = new Memories();
            var date = DateTime.Now;
            var happening = new Memory(date, "I saw a bird", "the park");
            
            thoughts.Think(happening);
            Assert.Equal(happening.When, thoughts.CurrentThought.When);
            Assert.Equal(happening.Occurrence, thoughts.CurrentThought.Occurrence);
            Assert.Equal(happening.Who, thoughts.CurrentThought.Who);
            Assert.Equal(happening.Where, thoughts.CurrentThought.Where);
        }
        
        [Fact]//think multiple thoughts, passed in as a timeline
        public void ThinkMultipleThoughtsTest()
        {
            var occurences = new []
            {
                "I saw a bird",
                "I fed a pigeon",
                "I ate a packed lunch",
                "My friend Mark jogged by",
            };
            
            var happenings = new List<Memory>()
            {
                new Memory(DateTime.Now, occurences[0], "the park"),
                new Memory(DateTime.Now, occurences[1], "the park"),
                new Memory(DateTime.Now, occurences[2], "the park"),
                new Memory(DateTime.Now, occurences[3], "the park"),
            };
            var thoughtComponent = new Memories();
            thoughtComponent.Think(happenings);
            
            Assert.Equal(occurences[3], thoughtComponent.CurrentThought.Occurrence);
            Assert.Equal(3, thoughtComponent.ShortTermMemory.Count);
        }

        [Fact]
        public void ThinkFalseFactTest()
        {            
            var thoughts = new Memories();
            var date = DateTime.Now;
            var happening = new Memory(date, "I saw a bird", "the park");
            
            thoughts.ThinkFalseFact(happening);
            Assert.Single(thoughts.FalseNarrative);
            var lie = thoughts.FalseNarrative.First();
            Assert.Equal(happening.When, lie.When);
            Assert.Equal(happening.Occurrence, lie.Occurrence);
            Assert.Equal(happening.Who, lie.Who);
            Assert.Equal(happening.Where, lie.Where);
        }

        [Fact]
        public void ThinkFalseNarrativeTest()
        {
            var occurences = new []
            {
                "I saw a bird",
                "I fed a pigeon",
                "I ate a packed lunch",
                "My friend Mark jogged by",
            };
            
            var happenings = new List<Memory>()
            {
                new Memory(DateTime.Now, occurences[0], "the park"),
                new Memory(DateTime.Now, occurences[1], "the park"),
                new Memory(DateTime.Now, occurences[2], "the park"),
                new Memory(DateTime.Now, occurences[3], "the park"),
            };
            
            var thoughtComponent = new Memories();
            thoughtComponent.ThinkFalseNarrative(happenings);
            
            Assert.Equal(4, thoughtComponent.FalseNarrative.Count);
        }
        
        [Fact]
        public void ShortTermMemoryTest()
        {
            var now = DateTime.Now;
            var tenMinutes = now + TimeSpan.FromMinutes(10);
            var thirtyMinutes = now + TimeSpan.FromMinutes(30);
            var sixtyMinutes = now + TimeSpan.FromMinutes(60);
            var sixtyPlusMinutes = now + TimeSpan.FromMinutes(70);

            var happeningNow = new Memory(now, "test begins", "HomicideDetective");
            var happeningInTenMinutes = new Memory(tenMinutes, "test plus ten minutes", "HomicideDetective");
            var happeningInThirtyMinutes = new Memory(thirtyMinutes, "test plus thirty minutes", "HomicideDetective");
            var happeningInOneHour = new Memory(sixtyMinutes, "test plus one hour", "HomicideDetective");
            var happeningInOneHourPlus = new Memory(sixtyPlusMinutes, "test after more than one hour", "HomicideDetective");
            
            var thoughts = new Memories();
            
            thoughts.Think(happeningNow);
            thoughts.Think(happeningInTenMinutes);
            thoughts.Think(happeningInThirtyMinutes);
            
            Assert.Equal(2, thoughts.ShortTermMemory.Count);
            Assert.Contains(happeningNow, thoughts.ShortTermMemory);
            Assert.Contains(happeningInTenMinutes, thoughts.ShortTermMemory);
            Assert.Equal(happeningInThirtyMinutes, thoughts.CurrentThought);
            
            thoughts.Think(happeningInOneHour);
            
            Assert.Equal(3, thoughts.ShortTermMemory.Count);
            Assert.Contains(happeningNow, thoughts.ShortTermMemory);
            Assert.Contains(happeningInTenMinutes, thoughts.ShortTermMemory);
            Assert.Contains(happeningInThirtyMinutes, thoughts.ShortTermMemory);
            Assert.Equal(happeningInOneHour, thoughts.CurrentThought);
            
            thoughts.Think(happeningInOneHourPlus);
            Assert.Equal(3, thoughts.ShortTermMemory.Count);
            Assert.DoesNotContain(happeningNow, thoughts.ShortTermMemory);
            Assert.Contains(happeningInTenMinutes, thoughts.ShortTermMemory);
            Assert.Contains(happeningInThirtyMinutes, thoughts.ShortTermMemory);
            Assert.Contains(happeningInOneHour, thoughts.ShortTermMemory);
            Assert.Equal(happeningInOneHourPlus, thoughts.CurrentThought);
        }

        [Fact]
        public void MidTermMemoryTest()
        {
            var now = DateTime.Now;
            var sixHours = now + TimeSpan.FromHours(6);
            var twelveHours = now + TimeSpan.FromHours(12);
            var eighteenHours = now + TimeSpan.FromHours(18);
            var twentyFiveHours = now + TimeSpan.FromHours(25);

            var happeningNow = new Memory(now, "test one", "HomicideDetective");
            var happeningInSixHours = new Memory(sixHours, "test two", "HomicideDetective");
            var happeningInTwelveHour = new Memory(twelveHours, "test three", "HomicideDetective");
            var happeningInEighteenHours = new Memory(eighteenHours, "test four", "HomicideDetective");
            var happeningInTwentyFiveHours = new Memory(twentyFiveHours, "test five", "HomicideDetective");
            
            var thoughts = new Memories();
            
            thoughts.Think(happeningNow);
            thoughts.Think(happeningInSixHours);
            thoughts.Think(happeningInTwelveHour);
            thoughts.Think(happeningInEighteenHours);
            
            Assert.Equal(2, thoughts.MidTermMemory.Count);
            thoughts.Think(happeningInTwentyFiveHours);
            Assert.Equal(3, thoughts.MidTermMemory.Count);
        }

        [Fact]
        public void LongTermMemoryTest()
        {

            var now = DateTime.Now;
            var sixHours = now + TimeSpan.FromHours(6);
            var twelveHours = now + TimeSpan.FromHours(12);
            var eighteenHours = now + TimeSpan.FromHours(18);
            var twentyFiveHours = now + TimeSpan.FromHours(25);

            var happeningNow = new Memory(now, "test one", "HomicideDetective");
            var happeningInSixHours = new Memory(sixHours, "test two", "HomicideDetective");
            var happeningInTwelveHour = new Memory(twelveHours, "test three", "HomicideDetective");
            var happeningInEighteenHours = new Memory(eighteenHours, "test four", "HomicideDetective");
            var happeningInTwentyFiveHours = new Memory(twentyFiveHours, "test five", "HomicideDetective");
            
            var thoughts = new Memories();
            
            thoughts.Think(happeningNow);
            thoughts.Think(happeningInSixHours);
            thoughts.Think(happeningInTwelveHour);
            thoughts.Think(happeningInEighteenHours);
            
            Assert.Equal(2, thoughts.MidTermMemory.Count);
            thoughts.Think(happeningInTwentyFiveHours);
            Assert.Equal(3, thoughts.MidTermMemory.Count);
            
            Assert.Single(thoughts.LongTermMemory); //only one event older than 24 hours was in the mid-term memory last we checked

            var newMem = new Memory(DateTime.Now + TimeSpan.FromMinutes(61), "I hiccuped", "here");
            thoughts.Think(newMem);
            Assert.Contains(happeningNow, thoughts.LongTermMemory);
            Assert.DoesNotContain(happeningInSixHours, thoughts.LongTermMemory);
            Assert.DoesNotContain(happeningInTwelveHour, thoughts.LongTermMemory);
            Assert.DoesNotContain(happeningInEighteenHours, thoughts.LongTermMemory);
            Assert.DoesNotContain(happeningInTwentyFiveHours, thoughts.LongTermMemory);
        }
    }
}