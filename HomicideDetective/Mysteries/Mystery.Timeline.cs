namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        public void GenerateTimeline()
        {
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

        //     private int _day = -1;
    //     private int _hours;
    //     public int Hours
    //     {
    //         get => _hours;
    //         set
    //         {
    //             if (value >= 24)
    //             {
    //                 _day++;
    //                 value -= 24;
    //                 _hours = value;
    //             }
    //         }
    //     }
    //     private int _minutes;
    //
    //     public int Minutes
    //     {
    //         get => _minutes;
    //         set
    //         {
    //             if (value >= 24)
    //             {
    //                 _hours++;
    //                 value -= 24;
    //                 _minutes = value;
    //             }
    //         }
    //     }
    //
    //     public int TimeOfDeath => Hours * 100 + Minutes;
    //     
    //     public Timeline(int hours, int minutes)
    //     {
    //         Hours = hours;
    //         Minutes = minutes;
    //         GenerateTimeline();
    //     }
    //     
    //     private void GenerateTimeline()
    //     {
    //         while (Hours > 1 && Hours < 7)
    //         {
    //             
    //         }
    //     }
    }
}