using System;
using HomicideDetective.Happenings;
using HomicideDetective.People;
using SadRogue.Integration;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class SpeechTests
    {
        private RogueLikeEntity SetUpEntity()
        {
            var rle = new RogueLikeEntity((0, 0), 1);
            var subs = new Substantive(Substantive.Types.Person, "Test", gender: "male",article: "",pronoun: "he", pronounPossessive: "his",
                description: "a test substantive", mass: 36, volume: 64);
            var thoughts = new Thoughts();
            thoughts.Think(new Happening(new DateTime(1900,1,1), "Christ is born", "A manger in Bethleham", false));
            thoughts.Think(new Happening(DateTime.Now - TimeSpan.FromMinutes(300), "I started working", "home office", false));
            thoughts.Think(new Happening(DateTime.Now - TimeSpan.FromMinutes(30), "The Rapture began", "London, U.K.", false));
            thoughts.Think(new Happening(DateTime.Now, "I've got to figure out how to survive!", "London, U.K.", false));
            
            var speech = new Speech();
            
            // speech.AddTruth("true-saying");
            // speech.AddLie("lie-saying");
            // speech.AddCommonKnowledge("common");
            
            speech.AddTruthFacialExpression("true-face");
            speech.AddLieFacialExpression("lie-face");
            speech.AddCommonFacialExpression("common-face");
            
            speech.AddTruthPosture("true-posture");
            speech.AddLiePosture("lie-posture");
            speech.AddCommonPosture("common-posture");

            speech.AddTruthTone("true-tone");
            speech.AddLieTone("lie-tone");
            speech.AddCommonTone("common-tone");

            rle.AllComponents.Add(subs);
            rle.AllComponents.Add(thoughts);
            rle.AllComponents.Add(speech);
            return rle;
        }
        
        [Fact]
        public void NewSpeechComponentTest()
        {
            var speech = new Speech();
            Assert.Contains("Their voice is ", speech.Description);
        }

        [Fact]
        public void SpeakToTest()
        {
            var entity = SetUpEntity();

            for (int i = 0; i < 15; i++)
                Assert.DoesNotContain("lie", entity.AllComponents.GetFirst<Speech>().SpeakTo(false));
        }

        [Fact]
        public void GreetTest()
        {
            var entity = SetUpEntity();
            var greeting = entity.AllComponents.GetFirst<Speech>().Greet();
            Assert.True(greeting.Contains("Hello") || greeting.Contains("Hi") || 
                        greeting.Contains("hello") || greeting.Contains("hi"));
        }
        
        [Fact]
        public void IntroduceTest()
        {
            var entity = SetUpEntity();
            var introduction = entity.AllComponents.GetFirst<Speech>().Introduce();
            Assert.Contains("My name is", introduction);
        }
        [Fact]
        public void InquireAboutSelfTest()
        {
            var entity = SetUpEntity();
            var introduction = entity.AllComponents.GetFirst<Speech>().InquireAboutSelf();
            Assert.Contains(entity.Info().Description, introduction);
        }
        [Fact]
        public void InquireWhereaboutsTest()
        {
            var entity = SetUpEntity();
            var inquiry = entity.AllComponents.GetFirst<Speech>().InquireWhereabouts(DateTime.Now);
            Assert.Contains("I was at home", inquiry);
        }
        [Fact]
        public void InquireAboutCompanyTest()
        {
            var entity = SetUpEntity();
            var inquiry = entity.AllComponents.GetFirst<Speech>().InquireAboutCompany(DateTime.Now);
            Assert.Contains("I was with no one", inquiry);
        }
        [Fact]
        public void InquireAboutHappeningTest()
        {
            var entity = SetUpEntity();
            var inquiry = entity.AllComponents.GetFirst<Speech>().InquireAboutHappening(DateTime.Now);
            Assert.Contains("At", inquiry);        
            Assert.Contains("I fell asleep", inquiry);        
        }
        // [Fact]
        // public void InquireAboutPersonTest()
        // {
        //     //path: doesn't know the person we're asking about
        //     //path: has heard of the person we're talking about
        //     //path: knows the person we're asking about
        //     throw new NotImplementedException();
        // }
        // [Fact]
        // public void InquireAboutPlaceTest()
        // {
        //     //path: doesn't know of the place
        //     //path: has heard of the place
        //     //path: has been to the place
        //     throw new NotImplementedException();
        // }
        //
        // [Fact]
        // public void InquireAboutThingTest()
        // {
        //     //path: doesn't know of the thing
        //     //path: has heard of the thing
        //     //path: has seen the thing
        //     throw new NotImplementedException();
        // }
    }
}