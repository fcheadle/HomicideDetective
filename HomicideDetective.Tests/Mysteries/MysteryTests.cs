using System.Collections.Generic;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using Xunit;
// ReSharper disable ObjectCreationAsStatement

namespace HomicideDetective.Tests.Mysteries
{
    public class MysteryTests
    {
        
        public static readonly IEnumerable<object[]> IntData = new List<object[]>()
        {
            new object[] {79},
            new object[] {144},
            new object[] {169},
            new object[] {4026},
            new object[] {10050},
            new object[] {69696},
            new object[] {999999},
        };
        
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateMysteryTest(int seed)
        {
            new TestHost();
            var mystery = new Mystery(seed, 0);
            mystery.Generate(100, 100, 50, 50);
            Assert.NotNull(mystery.Victim);
            Assert.NotNull(mystery.Victim.AllComponents.GetFirstOrDefault<Substantive>());
            Assert.NotNull(mystery.SceneOfCrimeInfo);
            Assert.NotNull(mystery.Murderer);
            var murdererPersonhood = mystery.Murderer.AllComponents.GetFirst<Personhood>();
            Assert.NotNull(murdererPersonhood.Speech);
            Assert.NotNull(murdererPersonhood.Memories);
            
            Assert.NotNull(mystery.MurderWeapon);
            Assert.NotNull(mystery.MurderWeapon.AllComponents.GetFirst<Substantive>());
        }

        [Theory]
        [MemberData(nameof(IntData))]
        public void GeneratePersonalInfoTest(int seed)
        {
            var firstMystery = new Mystery(seed, 0);
            var secondMystery = new Mystery(seed, 0);
            
            var firstAnswer = firstMystery.GeneratePersonalInfo("test");
            var secondAnswer = secondMystery.GeneratePersonalInfo("test");
            
            // Assert.Equal(firstAnswer.Details, secondAnswer.Details);
            // Assert.Equal(firstAnswer.Name, secondAnswer.Name);
            Assert.NotNull(firstAnswer.Name);
            Assert.NotEmpty(firstAnswer.Name);
            Assert.NotNull(secondAnswer.Name);
            Assert.NotEmpty(secondAnswer.Name);
        }
        
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateMurderWeaponInfoTest(int seed)
        {
            var firstMystery = new Mystery(seed, 0);
            var secondMystery = new Mystery(seed, 0);
            
            var firstAnswer = firstMystery.GenerateMurderWeaponInfo();
            var secondAnswer = secondMystery.GenerateMurderWeaponInfo();
            
            Assert.Equal(firstAnswer.Details, secondAnswer.Details);
            Assert.Equal(firstAnswer.Name, secondAnswer.Name);
            Assert.NotNull(firstAnswer.Name);
            Assert.NotEmpty(firstAnswer.Name);
            Assert.NotNull(secondAnswer.Name);
            Assert.NotEmpty(secondAnswer.Name);
        }
        
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateSceneOfMurderInfoTest(int seed)
        {
            var firstMystery = new Mystery(seed, 0);
            var secondMystery = new Mystery(seed, 0);
            
            var firstAnswer = firstMystery.GenerateSceneOfMurderInfo();
            var secondAnswer = secondMystery.GenerateSceneOfMurderInfo();
            
            Assert.Equal(firstAnswer.Details, secondAnswer.Details);
            Assert.Equal(firstAnswer.Name, secondAnswer.Name);
            Assert.NotNull(firstAnswer.Name);
            Assert.NotEmpty(firstAnswer.Name);
            Assert.NotNull(secondAnswer.Name);
            Assert.NotEmpty(secondAnswer.Name);
        }
        
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateVictimTest(int seed)
        {
            new TestHost();
            var firstMystery = new Mystery(seed, 0);
            var secondMystery = new Mystery(seed, 0);
            firstMystery.Generate(100, 100, 50, 50);
            secondMystery.Generate(100, 100, 50, 50);
            var firstAnswer = firstMystery.Victim;
            var secondAnswer = secondMystery.Victim;
            var firstSubstantive = firstAnswer.AllComponents.GetFirst<Substantive>();
            var secondSubstantive = firstAnswer.AllComponents.GetFirst<Substantive>();
            
            Assert.Equal(firstAnswer.Name, secondAnswer.Name);
            Assert.NotNull(firstSubstantive.Name);
            Assert.NotEmpty(firstSubstantive.Name);
            Assert.NotNull(secondSubstantive.Name);
            Assert.NotEmpty(secondSubstantive.Name);
            Assert.Equal(firstSubstantive.Details, secondSubstantive.Details);
            Assert.Equal(firstSubstantive.Description, secondSubstantive.Description);
            Assert.NotNull(firstSubstantive.Description);
            Assert.NotEmpty(firstSubstantive.Description);
            Assert.NotNull(secondSubstantive.Description);
            Assert.NotEmpty(secondSubstantive.Description);
        }
        
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateMurdererTest(int seed)
        {
            new TestHost();
            var firstMystery = new Mystery(seed, 0);
            var secondMystery = new Mystery(seed, 0);
            firstMystery.Generate(100, 100, 50, 50);
            secondMystery.Generate(100, 100, 50, 50);
            var firstAnswer = firstMystery.Murderer.AllComponents.GetFirst<Personhood>();
            var secondAnswer = secondMystery.Murderer.AllComponents.GetFirst<Personhood>();
            
            Assert.NotNull(firstAnswer.Name);
            Assert.NotEmpty(firstAnswer.Name);
            Assert.NotNull(secondAnswer.Name);
            Assert.NotEmpty(secondAnswer.Name);
            // Assert.Equal(firstAnswer.Details, secondAnswer.Details);
            // Assert.Equal(firstAnswer.Description, secondAnswer.Description);
            Assert.NotNull(firstAnswer.Description);
            Assert.NotEmpty(firstAnswer.Description);
            Assert.NotNull(secondAnswer.Description);
            Assert.NotEmpty(secondAnswer.Description);
        }

        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateMurderWeaponTest(int seed)
        {
            new TestHost();
            var firstMystery = new Mystery(seed, 0);
            var secondMystery = new Mystery(seed, 0);
            firstMystery.Generate(100, 100, 50, 50);
            secondMystery.Generate(100, 100, 50, 50);
            var firstAnswer = firstMystery.MurderWeapon;
            var firstSubstantive = firstAnswer.AllComponents.GetFirst<Substantive>();
            
            var secondAnswer = secondMystery.Victim;
            var secondSubstantive = firstAnswer.AllComponents.GetFirst<Substantive>();
            
            Assert.Equal(firstAnswer.Name, secondAnswer.Name);
            Assert.NotNull(firstSubstantive.Name);
            Assert.NotEmpty(firstSubstantive.Name);
            Assert.NotNull(secondSubstantive.Name);
            Assert.NotEmpty(secondSubstantive.Name);
            Assert.Equal(firstSubstantive.Details, secondSubstantive.Details);
            Assert.Equal(firstSubstantive.Description, secondSubstantive.Description);
            Assert.NotNull(firstSubstantive.Description);
            Assert.NotEmpty(firstSubstantive.Description);
            Assert.NotNull(secondSubstantive.Description);
            Assert.NotEmpty(secondSubstantive.Description);
        }

        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateWitnessesTest(int seed)
        {
            new TestHost();
            var mystery = new Mystery(seed, 0);
            mystery.Generate(100, 100, 50, 50);

            foreach (var entity in mystery.GenerateWitnessEntities())
            {
                var witness = entity.AllComponents.GetFirst<Personhood>();
                Assert.NotNull(witness.Name);
                Assert.NotEmpty(witness.Name);
                Assert.NotNull(witness.Description);
                Assert.NotEmpty(witness.Description);
                
                Assert.NotNull(witness.Speech);
                Assert.NotNull(witness.Memories);
            }
        }
    }
}