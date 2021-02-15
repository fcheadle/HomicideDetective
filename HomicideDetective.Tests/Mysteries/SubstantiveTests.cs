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
            var substantive = new Substantive(type, "Test", 0, "male","","he", "his",
                "a test substantive", 36, 64, "smol", "lite");
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
            var substantive = new Substantive(type, "Test", 0, "male","","he", "his",
                "a test substantive", 36, 64, "smol", "lite");
            var answer = substantive.Details;
            Assert.Equal(5, answer.Length);
            Assert.Contains("Test", answer);
            Assert.Contains("a test substantive", answer);
            Assert.Contains("Mass(g): 36", answer);
            Assert.Contains("Volume(ml): 64", answer);
        }
        
        [Theory]
        [MemberData(nameof(Types))]
        public void DetailedDescriptionTest(Substantive.Types type)
        {
            var substantive = new Substantive(type, "Test", 0, "male","a ","he", "his",
                "a test substantive", 36, 64, "smol", "lite");
            var answer = substantive.GenerateDetailedDescription();
            Assert.True(answer.Contains("This is a Test.") || answer.Contains("This is Test."));
            Assert.Contains("It smol, and it lite", answer);
        }


        [Theory]
        [MemberData(nameof(Types))]
        public void NewFromSeedTest(Substantive.Types type)
        {
            var substantive = new Substantive(type, 33);
            Assert.NotNull(substantive.Random);
            Assert.Equal(type, substantive.Type);
        }
    }
}