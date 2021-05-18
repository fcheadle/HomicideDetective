using System;
using System.Linq;
using HomicideDetective.People;
using SadRogue.Integration;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        private void GenerateTimeline()
        {
            if (Victim == null || Murderer == null || !Witnesses.Any())
                throw new Exception("Attempted to generate a timeline before we have a murderer, victim, or witnesses.");

            var schoolDay = GenerateSchoolDay();
            var workDay = GenerateOfficeWorkDay();
            var shiftWork = GenerateRetailWorkDay();
            var activeDayOff = GenerateActiveDayOff();
            var lazyDayOff = GenerateLazyDayOff();
            foreach (var witness in Witnesses)
            {
                switch (Random.Next(1, 6))
                {
                    default:
                    case 1: AddTimeline(witness, schoolDay); break;
                    case 2: AddTimeline(witness, workDay); break;
                    case 3: AddTimeline(witness, shiftWork); break;
                    case 4: AddTimeline(witness, activeDayOff); break;
                    case 5: AddTimeline(witness, lazyDayOff); break;
                }
            }
        }

        private void AddTimeline(RogueLikeEntity witness, Timeline events)
        {
            var thoughts = witness.AllComponents.GetFirst<Thoughts>();
            var speech = witness.AllComponents.GetFirst<Speech>();
            thoughts.Think(events);
            
            speech.TruthSayings.AddRange(events.Occurences);
        }

        private Timeline GenerateSchoolDay()
        {
            Timeline schoolDay = new Timeline();
            schoolDay.Add(new Happening(new Time(08,30), "I left for school. "));
            schoolDay.Add(new Happening(new Time(08,47), "I arrived at school. "));
            schoolDay.Add(new Happening(new Time(08,55), "My first class started. "));
            schoolDay.Add(new Happening(new Time(09,20), "My second class started. "));
            schoolDay.Add(new Happening(new Time(10,45), "My third class started. "));
            schoolDay.Add(new Happening(new Time(12,30), "We broke for lunch. Soggy Pizza and chocolate milk. Again. "));
            schoolDay.Add(new Happening(new Time(12,55), "My fourth class started. "));
            schoolDay.Add(new Happening(new Time(14,20), "My last class of the day started. "));
            schoolDay.Add(new Happening(new Time(15,45), "School's out! I'm riding bikes with my friends. "));
            return schoolDay;
        }

        private Timeline GenerateOfficeWorkDay()
        {
            Timeline workDay = new Timeline();
            workDay.Add(new Happening(new Time(06,45), "I left home to go to work. "));
            workDay.Add(new Happening(new Time(07,21), "I arrived at the office and started working. "));
            workDay.Add(new Happening(new Time(09,00), "I went to a corporate meeting in the big conference room. "));
            workDay.Add(new Happening(new Time(10,30), "I resumed working after the meeting finished. "));
            workDay.Add(new Happening(new Time(11,45), "Some work friends and I got lunch nearby. "));
            workDay.Add(new Happening(new Time(12,29), "We arrived back at the office from lunch. "));
            workDay.Add(new Happening(new Time(12,55), "I went to a team meeting in the little conference room. "));
            workDay.Add(new Happening(new Time(14,01), "After the meeting finished, I read the newspaper for a bit before I got back to work. "));
            workDay.Add(new Happening(new Time(14,13), "I put my nose back on the grindstone. "));
            workDay.Add(new Happening(new Time(15,50), "I got on a conference call with headquarters. "));
            workDay.Add(new Happening(new Time(16,37), "I am basically not getting anything else done today. "));
            workDay.Add(new Happening(new Time(16,54), "The boss just put a huge pile of work on my desk! "));
            workDay.Add(new Happening(new Time(18,04), "I am finally off work. "));
            return workDay;
        }

        private Timeline GenerateRetailWorkDay()
        {
            Timeline workDay = new Timeline();
            workDay.Add(new Happening(new Time(16,10), "I left home to go to work. "));
            workDay.Add(new Happening(new Time(16,27), "I arrived at the shop and started my shift. "));
            workDay.Add(new Happening(new Time(16,59), "I flirted with an attractive customer. "));
            workDay.Add(new Happening(new Time(18,12), "An unattended child knocked over a display. "));
            workDay.Add(new Happening(new Time(18,46), "Haha, a very old customer bought clothing in the young folk's style. "));
            workDay.Add(new Happening(new Time(19,09), "Someone trashed the men's bathroom. "));
            workDay.Add(new Happening(new Time(19,25), "I was annoyed with how slow the last 30 minutes of work were going. "));
            workDay.Add(new Happening(new Time(19,55), "Finally, we locked the doors and turned off the 'open' sign. "));
            workDay.Add(new Happening(new Time(20,22), "I finished cleaning the shop and went home. "));
            return workDay;
        }

        private Timeline GenerateActiveDayOff()
        {
            Timeline dayOff = new Timeline();
            dayOff.Add(new Happening(new Time(08,10), "I got up and left for the hiking trails. "));
            dayOff.Add(new Happening(new Time(09,30), "I arrived at the trailhead and began hiking. "));
            dayOff.Add(new Happening(new Time(10,04), "I passed another hiker and said hello. "));
            dayOff.Add(new Happening(new Time(10,49), "I found a place just off the trail and took a leek. "));
            dayOff.Add(new Happening(new Time(11,13), "I found a particularly exquisite view. "));
            dayOff.Add(new Happening(new Time(11,38), "Oh! I just saw an owl! "));
            dayOff.Add(new Happening(new Time(12,41), "I arrived back and the trailhead and came home. "));
            dayOff.Add(new Happening(new Time(13,02), "I took a shower and then left to meet a friend for lunch. "));
            dayOff.Add(new Happening(new Time(09,27), "I had lunch with a friend. That was fun! "));
            dayOff.Add(new Happening(new Time(10,27), "I went to a poker game. "));
            return dayOff;
        }
        
        private Timeline GenerateLazyDayOff()
        {
            Timeline dayOff = new Timeline();
            dayOff.Add(new Happening(new Time(01,10), "I woke up. "));
            dayOff.Add(new Happening(new Time(02,27), "I ate a big breakfast. "));
            dayOff.Add(new Happening(new Time(03,27), "I got lost in a book. "));
            dayOff.Add(new Happening(new Time(04,27), "I made myself lunch. "));
            dayOff.Add(new Happening(new Time(05,27), "I got lost in a book again. "));
            dayOff.Add(new Happening(new Time(06,27), "I heard a commotion outside, so I went outside to check it out. "));
            dayOff.Add(new Happening(new Time(07,27), "I took a long, hot bath. "));
            dayOff.Add(new Happening(new Time(08,27), "I finished a novel and started another. "));
            dayOff.Add(new Happening(new Time(09,27), "I watched game shows on the telly. "));
            dayOff.Add(new Happening(new Time(10,27), "I fell asleep watching T.V. "));
            return dayOff;
        }
    }
}