using System.Collections.Generic;
using System.Linq;

namespace HomicideDetective.Mysteries
{
    /// <summary>
    /// The Timeline of events over the course of a single day.
    /// </summary>
    public class Timeline : List<Happening>
    {
        public string DoingAtTime(Time time)
        {
            if (this.Any(h => h.OccurredAt.ToInt() == time.ToInt()))
                return this.First(h => h.OccurredAt.ToInt() == time.ToInt()).Occurrence;
            
            return this.Where(x => x.OccurredAt.LessThan(time)).OrderBy(x => -x.OccurredAt.ToInt()).First().Occurrence;

        }
    }
}