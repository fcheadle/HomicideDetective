using System;
using System.Collections.Generic;
using System.Linq;

namespace HomicideDetective.Happenings
{
    public class CourseOfEvents
    {
        private Dictionary<TimeSpan, string> Events { get; } = new();

        public Timeline ToTimeline(DateTime start)
        {
            Timeline tl = new();
            var events = Events.OrderBy(e => e.Key);
            foreach (var evnt in events)
                tl.Add(new Happening(start + evnt.Key, evnt.Value, "", false));

            return tl;
        }

        public void AddDetailsToSubstantive(Substantive subs)
        {
            foreach (var evnt in Events.OrderBy(e => e.Key))
            {
                subs.AddDetail(evnt.Value);
            }
        }
    }
}