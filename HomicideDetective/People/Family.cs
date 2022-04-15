using System.Collections.Generic;
using System.Linq;

namespace HomicideDetective.People
{
    public class Family
    {
        public string Surname { get; }
        public List<Personhood?> Elderly = new();
        public List<Personhood?> Adults = new();
        public List<Personhood?> Children = new();
        public IEnumerable<Personhood?> Members => Elderly.Concat(Adults).Concat(Children);
        public IEnumerable<Personhood?> AdultMembers => Elderly.Concat(Adults);
        public Family(string surname)
        {
            Surname = surname;
        }
    }
}