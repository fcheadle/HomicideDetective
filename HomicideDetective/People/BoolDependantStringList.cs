using System.Collections.Generic;
using System.Linq;
using GoRogue;

namespace HomicideDetective.People
{
    public class BoolDependantStringList
    {
        public List<string> Truths { get; set; }
        public List<string> Common { get; set; }
        public List<string> Lies { get; set; }
        public string Current { get; private set; } = "";

        public BoolDependantStringList()
        {
            Truths = new List<string>();
            Common = new List<string>();
            Lies = new List<string>();
        }

        public string GetNextString(bool truth)
        {
            List<string> answer;
            if (truth)
                answer = Truths.Concat(Common).ToList();
            else
                answer = Lies.Concat(Common).ToList();

            Current = answer.RandomItem();
            return Current;
        }
    }
}