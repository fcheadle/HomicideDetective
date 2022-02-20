using System.Collections.Generic;

namespace HomicideDetective.People
{
    public class Family
    {
        public string Surname { get; }
        public List<Personhood?> Elderly = new();
        public List<Personhood?> Adults = new();
        public List<Personhood?> Children = new();

        public Family(string surname)
        {
            Surname = surname;
        }
    }
}