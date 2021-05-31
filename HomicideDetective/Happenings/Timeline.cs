using System;
using System.Collections.Generic;
using System.Linq;
using HomicideDetective.Mysteries;

namespace HomicideDetective.Happenings
{
    /// <summary>
    /// The Timeline of events over the course of a single day.
    /// </summary>
    public class Timeline : List<Happening>
    {
        public string OccurrenceAtTime(DateTime time) => HappeningAtTime(time).What;

        public Happening MostRecent() => this.OrderBy(h => h.When).First();

        public Happening HappeningAtTime(DateTime time)
        {
            if (this.Any(h => h.When == time))
                return this.First(h => h.When == time);
            
            return this.Where(x => x.When < time).OrderBy(x => x.When).Last();
        }

        public IEnumerable<string> Occurences
        {
            get
            {
                foreach (var happening in this)
                {
                    yield return happening.What;
                }
            }
        }
    }
}