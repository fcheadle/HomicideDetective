using HomicideDetective.Mysteries;
using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class PersonTests
    {
        private string _firstname = "Billy";
        private string _lastname = "Smith";
        private string _description = "A growing boy who likes baseball and apple pie";
        private string _weightDescription = "just a touch rotund";
        private string _sizeDescription = "are just big boned";
        private int _weight = 100500;
        private int _size = 75250;

        [Fact]
        public void NewPersonTest()
        {
            var subs = new Substantive(Substantive.Types.Person, $"{_firstname} {_lastname}", 16, "male",
                description: _description, weightDescription: _weightDescription, sizeDescription: _sizeDescription,
                mass: _weight, volume: _size);
            var billy = new Person((1,1), subs);
            Assert.Equal($"{_firstname} {_lastname}", billy.Name);
            Assert.Equal(_size, billy.Substantive.Volume);
            Assert.Equal(_weight, billy.Substantive.Mass);
        }
        
        
        [Fact]
        public void GetDetailedDescriptionTest()
        {
            
            var subs = new Substantive(Substantive.Types.Person, $"{_firstname} {_lastname}", 16, "male",
                description: _description, weightDescription: _weightDescription, sizeDescription: _sizeDescription,
                mass: _weight, volume: _size);
            
            var billy = new Person((1,1), subs);
            var answer = billy.Substantive.GenerateDetailedDescription();
            Assert.Contains(_sizeDescription, answer);
            Assert.Contains(_weightDescription, answer);
        }

        [Fact]
        public void DetailsTest()
        {
            var subs = new Substantive(Substantive.Types.Person, $"{_firstname} {_lastname}", 16, "male",
                description: _description, weightDescription: _weightDescription, sizeDescription: _sizeDescription,
                mass: _weight, volume: _size);
            
            var billy = new Person((1,1), subs);
            var answer = billy.GetDetails();
            Assert.Contains(billy.Name, answer);
            Assert.Contains(billy.Substantive.Description, answer);
            Assert.Contains($"Mass(g): {_weight}", answer);
            Assert.Contains($"Volume(ml): {_size}", answer);
            Assert.Contains(billy.Substantive.GenerateDetailedDescription(), answer);
        }
    }
}