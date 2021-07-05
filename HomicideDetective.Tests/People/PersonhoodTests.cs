using System;
using HomicideDetective.People;
using SadRogue.Integration;
using SadRogue.Primitives;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class PersonhoodTests
    {
        private RogueLikeEntity CreateTestEntity()
        {
            var rle = new RogueLikeEntity((0, 0), Color.White, Color.Black, 1);
            var info = new Substantive(ISubstantive.Types.Person, "Testboy");
            rle.AllComponents.Add(info);
            rle.AllComponents.Add(new Person(info.Name, info.Description, info.Noun, info.Pronoun, info.PronounPossessive));
            return rle;
        }
        [Fact]
        public void PersonhoodConstructorTest()
        {
            var personhood = new Person();
            Assert.NotNull(personhood.Memories);
            Assert.NotNull(personhood.BodyLanguage);
            Assert.NotNull(personhood.Voice);
        }

        [Fact]
        public void SpeakToTest()
        {
            var rle = CreateTestEntity();
            var personhood = rle.AllComponents.GetFirst<Person>();
            Assert.Equal("Testboy", personhood.Name);

            Assert.Equal("Hello Detective.", personhood.SpeakTo());
            Assert.Equal("My name is Testboy.", personhood.SpeakTo());
        }
    }
}