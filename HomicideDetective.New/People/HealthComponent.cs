using System;
using System.Collections.Generic;
using System.Linq;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;

namespace HomicideDetective.New.People
{
    public enum BloodTypes
    {
        O,
        A,
        B,
        AB
    }
    public class HealthComponent : RogueLikeComponentBase, IHaveDetails
    {
        public string Name { get; }
        public string Description { get; }
        public double SystoleBloodPressure { get; private set; }
        public double DiastoleBloodPressure { get; private set; }
        public double Pulse { get => _heartBeatsPerMinute; set => _heartBeatsPerMinute = value; }
        public double BreathRate { get => _breathsPerMinute; set => _breathsPerMinute = value; }
        public double BloodVolume { get; private set; }
        public double NormalBodyTemperature { get; private set; }
        public double CurrentBodyTemperature { get; private set; } //celsius
        public double LungCapacity { get; private set; } //cm^3
        public double CurrentBreathVolume { get; private set; } //cm^3
        public double TypicalBloodVolume { get; set; } //in ml
        private int _timeUnitsElapsed;
        private double _heartBeatsPerMinute;
        private double _breathsPerMinute;
        private double _heartBeatStatus;
        private double _halfBreathVolume;

        public HealthComponent(float systoleBloodPressure = 120, float diastoleBloodPressure = 80, float pulse = 85, float bodyTemperature = 96.7f, float lungCapacity = 1000, float bloodVolume = 6000)
            : base(true,false,false, false)
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

        private void Breathe(float ms)
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
            int y = (int)Math.Round(_heartBeatStatus);
            return (x, y);
        }
        private void BeatHeart(int timeUnits)
        {
            //a graph that stays really close to 0 until we get close to zero, then it pulses up and down real quick-like
            //period goes from -15 to 15
            timeUnits = timeUnits % 15;
            timeUnits -= 15;
            timeUnits = timeUnits == 0 ? 1 : timeUnits;
            double period = _heartBeatsPerMinute / 60 * timeUnits;
            _heartBeatStatus = 2 * Math.Sin(Math.Sin(10 / period));
        }


        public string[] GetDetails()
        {
            string[] message = {
                "Temp: " + CurrentBodyTemperature,
                "Pulse: " + Pulse + "bpm",
                "Breath: " + CurrentBreathVolume + "/" + LungCapacity,
                "Heart BP: " + _heartBeatStatus
            };

            return message.Concat(HeartMonitorStrings()).ToArray();
        }

        private string[] HeartMonitorStrings()
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
            _timeUnitsElapsed++;
            BeatHeart(_timeUnitsElapsed);
            Breathe(_timeUnitsElapsed);
        }
    }
}
