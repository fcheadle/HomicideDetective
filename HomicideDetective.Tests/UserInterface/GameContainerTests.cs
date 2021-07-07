using System;
using HomicideDetective.UserInterface;
using Xunit;

namespace HomicideDetective.Tests.UserInterface
{
    public class GameContainerTests
    {
        private const int _year = 1970;
        private const int _month = 7;
        private const int _day = 5;
        private const int _hour = 18;
        private const int _minute = 0;
        
        public GameContainerTests()
        {
            new TestHost();
        }
        
        [Fact]
        public void InitGameContainerTest()
        {
            var game = new GameContainer();
            var expectedDate = new DateTime(_year, _month, _day, _hour, _minute, 0);
            Assert.Equal(expectedDate, game.CurrentTime);
            Assert.NotNull(game.Map);
            Assert.NotNull(game.PlayerCharacter);
            Assert.NotNull(game.MessageWindow);
            Assert.NotNull(game.Mystery);
        }
    }
}