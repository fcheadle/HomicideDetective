using System;
using System.Collections.Generic;
using HomicideDetective.Mysteries;
using SadRogue.Primitives;
using SadRogue.Integration.Components;

namespace HomicideDetective.People
{
    public class Health : RogueLikeComponentBase, IDetailed
    {
        public enum BloodTypes { O, A, B, AB }

        public BloodTypes BloodType { get; }
        public string Name { get; }
        public string Description { get; set; }
        public bool Alive { get; private set; } = true;
        public double SystoleBloodPressure { get; private set; }
        public double DiastoleBloodPressure { get; private set; }

        public double Pulse
        {
            get => _heartBeatsPerMinute;
            set => _heartBeatsPerMinute = value;
        }

        public double BreathRate
        {
            get => _breathsPerMinute;
            set => _breathsPerMinute = value;
        }

        public double BloodVolume { get; private set; }
        public double NormalBodyTemperature { get; private set; }
        public double CurrentBodyTemperature { get; private set; } //celsius
        public double LungCapacity { get; private set; } //cm^3
        public double CurrentBreathVolume { get; private set; } //cm^3
        public double TypicalBloodVolume { get; set; } //in ml
        public double CurrentHeartStatus => _heartBeatStatus;
        private int _timeUnitsElapsed;
        private double _heartBeatsPerMinute;
        private double _breathsPerMinute;
        private double _heartBeatStatus;
        private double _halfBreathVolume;

        public Health(float systoleBloodPressure = 120, float diastoleBloodPressure = 80, float pulse = 85,
            float bodyTemperature = 96.7f, float lungCapacity = 1000, float bloodVolume = 6000)
            : base(true, false, false, false)
        {
            Name = "Health";
            Description = "I track the health of an Entity.";
            SystoleBloodPressure = systoleBloodPressure;
            DiastoleBloodPressure = diastoleBloodPressure;
            Pulse = pulse;
            NormalBodyTemperature = bodyTemperature;
            CurrentBodyTemperature = NormalBodyTemperature;
            BloodVolume = bloodVolume;
            TypicalBloodVolume = bloodVolume;
            LungCapacity = lungCapacity;
            _halfBreathVolume = LungCapacity + 1;
            _halfBreathVolume /= 2;
            _breathsPerMinute = 14;
            CurrentBreathVolume = _halfBreathVolume;
        }

        public void Breathe(double ms)
        {
            //period is _breathsPerMinute

            double period = _breathsPerMinute / 60;

            double delta = ms * period;
            double ratio = Math.Sin(delta); //from -1 to 1
            CurrentBreathVolume = Math.Round(_halfBreathVolume * ratio + _halfBreathVolume, 1);
        }

        public Point MonitorHeart()
        {
            //x position is between 0-23
            int x = _timeUnitsElapsed % 24;
            int y = (int) Math.Round(_heartBeatStatus);
            return (x, y);
        }

        public void BeatHeart(int timeUnits)
        {
            //a graph that stays really close to 0 until we get close to zero, then it pulses up and down real quick-like
            //period goes from -15 to 15
            timeUnits %= 15;
            timeUnits -= 15;
            timeUnits = timeUnits == 0 ? 1 : timeUnits;
            double period = _heartBeatsPerMinute / 60 * timeUnits;
            _heartBeatStatus = 2 * Math.Sin(Math.Sin(10 / period));
        }

        private string BodyTempString => $"Temp: {CurrentBodyTemperature}";
        private string PulseString => $"Pulse: {Pulse}bpm";
        private string BloodPressureString => $"Blood Pressure: {SystoleBloodPressure}/{DiastoleBloodPressure}";
        private string BloodTypeString => $"Blood Type: {BloodType}";
        private string BreathString => $"Blood Pressure: {CurrentBreathVolume}/{LungCapacity}";

        public List<string> Details 
            => new List<string>(){BodyTempString, PulseString, BloodPressureString, BloodTypeString, BreathString};

        public string[] HeartMonitorStrings()
        {
            var beep = MonitorHeart();
            List<string> answer = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string line = "";
                for (int j = 0; j < 30; j++)
                {
                    if (beep.X == j && beep.Y == i - 2)
                        line += "*";
                    else
                        line += "_";
                }
                answer.Add(line);
            }

            return answer.ToArray();
        }

        public void ProcessTimeUnit()
        {
            if (Alive)
            {
                // _timeUnitsElapsed++;
                // BeatHeart(_timeUnitsElapsed);
                // Breathe(_timeUnitsElapsed);
            }
        }

        public void Murder() => Alive = false;

        public void Murder(Substantive murderer, Substantive murderWeapon, Substantive sceneOfTheCrime)
        {
            Murder();
            Description = $" Murdered by {murderer.Name} with a {murderWeapon.Name}, at {sceneOfTheCrime.Name}.";
        }
    }
}
