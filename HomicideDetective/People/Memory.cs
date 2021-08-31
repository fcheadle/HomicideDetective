using System;
using System.Collections.Generic;
using System.Linq;

namespace HomicideDetective.People
{
    /// <summary>
    /// A single memory
    /// </summary>
    public class Memory : IPrintable
    {
        /// <summary>
        /// Who (besides the owner of the memory) is involved?
        /// </summary>
        public IEnumerable<string> Who { get; set; }
        
        /// <summary>
        /// What items were involved?
        /// </summary>
        public IEnumerable<string> What { get; set; }
        
        /// <summary>
        /// What happened?
        /// </summary>
        public string Occurrence { get; set; }
        
        /// <summary>
        /// When did it happen?
        /// </summary>
        public DateTime When { get; set; }
        
        /// <summary>
        /// Where was this memory formed?
        /// </summary>
        public string Where { get; set; }
        
        /// <summary>
        /// Is this memory secret?
        /// </summary>
        public bool Private { get; set; }

        public Memory(DateTime when, string occurrence, string where, IEnumerable<string>? who = null, IEnumerable<string>? what = null, bool isPrivate = false)
        {
            Occurrence = occurrence;
            Where = where;
            Private = isPrivate;
            Who = who ?? new List<string>();
            What = what ?? new List<string>();
            When = when;
        }

        public string GetPrintableString()
        {
            string answer = $"At {When.Hour}:{When.Minute}, {Occurrence}";
            if (Who.Count() == 2)
            {
                answer += $". {Who.First()} and {Who.Last()} ";
                
                answer += "were there";
            }

            if (Who.Count() > 2)
            {
                answer += ". ";
                var list = Who.ToList();
                for (int i = 0; i < list.Count - 2; i++)
                {
                    answer += $"{list[i]}, ";
                }

                answer += $"and {list.Last()} were there";
            }
            
            if (What.Any())
            {
                answer += ". ";
                foreach (var thing in What)
                    answer += $"{thing}, ";

                answer += "were involved";
            }

            return answer;
        }
    }
}