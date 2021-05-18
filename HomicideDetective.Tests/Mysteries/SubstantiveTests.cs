using System.Collections.Generic;
using System.Runtime.Serialization;
using HomicideDetective.Mysteries;
using Xunit;

namespace HomicideDetective.Tests.Mysteries
{
    public class SubstantiveTests
    {
        [DataMember] 
        public static IEnumerable<object[]> Types = new List<object[]>
        {
            new object[] {Substantive.Types.Person},
            new object[] {Substantive.Types.Place},
            new object[] {Substantive.Types.Thing},
        };
        
        [Theory]
        [MemberData(nameof(Types))]
        public void NewSubstantiveTest(Substantive.Types type)
        {
            var substantive = new Substantive(type, "Test", gender: "male",article: "",pronoun: "he", pronounPossessive: "his",
                description: "a test substantive", mass: 36, volume: 64, sizeDescription: "smol", weightDescription: "lite");
            Assert.Equal("Test", substantive.Name);    
            Assert.Equal("a test substantive", substantive.Description);    
            Assert.Equal(36, substantive.Mass);    
            Assert.Equal(64, substantive.Volume);    
            Assert.Equal("smol", substantive.SizeDescription);    
            Assert.Equal("lite", substantive.WeightDescription);    
        }

        [Theory]
        [MemberData(nameof(Types))]
        public void DetailsTest(Substantive.Types type)
        {
            var substantive = new Substantive(type, "Test", gender: "male",article: "",pronoun: "he", pronounPossessive: "his",
                description: "a test substantive", mass: 36, volume: 64, sizeDescription: "smol", weightDescription: "lite");
            var answer = substantive.Details;
            Assert.Equal(5, answer.Count);
            Assert.Contains("Test", answer);
            Assert.Contains("a test substantive", answer);
            Assert.Contains("Mass: 36g", answer);
            Assert.Contains("Volume: 64ml", answer);
        }
        
        [Theory]
        [MemberData(nameof(Types))]
        public void DetailedDescriptionTest(Substantive.Types type)
        {
            var substantive = new Substantive(type, "Test", gender: "male",article: "a ",pronoun: "he", pronounPossessive: "his",
                description: "a test substantive", mass: 36, volume: 64, sizeDescription: "smol", weightDescription: "lite");
            var answer = substantive.GenerateDetailedDescription();
            Assert.True(answer.Contains("This is a Test.") || answer.Contains("This is Test."));
            Assert.Contains("It smol, and it lite", answer);
        }
    }
}