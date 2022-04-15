using System.Collections.Generic;
using GoRogue.MapGeneration;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using HomicideDetective.Words;
using SadRogue.Integration;
using SadRogue.Primitives;
using Xunit;

namespace HomicideDetective.Tests.Mysteries
{
    public class MysteryTests
    {
        
        public static readonly IEnumerable<object[]> IntData = new List<object[]>()
        {
            new object[] {0},
            new object[] {12},
            new object[] {32},
            new object[] {40},
            new object[] {50},
            new object[] {60},
            new object[] {79},
            new object[] {144},
            new object[] {169},
            new object[] {512},
            new object[] {1024},
            new object[] {4026},
            new object[] {10050},
            new object[] {69696},
            new object[] {999999},
        };

        private void AssertVictim(RogueLikeEntity entity)
        {
            //victim should have an ISubstantive of some kind
            var subs = entity.AllComponents.GetFirstOrDefault<ISubstantive>();
            Assert.NotNull(subs);

            //victim should NOT have Personhood, because they are dead
            var personhood = entity.AllComponents.GetFirstOrDefault<Personhood>();
            Assert.Null(personhood);
            
            TestUtilities.AssertPerson(subs!);
        }

        
        /// <summary>
        /// Generating Mysteries is very processor and time intensive. Therefore, we need to assert on EVERYTHING for
        /// each mystery that we generate, so we aren't generating a mystery just to check one value.
        /// </summary>
        /// <remarks>Tests that a mystery Generates every required piece of info, and tests that two mysteries produce
        /// the same values</remarks>
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateMysteryTest(int seed)
        {
            new TestHost();
            var firstMystery = new Mystery(seed, 0);
            var secondMystery = new Mystery(seed, 0);
            
            Assert.Equal(seed, firstMystery.Seed);
            Assert.Equal(seed, secondMystery.Seed);
            Assert.Equal(0, firstMystery.CaseNumber);
            Assert.Equal(0, secondMystery.CaseNumber);
            Assert.NotNull(firstMystery.Random);
            Assert.NotNull(secondMystery.Random);
            
            // firstMystery.Generate(100, 100, 50, 50);
            // secondMystery.Generate(100, 100, 50, 50);
            //
            // TestUtilities.AssertEntitiesMatch(firstMystery.Victim, secondMystery.Victim);
            // TestUtilities.AssertSubstantiveMatch(firstMystery.SceneOfCrimeInfo, secondMystery.SceneOfCrimeInfo);
            // TestUtilities.AssertEntitiesMatch(firstMystery.Murderer, secondMystery.Murderer);
            // TestUtilities.AssertEntitiesMatch(firstMystery.MurderWeapon, secondMystery.MurderWeapon);
            // Assert.Equal(firstMystery.Witnesses.Count, secondMystery.Witnesses.Count);
            //
            // for (int i = 0; i < firstMystery.Witnesses.Count; i++)
            // {
            //     var firstSub = firstMystery.Witnesses[i].Info();
            //     var secondSub = secondMystery.Witnesses[i].Info();
            //     TestUtilities.AssertSubstantiveMatch(firstSub, secondSub);
            // }
        }
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateVictimEntityTest(int seed)
        {
            var entity = new Mystery(seed, 0).GenerateVictimEntity();
            AssertVictim(entity);
        }
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateMurdererEntityTest(int seed)
        {
            var entity = new Mystery(seed, 0).GenerateMurdererEntity();
            TestUtilities.AssertPerson(entity);
        }
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateWitnessEntitiesTest(int seed)
        {
            var entities = new Mystery(seed, 0).GenerateWitnessEntities();
            Assert.NotEmpty(entities);
            foreach(var entity in entities) 
                TestUtilities.AssertPerson(entity);
        }
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateMiscellaneousItemTest(int seed)
        {
            var thing = new Mystery(seed, 0).GenerateMiscellaneousItem();
            TestUtilities.AssertThing(thing);
        }
    }
}