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
        

        private void AssertEntitiesMatch(RogueLikeEntity firstAnswer, RogueLikeEntity secondAnswer)
        {
            var first = firstAnswer.Info();
            var second = secondAnswer.Info();
            AssertSubstantiveMatch(first, second);
        }

        private void AssertSubstantiveMatch(ISubstantive first, ISubstantive second)
        {
            Assert.Equal(first.Name, second.Name);
            Assert.Equal(first.Details, second.Details);
            Assert.Equal(first.Description, second.Description);
            
            Assert.Equal(first.Pronouns.Objective, second.Pronouns.Objective);
            Assert.Equal(first.Pronouns.Subjective, second.Pronouns.Subjective);
            Assert.Equal(first.Pronouns.Possessive, second.Pronouns.Possessive);
            Assert.Equal(first.Pronouns.Reflexive, second.Pronouns.Reflexive);
            
            Assert.Equal(first.Nouns.Singular, second.Nouns.Singular);
            Assert.Equal(first.Nouns.Plural, second.Nouns.Plural);
            
            if(first.UsageVerb == null) 
                Assert.Null(second.UsageVerb);
            else
            {
                Assert.NotNull(second.UsageVerb);
                Assert.Equal(first.UsageVerb.Infinitive, second.UsageVerb!.Infinitive);
                Assert.Equal(first.UsageVerb.PastTense.FirstPersonSingular, second.UsageVerb.PastTense.FirstPersonSingular);
                Assert.Equal(first.UsageVerb.PastTense.FirstPersonPlural, second.UsageVerb.PastTense.FirstPersonPlural);
                Assert.Equal(first.UsageVerb.PastTense.SecondPersonSingular, second.UsageVerb.PastTense.SecondPersonSingular);
                Assert.Equal(first.UsageVerb.PastTense.SecondPersonPlural, second.UsageVerb.PastTense.SecondPersonPlural);
                Assert.Equal(first.UsageVerb.PastTense.ThirdPersonSingular, second.UsageVerb.PastTense.ThirdPersonSingular);
                Assert.Equal(first.UsageVerb.PastTense.ThirdPersonPlural, second.UsageVerb.PastTense.ThirdPersonPlural);
                
                Assert.Equal(first.UsageVerb.PresentTense.FirstPersonSingular, second.UsageVerb.PresentTense.FirstPersonSingular);
                Assert.Equal(first.UsageVerb.PresentTense.FirstPersonPlural, second.UsageVerb.PresentTense.FirstPersonPlural);
                Assert.Equal(first.UsageVerb.PresentTense.SecondPersonSingular, second.UsageVerb.PresentTense.SecondPersonSingular);
                Assert.Equal(first.UsageVerb.PresentTense.SecondPersonPlural, second.UsageVerb.PresentTense.SecondPersonPlural);
                Assert.Equal(first.UsageVerb.PresentTense.ThirdPersonSingular, second.UsageVerb.PresentTense.ThirdPersonSingular);
                Assert.Equal(first.UsageVerb.PresentTense.ThirdPersonPlural, second.UsageVerb.PresentTense.ThirdPersonPlural);
                
                Assert.Equal(first.UsageVerb.FutureTense.FirstPersonSingular, second.UsageVerb.FutureTense.FirstPersonSingular);
                Assert.Equal(first.UsageVerb.FutureTense.FirstPersonPlural, second.UsageVerb.FutureTense.FirstPersonPlural);
                Assert.Equal(first.UsageVerb.FutureTense.SecondPersonSingular, second.UsageVerb.FutureTense.SecondPersonSingular);
                Assert.Equal(first.UsageVerb.FutureTense.SecondPersonPlural, second.UsageVerb.FutureTense.SecondPersonPlural);
                Assert.Equal(first.UsageVerb.FutureTense.ThirdPersonSingular, second.UsageVerb.FutureTense.ThirdPersonSingular);
                Assert.Equal(first.UsageVerb.FutureTense.ThirdPersonPlural, second.UsageVerb.FutureTense.ThirdPersonPlural);
            }        
        }

        private void AssertVictim(RogueLikeEntity entity)
        {
            //victim should have an ISubstantive of some kind
            var subs = entity.AllComponents.GetFirstOrDefault<ISubstantive>();
            Assert.NotNull(subs);

            //victim should NOT have Personhood, because they are dead
            var personhood = entity.AllComponents.GetFirstOrDefault<Personhood>();
            Assert.Null(personhood);
            
            AssertPerson(subs!);
        }

        private void AssertPerson(RogueLikeEntity entity) =>
            AssertPerson(entity.Info());

        private void AssertPerson(ISubstantive subs)
        {
            //person should have a name and description
            Assert.NotNull(subs.Name);
            Assert.NotEmpty(subs.Name);
            Assert.NotNull(subs.Description);
            Assert.NotEmpty(subs.Description);

            //person should not use item pronouns
            var pronouns = Constants.ItemPronouns;
            Assert.NotEqual(pronouns.Objective, subs.Pronouns.Objective);
            Assert.NotEqual(pronouns.Subjective, subs.Pronouns.Subjective);
            Assert.NotEqual(pronouns.Possessive, subs.Pronouns.Possessive);
            Assert.NotEqual(pronouns.Reflexive, subs.Pronouns.Reflexive);
        }

        private void AssertPlace(ISubstantive subs)
        {
            //place should have a name and description
            Assert.NotNull(subs.Name);
            Assert.NotEmpty(subs.Name);
            Assert.NotNull(subs.Description);
            Assert.NotEmpty(subs.Description);
            
            //Places don't have usage verbs
            Assert.Null(subs.UsageVerb);
            
            //Places should use item pronouns
            var pronouns = Constants.ItemPronouns;
            Assert.Equal(pronouns.Objective, subs.Pronouns.Objective);
            Assert.Equal(pronouns.Subjective, subs.Pronouns.Subjective);
            Assert.Equal(pronouns.Possessive, subs.Pronouns.Possessive);
            Assert.Equal(pronouns.Reflexive, subs.Pronouns.Reflexive);
        }

        private void AssertThing(RogueLikeEntity entity) => AssertThing(entity.Info());
        private void AssertThing(ISubstantive subs)
        {
            //thing should have a name and description
            Assert.NotNull(subs.Name);
            Assert.NotEmpty(subs.Name);
            Assert.NotNull(subs.Description);
            Assert.NotEmpty(subs.Description);
            
            //thing should have at least one or two details
            Assert.NotEmpty(subs.Details);
            
            //thing should use item pronouns
            var pronouns = Constants.ItemPronouns;
            Assert.Equal(pronouns.Objective, subs.Pronouns.Objective);
            Assert.Equal(pronouns.Subjective, subs.Pronouns.Subjective);
            Assert.Equal(pronouns.Possessive, subs.Pronouns.Possessive);
            Assert.Equal(pronouns.Reflexive, subs.Pronouns.Reflexive);
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
            
            firstMystery.Generate(100, 100, 50, 50);
            secondMystery.Generate(100, 100, 50, 50);
            
            AssertEntitiesMatch(firstMystery.Victim, secondMystery.Victim);
            AssertSubstantiveMatch(firstMystery.SceneOfCrimeInfo, secondMystery.SceneOfCrimeInfo);
            AssertEntitiesMatch(firstMystery.Murderer, secondMystery.Murderer);
            AssertEntitiesMatch(firstMystery.MurderWeapon, secondMystery.MurderWeapon);
            Assert.Equal(firstMystery.Witnesses.Count, secondMystery.Witnesses.Count);
            
            for (int i = 0; i < firstMystery.Witnesses.Count; i++)
            {
                var firstSub = firstMystery.Witnesses[i].Info();
                var secondSub = secondMystery.Witnesses[i].Info();
                AssertSubstantiveMatch(firstSub, secondSub);
            }
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
            AssertPerson(entity);
        }
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateWitnessEntitiesTest(int seed)
        {
            var entities = new Mystery(seed, 0).GenerateWitnessEntities();
            Assert.NotEmpty(entities);
            foreach(var entity in entities) 
                AssertPerson(entity);
        }
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateSceneOfMurderInfoTest(int seed)
        {
            var scene = new Mystery(seed, 0).GenerateSceneOfMurderInfo("trailer home");
            Assert.Contains("trailer home", scene.Name);
            AssertPlace(scene);
        }
        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateMiscellaneousItemTest(int seed)
        {
            var thing = new Mystery(seed, 0).GenerateMiscellaneousItem();
            AssertThing(thing);
        }
    }
}