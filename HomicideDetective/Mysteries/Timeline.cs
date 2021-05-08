namespace HomicideDetective.Mysteries
{
    public class Timeline
    {
        private int _day = -1;
        private int _hours;
        public int Hours
        {
            get => _hours;
            set
            {
                if (value >= 24)
                {
                    _day++;
                    value -= 24;
                    _hours = value;
                }
            }
        }
        private int _minutes;

        public int Minutes
        {
            get => _minutes;
            set
            {
                if (value >= 24)
                {
                    _hours++;
                    value -= 24;
                    _minutes = value;
                }
            }
        }

        public int TimeOfDeath => Hours * 100 + Minutes;
        
        public Timeline(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
            GenerateTimeline();
        }
        
        private void GenerateTimeline()
        {
            while (Hours > 1 && Hours < 7)
            {
                
            }
        }
    }
}