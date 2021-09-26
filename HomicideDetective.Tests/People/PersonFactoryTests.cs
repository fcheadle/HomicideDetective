using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class PersonFactoryTests
    {
        [Fact]
        public void MakePersonTest()
        {
            var answer1 = PersonFactory.GeneratePersonalInfo(1024, "Garold");
            var answer2 = PersonFactory.GeneratePersonalInfo(1024, "Garold");
            Assert.Contains("Garold", answer1.Name);
            Assert.Contains("Garold", answer2.Name);
            Assert.Equal(answer1.Name, answer2.Name);
            Assert.True(answer1.Age > 0);
            Assert.True(answer2.Age > 0);
            Assert.Equal(answer1.Age, answer2.Age);
            Assert.NotNull(answer1.Speech);
            Assert.NotNull(answer2.Speech);
            Assert.NotNull(answer1.Details);
            Assert.NotNull(answer2.Details);
            Assert.Equal(answer1.Details, answer2.Details);
            Assert.Equal(answer1.Fingerprint.Seed, answer2.Fingerprint.Seed);
            Assert.NotNull(answer1.Markings);
            Assert.NotNull(answer2.Markings);
            Assert.Equal(answer1.Markings.MarkingsLeftBy, answer2.Markings.MarkingsLeftBy);
            Assert.Equal(answer1.Markings.MarkingsOn, answer2.Markings.MarkingsOn);
            Assert.NotNull(answer1.Memories);
            Assert.NotNull(answer1.Nouns);
            Assert.Equal(answer1.Nouns, answer2.Nouns);
            Assert.NotNull(answer1.Pronouns);
            Assert.Equal(answer1.Pronouns, answer2.Pronouns);
            Assert.Null(answer1.UsageVerb);
            Assert.Null(answer2.UsageVerb);
            
            //will throw exception if speech is not set up correctly
            var greeting = answer1.Greet();
            Assert.Contains("Hello", greeting);
            Assert.Contains("Detective", greeting);

            greeting = answer2.Greet();
            Assert.Contains("Hello", greeting);
            Assert.Contains("Detective", greeting);
        }
    }
}