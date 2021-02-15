using HomicideDetective.Mysteries;
using Xunit;

namespace HomicideDetective.Tests.Mysteries
{
    public class MysteryTests
    {
        [Fact]
        public void NewMysteryTest()
        {
            var mystery = new Mystery(0, 1);
            Assert.Equal(1, mystery.Seed);
            Assert.Equal(1, mystery.CaseNumber);
            Assert.Null(mystery.Victim);
            Assert.Null(mystery.Witnesses);
            Assert.Null(mystery.CurrentScene);
            Assert.Null(mystery.SceneOfCrime);
            Assert.Null(mystery.Murderer);
        }

        [Theory]
        [InlineData(79)]
        [InlineData(144)]
        [InlineData(169)]
        [InlineData(4026)]
        [InlineData(10050)]
        [InlineData(69696)]
        [InlineData(999999)]
        public void CommitMurderTest(int seed)
        {
            var mystery = new Mystery(seed, 0);
            mystery.CommitMurder();
            Assert.NotNull(mystery.Victim);
            Assert.NotNull(mystery.Witnesses);
            Assert.NotNull(mystery.SceneOfCrime);
            Assert.NotNull(mystery.Murderer);
            Assert.NotNull(mystery.MurderWeapon);
        }

        [Theory]
        [InlineData(79)]
        [InlineData(144)]
        [InlineData(169)]
        [InlineData(4026)]
        [InlineData(10050)]
        [InlineData(69696)]
        [InlineData(999999)]
        public void GeneratePersonTest(int seed)
        {
            var firstMystery = new Mystery(seed, 0);
            var secondMystery = new Mystery(seed, 0);
            
            var firstAnswer = firstMystery.GeneratePerson("test");
            var secondAnswer = secondMystery.GeneratePerson("test");
            
            Assert.Equal(firstAnswer.Details, secondAnswer.Details);
            // Assert.NotNull(firstAnswer.SizeDescription);
            // Assert.NotEmpty(firstAnswer.SizeDescription);
            // Assert.NotNull(firstAnswer.WeightDescription);
            // Assert.NotEmpty(firstAnswer.WeightDescription);
        }
        
        [Theory]
        [InlineData(79)]
        [InlineData(144)]
        [InlineData(169)]
        [InlineData(4026)]
        [InlineData(10050)]
        [InlineData(69696)]
        [InlineData(999999)]
        public void GenerateMurderWeaponTest(int seed)
        {
            var firstMystery = new Mystery(seed, 0);
            var secondMystery = new Mystery(seed, 0);
            
            var firstAnswer = firstMystery.GenerateMurderWeapon();
            var secondAnswer = secondMystery.GenerateMurderWeapon();
            
            Assert.Equal(firstAnswer.Details, secondAnswer.Details);
            Assert.NotNull(firstAnswer.SizeDescription);
            Assert.NotEmpty(firstAnswer.SizeDescription);
            Assert.NotNull(firstAnswer.WeightDescription);
            Assert.NotEmpty(firstAnswer.WeightDescription);
        }
    }
}