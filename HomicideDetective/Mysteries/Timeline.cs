using System.Collections.Generic;
using System.Linq;

namespace HomicideDetective.Mysteries
{
    /// <summary>
    /// The Timeline of events over the course of a single day.
    /// </summary>
    public class Timeline : List<Happening>
    {
        public string OccurrenceAtTime(Time time) => HappeningAtTime(time).Occurrence;

        public Happening MostRecent() => this.OrderBy(h => h.OccurredAt.ToInt()).First();

        public Happening HappeningAtTime(Time time)
        {            
            if (this.Any(h => h.OccurredAt.ToInt() == time.ToInt()))
                return this.First(h => h.OccurredAt.ToInt() == time.ToInt());
            
            return this.Where(x => x.OccurredAt.LessThan(time)).OrderBy(x => -x.OccurredAt.ToInt()).First();
        }

        public IEnumerable<string> Occurences
        {
            get
            {
                foreach (var happening in this)
                {
                    yield return happening.Occurrence;
                }
            }
        }
    }
}