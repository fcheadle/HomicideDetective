using System;
using System.Linq;
using HomicideDetective.Happenings;
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
            schoolDay.Add(new Happening(new DateTime(1970, 07,4, 08,30, 0), "I left for school. ", "office", false));
            schoolDay.Add(new Happening(new DateTime(1970, 07,4, 08,47, 0), "I arrived at school. ", "office", false));
            schoolDay.Add(new Happening(new DateTime(1970, 07,4, 08,55, 0), "My first class started. ", "office", false));
            schoolDay.Add(new Happening(new DateTime(1970, 07,4, 09,20, 0), "My second class started. ", "office", false));
            schoolDay.Add(new Happening(new DateTime(1970, 07,4, 10,45, 0), "My third class started. ", "office", false));
            schoolDay.Add(new Happening(new DateTime(1970, 07,4, 12,30, 0), "We broke for lunch. Soggy Pizza and chocolate milk. Again. ", "office", false));
            schoolDay.Add(new Happening(new DateTime(1970, 07,4, 12,55, 0), "My fourth class started. ", "office", false));
            schoolDay.Add(new Happening(new DateTime(1970, 07,4, 14,20, 0), "My last class of the day started. ", "office", false));
            schoolDay.Add(new Happening(new DateTime(1970, 07,4, 15,45, 0), "School's out! I'm riding bikes with my friends. ", "office", false));
            return schoolDay;
        }

        private Timeline GenerateOfficeWorkDay()
        {
            Timeline workDay = new Timeline();
            workDay.Add(new Happening(new DateTime(1970, 07,4, 06,45,0), "I left home to go to work. ", "office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 07,21,0), "I arrived at the office and started working. ","office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 09,00,0), "I went to a corporate meeting in the big conference room. ","office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 10,30,0), "I resumed working after the meeting finished. ", "office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 11,45,0), "Some work friends and I got lunch nearby. ", "office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 12,29,0), "We arrived back at the office from lunch. ", "office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 12,55,0), "I went to a team meeting in the little conference room. ", "office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 14,01,0), "After the meeting finished, I read the newspaper for a bit before I got back to work. ","office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 14,13,0), "I put my nose back on the grindstone. ", "office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 15,50,0), "I got on a conference call with headquarters. ", "office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 16,37,0), "I am basically not getting anything else done today. ", "office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 16,54,0), "The boss just put a huge pile of work on my desk! ", "office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 18,04,0), "I am finally off work. ", "office", false));
            return workDay;
        }

        private Timeline GenerateRetailWorkDay()
        {
            Timeline workDay = new Timeline();
            workDay.Add(new Happening(new DateTime(1970, 07,4, 16,10, 0), "I left home to go to work. ","office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 16,27, 0), "I arrived at the shop and started my shift. ","office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 16,59, 0), "I flirted with an attractive customer. ","office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 18,12, 0), "An unattended child knocked over a display. ","office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 18,46, 0), "Haha, a very old customer bought clothing in the young folk's style. ","office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 19,09, 0), "Someone trashed the men's bathroom. ","office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 19,25, 0), "I was annoyed with how slow the last 30 minutes of work were going. ","office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 19,55, 0), "Finally, we locked the doors and turned off the 'open' sign. ","office", false));
            workDay.Add(new Happening(new DateTime(1970, 07,4, 20,22, 0), "I finished cleaning the shop and went home. ","office", false));
            return workDay;
        }

        private Timeline GenerateActiveDayOff()
        {
            Timeline dayOff = new Timeline();
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 08,10, 0), "I got up and left for the hiking trails. ","office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 09,30, 0), "I arrived at the trailhead and began hiking. ","office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 10,04, 0), "I passed another hiker and said hello. ","office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 10,49, 0), "I found a place just off the trail and took a leek. ","office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 11,13, 0), "I found a particularly exquisite view. ","office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 11,38, 0), "Oh! I just saw an owl! ","office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 12,41, 0), "I arrived back and the trailhead and came home. ","office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 13,02, 0), "I took a shower and then left to meet a friend for lunch. ","office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 09,27, 0), "I had lunch with a friend. That was fun! ","office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 10,27, 0), "I went to a poker game. ","office", false));
            return dayOff;
        }
        
        private Timeline GenerateLazyDayOff()
        {
            Timeline dayOff = new Timeline();
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 01,10, 0), "I woke up. ", "office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 02,27, 0), "I ate a big breakfast. ", "office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 03,27, 0), "I got lost in a book. ", "office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 04,27, 0), "I made myself lunch. ", "office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 05,27, 0), "I got lost in a book again. ", "office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 06,27, 0), "I heard a commotion outside, so I went outside to check it out. ", "office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 07,27, 0), "I took a long, hot bath. ", "office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 08,27, 0), "I finished a novel and started another. ", "office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 09,27, 0), "I watched game shows on the telly. ", "office", false));
            dayOff.Add(new Happening(new DateTime(1970, 07,4, 10,27, 0), "I fell asleep watching T.V. ", "office", false));
            return dayOff;
        }
    }
}