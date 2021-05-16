using System.Collections.Generic;
using System.Linq;

namespace HomicideDetective.Mysteries
{
    /// <summary>
    /// The Timeline of events over the course of a single day.
    /// </summary>
    public class Timeline : Dictionary<Time, string>
    {
        public string DoingAtTime(Time time)
        {
            if (ContainsKey(time))
                return base[time];

            
            var temp = this.OrderBy(x => x.Key.ToInt()).ToList();
            string answer = "";
            foreach (var activityAtTime in temp)
            {
                if (activityAtTime.Key.LessThan(time))
                {
                    answer = activityAtTime.Value;
                }
            }

            return answer;
        }
    }
}