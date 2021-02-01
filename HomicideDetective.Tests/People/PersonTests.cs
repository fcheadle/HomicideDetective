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
        private int _weight = 14400;
        private int _size = 6250;

        [Fact]
        public void NewPersonTest()
        {
            var billy = new Person((1,1), _firstname, _lastname, _description, _weight, _size, _sizeDescription, _weightDescription);
            Assert.Equal($"{_firstname} {_lastname}", billy.Name);
            Assert.Equal(_size, billy.Substantive.Volume);
            Assert.Equal(_weight, billy.Substantive.Mass);
        }
        
        
        [Fact]
        public void GetDetailedDescriptionTest()
        {
            var billy = new Person((1,1), _firstname, _lastname, _description, _weight, _size, _sizeDescription, _weightDescription);
            var answer = billy.Substantive.GenerateDetailedDescription();
            Assert.Contains(_sizeDescription, answer);
            Assert.Contains(_weightDescription, answer);
        }

        [Fact]
        public void DetailsTest()
        {
            var billy = new Person((1,1), _firstname, _lastname, _description, _weight, _size, _sizeDescription, _weightDescription);
            var answer = billy.GetDetails();
            Assert.Contains(billy.Name, answer);
            Assert.Contains(billy.Substantive.Description, answer);
            Assert.Contains($"Mass(g): {_weight}", answer);
            Assert.Contains($"Volume(ml): {_size}", answer);
            Assert.Contains(billy.Substantive.GenerateDetailedDescription(), answer);
        }
    }
}