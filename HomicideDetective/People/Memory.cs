using System;
using System.Collections.Generic;
using System.Linq;
using HomicideDetective.UserInterface;

namespace HomicideDetective.People
{
    /// <summary>
    /// A happening is a thing that happened at a time.
    /// </summary>
    /// <remarks>Use pretty sentences, please.</remarks>
    public class Memory : IPrintable
    {
        public List<string> Who { get; set; }
        public string What { get; set; }
        public DateTime When { get; set; }
        public string Where { get; set; }
        public bool Private { get; set; }

        public Memory(DateTime when, string what, string where, List<string> who, bool isPrivate)
        {
            What = what;
            Where = where;
            Private = isPrivate;
            Who = who;
            When = when;
        }
        
        public Memory(DateTime when, string what, string where, bool isPrivate)
        {
            What = what;
            Where = where;
            Private = isPrivate;
            Who = new List<string>();
            When = when;
        }

        public string GetPrintableString()
        {
            string answer = $"At {When}, {What}. ";
            if (Who.Any())
            {
                foreach (var person in Who)
                    answer += $"{person}, ";

                answer += "were there.";
            }

            return answer;
        }
        
    }
}