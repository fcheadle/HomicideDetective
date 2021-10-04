using HomicideDetective.People;
using HomicideDetective.Words;
using SadRogue.Integration;
using Xunit;

namespace HomicideDetective.Tests
{
    public static class TestUtilities
    {
        public static void AssertEntitiesMatch(RogueLikeEntity firstAnswer, RogueLikeEntity secondAnswer)
        {
            var first = firstAnswer.Info();
            var second = secondAnswer.Info();
            AssertSubstantiveMatch(first, second);
        }

        public static void AssertSubstantiveMatch(ISubstantive first, ISubstantive second)
        {
            Assert.Equal(first.Name, second.Name);
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

        public static void AssertVictim(RogueLikeEntity entity)
        {
            //victim should have an ISubstantive of some kind
            var subs = entity.AllComponents.GetFirstOrDefault<ISubstantive>();
            Assert.NotNull(subs);

            //victim should NOT have Personhood, because they are dead
            var personhood = entity.AllComponents.GetFirstOrDefault<Personhood>();
            Assert.Null(personhood);
            
            AssertPerson(subs!);
        }

        public static void AssertPerson(RogueLikeEntity entity) =>
            AssertPerson(entity.Info());

        public static void AssertPerson(ISubstantive subs)
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

        public static void AssertPlace(ISubstantive subs)
        {
            //place should have a name and description
            Assert.NotNull(subs.Name);
            Assert.NotEmpty(subs.Name);
            Assert.NotNull(subs.Description);
            Assert.NotEmpty(subs.Description);

            //Places should use item pronouns
            var pronouns = Constants.ItemPronouns;
            Assert.Equal(pronouns.Objective, subs.Pronouns.Objective);
            Assert.Equal(pronouns.Subjective, subs.Pronouns.Subjective);
            Assert.Equal(pronouns.Possessive, subs.Pronouns.Possessive);
            Assert.Equal(pronouns.Reflexive, subs.Pronouns.Reflexive);
        }

        public static void AssertThing(RogueLikeEntity entity) => AssertThing(entity.Info());
        public static void AssertThing(ISubstantive subs)
        {
            //thing should have a name and description
            Assert.NotNull(subs.Name);
            Assert.NotEmpty(subs.Name);
            Assert.NotNull(subs.Description);
            Assert.NotEmpty(subs.Description);
            
            //thing should use item pronouns
            var pronouns = Constants.ItemPronouns;
            Assert.Equal(pronouns.Objective, subs.Pronouns.Objective);
            Assert.Equal(pronouns.Subjective, subs.Pronouns.Subjective);
            Assert.Equal(pronouns.Possessive, subs.Pronouns.Possessive);
            Assert.Equal(pronouns.Reflexive, subs.Pronouns.Reflexive);
        }
    }
}