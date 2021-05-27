using System.Collections.Generic;
using System.Runtime.Serialization;
using Xunit;

namespace HomicideDetective.Tests
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
                description: "a test substantive", mass: 36, volume: 64);
            Assert.Equal("Test", substantive.Name);    
            Assert.Equal("a test substantive", substantive.Description);    
            Assert.Equal(36, substantive.Mass);    
            Assert.Equal(64, substantive.Volume);    
        }

        [Theory]
        [MemberData(nameof(Types))]
        public void DetailsTest(Substantive.Types type)
        {
            var substantive = new Substantive(type, "Test", gender: "male",article: "",pronoun: "he", pronounPossessive: "his",
                description: "a test substantive", mass: 36, volume: 64);
            
            Assert.Empty(substantive.Details);
            substantive.AddDetail("arbitrary detail");
            Assert.Single(substantive.Details);
            Assert.Contains("arbitrary detail", substantive.Details);
        }
        
        [Theory]
        [MemberData(nameof(Types))]
        public void DetailedDescriptionTest(Substantive.Types type)
        {
            var substantive = new Substantive(type, "Test", gender: "male",article: "a ",pronoun: "he", pronounPossessive: "his",
                description: "a test substantive", mass: 36, volume: 64);
            var answer = substantive.GenerateDetailedDescription();
            Assert.True(answer.Contains("This is a  Test.") || answer.Contains("This is Test."));
        }
    }
}