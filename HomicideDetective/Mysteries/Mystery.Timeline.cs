using System;
using System.Linq;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        public void GenerateTimeline()
        {
            if (Victim == null || Murderer == null || !Witnesses.Any())
                throw new Exception("Attempted to generate a timeline before we have a murderer, victim, or witnesses.");
            
            Time timeOfDeath = new Time(Random.Next(0, 23), Random.Next(0, 59));
            Time startOfTimeLine = new Time(0, 0);
            AddSleepSpeech(timeOfDeath);
            
            Time startOfDay = new Time(6, 30);
            AddWakeUpSpeech(timeOfDeath);

            Time schoolWork = new Time(8, 15);
            AddSchoolWorkSpeech(timeOfDeath);

            Time afterSchoolWork = new Time(15, 55);
            AddAfterSchoolWorkSpeech(timeOfDeath);

            Time evening = new Time(19, 00);
        }

        private void AddAfterSchoolWorkSpeech(Time afterSchoolWork)
        {
            throw new System.NotImplementedException();
        }

        private void AddSchoolWorkSpeech(Time startOfDay)
        {
            throw new System.NotImplementedException();
        }

        private void AddWakeUpSpeech(Time startOfDay)
        {
            throw new System.NotImplementedException();
        }

        private void AddSleepSpeech(Time at)
        {
            throw new System.NotImplementedException();
        }
    }
}