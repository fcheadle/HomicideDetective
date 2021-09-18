using System;
using System.Collections.Generic;
using System.Linq;
using HomicideDetective.People;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        private void GenerateTimeline()
        {
            if (Victim == null || Murderer == null || !Witnesses.Any())
                throw new Exception("Attempted to generate a timeline before we have a murderer, victim, or witnesses.");

            var children = new List<Personhood>();
            var highSchool = new List<Personhood>();
            var college = new List<Personhood>();
            var foodService = new List<Personhood>();
            var serviceWorkers = new List<Personhood>();
            var officeWorkers = new List<Personhood>();
            var retailWorkers = new List<Personhood>();
            var laborers = new List<Personhood>();
            var legal = new List<Personhood>();
            var emergency = new List<Personhood>();
            var spiritual = new List<Personhood>();
            
            foreach (var entity in Witnesses)
            {
                var witness = entity.GoRogueComponents.GetFirst<Personhood>();
                
                //children / caretakers
                if(witness.AgeCategory < AgeCategory.HighSchoolFreshmen || witness.Occupation == Occupations.ChildCaretaker)
                    children.Add(witness);
                
                //high school and high school teachers
                else if(witness.AgeCategory < AgeCategory.YoungAdult || witness.Occupation == Occupations.Teacher)
                    highSchool.Add(witness);
                
                //college
                else if(witness.Occupation is >= Occupations.CollegeStudent and <= Occupations.Professor)
                    college.Add(witness);
                
                //food
                else if(witness.Occupation is >= Occupations.FoodWorker and <= Occupations.Janitor)
                    foodService.Add(witness);
                
                // service
                else if(witness.Occupation == Occupations.Mechanic)
                     serviceWorkers.Add(witness);
                
                //retail and sales
                else if(witness.Occupation is >= Occupations.RetailWorker and <= Occupations.Salesman)
                    retailWorkers.Add(witness);
                
                //office
                else if(witness.Occupation == Occupations.OfficeWorker)
                    officeWorkers.Add(witness);
                
                //emergency services
                else if(witness.Occupation is >= Occupations.Nurse and <= Occupations.Doctor)
                    emergency.Add(witness);
                
                //legal
                else if(witness.Occupation == Occupations.Lawyer)
                    legal.Add(witness);
                
                //laborers
                else if (witness.Occupation == Occupations.FactoryLaborer)
                    laborers.Add(witness);

                //unemployed
                else
                    spiritual.Add(witness);
            }

            int year = 1970, month = 07, day = 02;
            GenerateDay(year, month, day, 8, 30, 360, children, ChildsDayQueue(), "school");
            GenerateDay(year, month, day, 8, 30, 360, highSchool, HighSchoolQueue(), "high school");
            GenerateDay(year, month, day, 8, 30, 360, college, CollegeQueue(), "college");
            GenerateDay(year, month, day, 8, 30, 360, foodService, FoodServiceQueue(), "the restaurant");
            GenerateDay(year, month, day, 8, 30, 360, serviceWorkers, ServiceWorkersQueue(), "at work");
            GenerateDay(year, month, day, 8, 30, 360, emergency, EmergencyServicesQueue(), "all over this town");
            GenerateDay(year, month, day, 8, 30, 360, legal, LegalQueue(), "law firm");
            GenerateDay(year, month, day, 8, 30, 360, laborers, LaborQueue(), "the factory");
            GenerateDay(year, month, day, 8, 30, 360, spiritual, SpiritualQueue(), "at my office");
            GenerateDay(year, month, day, 8, 30, 360, officeWorkers, OfficeWorkQueue(), "the office");
            GenerateDay(year, month, day, 8, 30, 360, retailWorkers, RetailWorkerQueue(), "the store");
            // GenerateActiveDayOff();
            // GenerateLazyDay
        }

        private Queue<string> HighSchoolQueue()
        {
            var queue = new Queue<string>();
            queue.Enqueue("I got up and caught the bus");
            queue.Enqueue("I listened to the marching band practice for a few minutes before class");
            queue.Enqueue("First class started. The teacher has a boring voice");
            queue.Enqueue("Second class. I always hated this subject");
            queue.Enqueue("Third period is always the slowest because it's right before lunch");
            queue.Enqueue("At lunchtime, they tried to serve us soggy pizza");
            queue.Enqueue("Afternoon labs are very hit-or-miss. Today's lab was uninteresting");
            queue.Enqueue("The bell rang to let us out of school and I walked to my friend's house");
            queue.Enqueue("I came home shortly after school let out");
            return queue;
        }

        private Queue<string> CollegeQueue()
        {
            var queue = new Queue<string>();
            queue.Enqueue("I woke up late to my first class so I decided not to go");
            queue.Enqueue("My second class was weird, I just kept thinking about how hungover I was");
            queue.Enqueue("I have a long period between classes around lunchtime so I had a beer");
            queue.Enqueue("The girl who sits in front of me in biology takes the best notes, so I always copy her");
            queue.Enqueue("Advanced Maths in the afternoon is absolutely brutal");
            queue.Enqueue("My final class of the day is literature, which I find super easy");
            queue.Enqueue("After my last class got out, I went to my friend's dorm to get high");
            queue.Enqueue("I spent a few hours listening to music with my friend");
            queue.Enqueue("When the sun started to set, my friend and I went out to the bars");
            queue.Enqueue("My friend made out with a stranger at the bar");
            queue.Enqueue("I must have drank too much because I don't remember leaving the bar");
            return queue;
        }

        private Queue<string> FoodServiceQueue()
        {
            var queue = new Queue<string>();
            queue.Enqueue("I left home to catch my shift");
            queue.Enqueue("The shift before left a very disorderly kitchen");
            queue.Enqueue("After scrubbing the kitchen thoroughly, I discovered we are out of lettuce");
            queue.Enqueue("It was a very quiet morning after the breakfast rush died down");
            queue.Enqueue("A customer spilled soda all over me, so I changed clothes at work");
            queue.Enqueue("A friend of the owner came in and was incredibly rude");
            queue.Enqueue("A table of twelve people came in. Three of them were small children");
            queue.Enqueue("I saw two young couples on a double date. They don't seem like they're having fun");
            queue.Enqueue("A man in a dark baseball cap came in alone and ordered hash browns. I found it odd");
            queue.Enqueue("Ew, I am completely covered in fryer exhaust");
            queue.Enqueue("At ten minutes til close, we stopped taking new orders");
            queue.Enqueue("Someone came and knocked on the door after we locked it. I didn't get a good look at their face");
            queue.Enqueue("A black sedan sped out of the parking lot when I finally got off shift");
            queue.Enqueue("I spent a good five minutes washing off in the bathroom before heading home");
            return queue;
        }

        private Queue<string> ServiceWorkersQueue()
        {
            var queue = new Queue<string>();
            queue.Enqueue("I got up at the crack of dawn to go to work");
            //todo
            return queue;
        }

        private Queue<string> EmergencyServicesQueue()
        {
            var queue = new Queue<string>();
            queue.Enqueue("I got up at the crack of dawn to go to work");
            //todo
            return queue;
        }

        private Queue<string> LegalQueue()
        {
            var queue = new Queue<string>();
            queue.Enqueue("I got up at the crack of dawn to go to work");
            //todo
            return queue;
        }

        private Queue<string> LaborQueue()
        {
            var queue = new Queue<string>();
            queue.Enqueue("I got up at the crack of dawn to go to work");
            queue.Enqueue("I arrived at work, thankful for the nice weather");
            queue.Enqueue("The factory takes no time at all to smell like sweat, steel, and sawdust");
            queue.Enqueue("It was a normal and uneventful day at the factory");
            queue.Enqueue("Some Big-Wigs walked through at 11, and the bossman wanted us working extra hard while they did");
            queue.Enqueue("We broke for union mandated lunch");
            queue.Enqueue("Some of the boys and I snuck off to have a beer over lunch");
            queue.Enqueue("After lunch, someone hit some merchandise with a forklift");
            queue.Enqueue("We had to clear the factory floor to deal with a spill");
            queue.Enqueue("The last several hours of the day, I worked my ass off so I could come home on time");
            queue.Enqueue("Finally, the steam whistle let us all off the clock");
            return queue;
        }

        private Queue<string> SpiritualQueue()
        {
            var queue = new Queue<string>();
            queue.Enqueue("I woke up and thanked the creator for giving me this day");
            queue.Enqueue("I left home to arrive at my chapel");
            queue.Enqueue("An old friend stopped by to pray");
            queue.Enqueue("I thought for a long time about how to be a better believer");
            queue.Enqueue("I could feel the spirit guiding me, but to what, I cannot say");
            queue.Enqueue("I caught a graffiti artist, and told them the lord is merciful");
            queue.Enqueue("I prayed with a stranger");
            queue.Enqueue("I heard about the victim - most tragic");
            return queue;
        }

        private Queue<string> OfficeWorkQueue()
        {
            var queue = new Queue<string>();
            queue.Enqueue("I left home to go to work");
            queue.Enqueue("I arrived at the office and started working");
            queue.Enqueue("I got the last cup of coffee");
            queue.Enqueue("I went to a corporate meeting in the big conference room");
            queue.Enqueue("I resumed working after the meeting finished");
            queue.Enqueue("Some work friends and I got lunch nearby");
            queue.Enqueue("Someone made a feaux pas");
            queue.Enqueue("I arrived back at the office from lunch");
            queue.Enqueue("I went to a team meeting in the little conference room");
            queue.Enqueue("After the meeting finished, I read the newspaper for a bit");
            queue.Enqueue("I focused on the problem before me");
            queue.Enqueue("I put my nose back on the grindstone");
            queue.Enqueue("I got on a conference call with headquarters");
            queue.Enqueue("I am basically not getting anything else done today");
            queue.Enqueue("The boss just put a huge pile of work on my desk");
            queue.Enqueue("I am finally off work");
            return queue;
        }

        private Queue<string> RetailWorkerQueue() 
        {
            var queue = new Queue<string>();
            queue.Enqueue("I left home to go to work");
            queue.Enqueue("I arrived at the shop and started my shift");
            queue.Enqueue("The register stopped working so I made change by hand");
            queue.Enqueue("I flirted with an attractive customer");
            queue.Enqueue("An unattended child knocked over a display");
            queue.Enqueue("Someone made a feaux pas");
            queue.Enqueue("I can't believe how good this sale is, I'm saving some merchandise for myself");
            queue.Enqueue("Hello! How can I help you today? Hello! How can I help you today? Hello! How can I help you today");
            queue.Enqueue("Haha, a very old customer bought clothing in the young folk's style");
            queue.Enqueue("Someone trashed the men's bathroom");
            queue.Enqueue("I was annoyed with how slow the last little bit of work was going");
            queue.Enqueue("One customer is milling about several minutes after we closed");
            queue.Enqueue("Finally, we locked the doors and turned off the 'open' sign");
            queue.Enqueue("I finished cleaning the shop and went home");
            return queue;
        }

        private Queue<string> ChildsDayQueue()
        {
            var courseOfEvents = new Queue<string>();
            courseOfEvents.Enqueue("I left home to go to school");
            courseOfEvents.Enqueue("I arrived at school");
            courseOfEvents.Enqueue("Someone made a feaux pas");
            courseOfEvents.Enqueue("The teacher made me laugh");
            courseOfEvents.Enqueue("A schoolmate made me laugh");
            courseOfEvents.Enqueue("We learned about butterflies");
            courseOfEvents.Enqueue("We learned how to divide");
            courseOfEvents.Enqueue("We learned how to tie our shoes");
            courseOfEvents.Enqueue("I ran myself to the point of exhaustion at recess");
            courseOfEvents.Enqueue("We broke for lunch. It wasn't very good");
            courseOfEvents.Enqueue("The teacher read us a story about dinosaurs");
            courseOfEvents.Enqueue("The bell rang and I got on the bus");
            courseOfEvents.Enqueue("I got off the bus at home");
            courseOfEvents.Enqueue("Schools out! I rode the bike with my friends");
            return courseOfEvents;
        }
        private void GenerateDay(int year, int month, int day, int startingHour, int startingMinute, int durationMinutes, List<Personhood> thosePresent, Queue<string> courseOfEvents, string location)
        {
            int minutesElapsed = 0;
            var startDate = new DateTime(year, month, day, startingHour, startingMinute, 0, 0);
            var memories = new List<Memory>();

            var who = new List<string>();
            foreach(var witness in thosePresent)
                who.Add(witness.Name);
            
            
            while (courseOfEvents.Any())
            {
                if (Random.Next() % 7 == 0)
                {
                    var happening = courseOfEvents.Dequeue();
                    memories.Add(new Memory(startDate + TimeSpan.FromMinutes(minutesElapsed),happening, location, who));
                } 
                
                minutesElapsed += 6;
            }
            
            foreach(var witness in thosePresent)
                AddTimeline(witness, memories);
        }

        private void AddTimeline(Personhood witness, List<Memory> events)
        {
            var thoughts = witness.Memories;
            thoughts.Think(events);
        }
    }
}