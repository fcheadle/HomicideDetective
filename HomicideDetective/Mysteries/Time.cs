using System;
namespace HomicideDetective.Mysteries
{
    /// <summary>
    /// The hours/minutes, given in 24 hour military time.
    /// </summary>
    public class Time
    {
        private int _hours;
        public int Hours
        {
            get => _hours;
            set
            {
                if (value >= 24)
                {
                    value -= 24;
                    _hours = value;
                }
                else _hours = value;
            }
        }
        private int _minutes;

        public int Minutes
        {
            get => _minutes;
            set
            {
                if (value >= 60)
                {
                    _hours++;
                    value -= 60;
                    _minutes = value;
                }
                else _minutes = value;
            }
        }

        public Time(int hours, int minutes)
        {
            if (hours < 0 || hours >= 24)
                throw new ArgumentException($"Hours must be between 00 and 23 (was {hours})");
            if (minutes < 0 || minutes >= 60)
                throw new ArgumentException($"Minutes must be between 00 and 59 (was {minutes})");

            _hours = hours;
            _minutes = minutes;
        }

        public override string ToString() => $"{ToInt()} hundred hours";
        public int ToInt() => Hours * 100 + Minutes; 
        public bool LessThan(Time other) => ToInt() < other.ToInt();
        public bool GreaterThan(Time other) => ToInt() > other.ToInt();
        public bool Equals(Time other) => ToInt() == other.ToInt();

    }
}