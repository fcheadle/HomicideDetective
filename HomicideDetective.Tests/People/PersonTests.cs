using System;
using System.Collections.Generic;
using System.Linq;
using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class PersonTests
    {
        private readonly string _name = "Testboy";
        private readonly string _description = "a test class instance";
        private readonly string _noun = "person";
        private readonly string _pronoun = "him";
        private readonly string _pronounPossessive = "his";

        private readonly string _testLab = "the lab";

        public static readonly IEnumerable<object[]> DateTimes = new List<object[]>()
        {
            new object[]{ DateTime.Now},
            new object[]{ DateTime.Now - TimeSpan.FromMinutes(60)},
            new object[]{ DateTime.Now - TimeSpan.FromMinutes(120)},
            new object[]{ DateTime.Now - TimeSpan.FromMinutes(180)},
            new object[]{ DateTime.Now - TimeSpan.FromMinutes(240)},
            new object[]{ DateTime.Now - TimeSpan.FromMinutes(299)},
        };

        public static readonly IEnumerable<object[]> PeopleNames = new List<object[]>()
        {
            new object[] { "billy bob" },
            new object[] { "skeeter" },
            new object[] { "gator" },
            new object[] { "cletus" },
            new object[] { "clyde" },
            new object[] { "lyn" },
            new object[] { "katie" },
            new object[] { "kora" },
        };

        public static readonly IEnumerable<object[]> PlaceNames = new List<object[]>()
        {
            new object[] { "skate park" },
            new object[] { "alley" },
            new object[] { "theatre" },
            new object[] { "arcade" },
            new object[] { "bowling alley" },
            new object[] { "parlor" },
            new object[] { "conservatory" },
            new object[] { "sunroom" },
        };

        public static readonly IEnumerable<object[]> ItemNames = new List<object[]>()
        {
            new object[] { "skateboard" },
            new object[] { "switchblade" },
            new object[] { "hairdryer" },
            new object[] { "pistol" },
            new object[] { "bowling pin" },
            new object[] { "scissors" },
            new object[] { "rope" },
            new object[] { "candlestick" },
        };

        private Personhood CreateTestEntity() =>
            new(_name, _description, _noun, _pronoun, _pronounPossessive, 24, Occupations.Professor, 64, 128);
        
        private void CreateTestTimeline(Personhood person)
        {
            for (int i = 300; i > 0; i--)
            {
                var now = DateTime.Now - TimeSpan.FromMinutes(i);
                var index = i % PeopleNames.Count();
                var whoWith = PeopleNames.ToList()[index][0].ToString();
                var memory = new Memory(now, $"'i' is {i}", _testLab, new []{whoWith});
                person.Memories.Think(memory);
            }
        }
        [Fact]
        public void PersonalInfoTest()
        {
            var person = CreateTestEntity();
            Assert.True(_name == person.Name, "Name did not match what we provided in our parameter");
            Assert.True(_description == person.Description, "Description did not match what we provided in our parameter");
            Assert.True(_noun == person.Noun, "Noun did not match what we provided in our parameter");
            Assert.True(_pronoun == person.Pronoun, "Pronoun did not match what we provided in our parameter");
            Assert.True(_pronounPossessive == person.PronounPossessive, "Pronoun did not match what we provided in our parameter");
        }

        [Fact]
        public void PersonHasMemoriesTest()
        {
            var person = CreateTestEntity();
            Assert.NotNull(person.Memories);
        }

        [Fact]
        public void PersonHasVoiceTest()
        {
            var person = CreateTestEntity();
            Assert.NotNull(person.Speech);
            Assert.NotEmpty(person.Speech.VoiceDescription);
        }

        [Fact]
        public void GetPrintableStringTest()
        {
            var person = CreateTestEntity();
            var answer = person.GetPrintableString();
            Assert.Contains("This is a person", answer);
        }

        [Fact]
        public void SpeakToPersonTest()
        {
            var expectedGreeted = "Hello Detective";
            var expectedIntro = "My name is Testboy";
            var person = CreateTestEntity();
            var answer = person.SpeakTo();
            Assert.Contains(expectedGreeted, answer);
            Assert.Contains(expectedIntro, answer);
        }

        [Fact]
        public void GreetPersonTest()
        {
            var hello = "Hello";
            var detective = "Detective";
            var person = CreateTestEntity();

            Assert.Contains(hello, person.Greet());
            Assert.Contains(detective, person.Greet());
        }

        [Fact]
        public void IntroduceSelfTest()
        {
            var expected = "My name is Testboy";
            var person = CreateTestEntity();

            Assert.Contains(expected, person.Introduce());
        }

        [Fact(Skip = "Not Implemented")]
        public void InquireAboutSelfTest()
        {
            var person = CreateTestEntity();
            Assert.Contains(_description, person.InquireAboutSelf());
        }
        
        [Theory(Skip = "Not Implemented")]
        [MemberData(nameof(DateTimes))]
        public void InquireWhereaboutsTest(DateTime atTime)
        {
            var person = CreateTestEntity();
            CreateTestTimeline(person);
            Assert.Contains(_testLab, person.InquireWhereabouts(atTime));
        }
        
        [Theory(Skip = "Not Implemented")]
        [MemberData(nameof(DateTimes))]
        public void InquireWhoWithTest(DateTime atTime)
        {
            var person = CreateTestEntity();
            CreateTestTimeline(person);
            Assert.Contains(_testLab, person.InquireAboutCompany(atTime));
        }
        
        [Theory(Skip = "Not Implemented")]
        [MemberData(nameof(DateTimes))]
        public void InquireAboutMemoryTest(DateTime atTime)
        {
            var person = CreateTestEntity();
            CreateTestTimeline(person);
            Assert.Contains(_testLab, person.InquireAboutMemory(atTime));
        }
        
        [Theory(Skip = "Not Implemented")]
        [MemberData(nameof(PeopleNames))]
        public void InquireAboutOtherPersonTest(string name)
        {
            var person = CreateTestEntity();
            CreateTestTimeline(person);
            Assert.Contains(_testLab, person.InquireAboutPerson(name));
        }

        [Theory(Skip = "Not Implemented")]
        [MemberData(nameof(PlaceNames))]
        public void InquireAboutPlaceTest(string place)
        {
            var person = CreateTestEntity();
            CreateTestTimeline(person);
            Assert.Contains(_testLab, person.InquireAboutPlace(place));
            
        }

        [Theory(Skip = "Not Implemented")]
        [MemberData(nameof(ItemNames))]
        public void InquireAboutThingTest(string thing)
        {
            var person = CreateTestEntity();
            CreateTestTimeline(person);
            Assert.Contains(_testLab, person.InquireAboutThing(thing));
        }
    }
}