// using HomicideDetective.Mysteries;
// using HomicideDetective.People;
// using Xunit;
//
// namespace HomicideDetective.Tests.People
// {
//     public class PersonTests
//     {
//         private string _firstname = "Billy";
//         private string _lastname = "Smith";
//         private string _description = "A growing boy who likes baseball and apple pie";
//         private string _weightDescription = "just a touch rotund";
//         private string _sizeDescription = "are just big boned";
//         private int _weight = 100500;
//         private int _size = 75250;
//
//         [Fact]
//         public void NewPersonTest()
//         {
//             var subs = new Substantive(Substantive.Types.Person, $"{_firstname} {_lastname}", 16, "male",
//                 description: _description, weightDescription: _weightDescription, sizeDescription: _sizeDescription,
//                 mass: _weight, volume: _size);
//             var billy = new Person((1,1), subs);
//             Assert.Equal($"{_firstname} {_lastname}", billy.Name);
//             Assert.Equal(_size, billy.Substantive.Volume);
//             Assert.Equal(_weight, billy.Substantive.Mass);
//         }
//     }
// }