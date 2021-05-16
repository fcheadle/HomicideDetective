using HomicideDetective.Mysteries;
using Xunit;

namespace HomicideDetective.Tests.Mysteries
{
    public class TimeTests
    {
        [Theory]
        [InlineData(9, 00)]
        [InlineData(13, 24)]
        [InlineData(15, 45)]
        [InlineData(11, 20)]
        [InlineData(5, 17)]
        public void ToIntTest(int hours, int minutes)
        {
            var time = new Time(hours, minutes);
            var expected = hours * 100 + minutes;
            Assert.Equal(hours, time.Hours);
            Assert.Equal(minutes, time.Minutes);
            Assert.Equal(expected, time.ToInt());
        }
        
        [Theory]
        [InlineData(9, 00)]
        [InlineData(13, 24)]
        [InlineData(15, 45)]
        [InlineData(11, 20)]
        [InlineData(5, 17)]
        public void ToStringTest(int hours, int minutes)
        {
            Time test = new Time(hours, minutes);
            Assert.Equal($"{test.ToInt()} hundred hours", test.ToString());
        }
        
        [Theory]
        [InlineData(9, 00)]
        [InlineData(13, 24)]
        [InlineData(15, 45)]
        [InlineData(11, 20)]
        [InlineData(5, 17)]
        public void LessThanTest(int hours, int minutes)
        {
            Time test = new Time(hours, minutes);
            Time begin = new Time(0, 0);
            Time end = new Time(23, 59);
            Assert.True(test.LessThan(end));
            Assert.False(test.LessThan(begin));
        }
        [Theory]
        [InlineData(9, 00)]
        [InlineData(13, 24)]
        [InlineData(15, 45)]
        [InlineData(11, 20)]
        [InlineData(5, 17)]
        public void GreaterThanTest(int hours, int minutes)
        {
            Time test = new Time(hours, minutes);
            Time begin = new Time(0, 0);
            Time end = new Time(23, 59);
            Assert.True(test.GreaterThan(begin));
            Assert.False(test.GreaterThan(end));
        }
        [Fact]
        public void EqualTest()
        {
            Time left = new Time(11, 15);
            Time right = new Time(20, 59);
            Assert.False(left.Equals(right));

            right = new Time(11, 15);
            Assert.True(left.Equals(right));
        }
        [Fact]
        public void IncrementMinutesTest()
        {
            Time time = new Time(00, 00);
            for (int i = 0; i < 200; i++)
            {
                Assert.Equal(i % 60, time.Minutes);
                time.Minutes++;
            }
        }
        [Fact]
        public void IncrementHoursTest()
        {            
            Time time = new Time(00, 00);
            for (int i = 0; i < 200; i++)
            {
                Assert.Equal(i % 24, time.Hours);
                time.Hours++;
            }
            
        }
    }
}