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
            var info = new Substantive(Substantive.Types.Person, "Testboy");
            rle.AllComponents.Add(info);
            rle.AllComponents.Add(new Personhood());
            return rle;
        }
        [Fact]
        public void PersonhoodConstructorTest()
        {
            var personhood = new Personhood();
            Assert.NotNull(personhood.Memories);
            Assert.NotNull(personhood.BodyLanguage);
            Assert.NotNull(personhood.Voice);
            Assert.Throws<NullReferenceException>(() =>
            {
                return personhood.Info;
            });
        }

        [Fact]
        public void SpeakToTest()
        {
            var rle = CreateTestEntity();
            var personhood = rle.AllComponents.GetFirst<Personhood>();
            Assert.Equal("Testboy", personhood.Info.Name);

            Assert.Equal("Hello Detective.", personhood.SpeakTo());
            Assert.Equal("My name is Testboy.", personhood.SpeakTo());
        }
    }
}