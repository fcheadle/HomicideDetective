using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadConsole.Components;
using SadConsole.Components.GoRogue;
using System;

namespace Engine.Components.Creature
{
    public enum BloodType
    {
        O,
        A,
        B,
        AB
    }
    public class HealthComponent : ComponentBase
    {
        public int SystoleBloodPressure { get; private set; }
        public int DiastoleBloodPressure { get; private set; }
        public int Pulse { get => _heartBeatsPerMinute; set => _heartBeatsPerMinute = value; }
        public int BreathRate { get => _breathsPerMinute; set => _breathsPerMinute = value; }
        public int BloodVolume { get; private set; }
        public double NormalBodyTemperature { get; private set; }
        public double CurrentBodyTemperature { get; private set; }
        public int MaximumBreathVolume { get; private set; }
        public int CurrentBreathVolume { get; private set; }

        private int _heartBeatsPerMinute;
        private int _timeSinceLastHeartbeat = 0;
        private int _breathsPerMinute;
        private int _halfBreathVolume;

        public HealthComponent(int systoleBloodPressure = 120, int diastoleBloodPressure = 80, int pulse = 85, int bloodVolume = 5000, double bodyTemperature = 96.7)
            : base(isUpdate: false, isKeyboard: false, isDraw: false, isMouse: false)
        {
            SystoleBloodPressure = systoleBloodPressure;
            DiastoleBloodPressure = diastoleBloodPressure;
            Pulse = pulse;
            BloodVolume = bloodVolume;
            NormalBodyTemperature = bodyTemperature;
            CurrentBodyTemperature = NormalBodyTemperature;
            _halfBreathVolume = MaximumBreathVolume + 1;
            _halfBreathVolume /= 2;
            _breathsPerMinute = 14;
        }

        public override void ProcessGameFrame()
        {
        }
        public override void Update(SadConsole.Console console, TimeSpan delta)
        {
            BeatHeart();
            Breathe(delta);
        }

        private void Breathe(TimeSpan delta)
        {
            double ratio = Math.Sin(delta.TotalSeconds * TimeSpan.TicksPerSecond); //from -1 to 1
            //from 1 to MaxBreathVolume

        }

        private void BeatHeart()
        {
            _timeSinceLastHeartbeat++;
            if (1 / _heartBeatsPerMinute < _timeSinceLastHeartbeat)
            {
                _timeSinceLastHeartbeat = 0;
            }
        }

        public override string ToString()
        {
            //this person is the picture of health...
            return base.ToString();
        }

        internal string[] Details()
        {
            string[] message =
            {
                    "Current Body Temp is " + CurrentBodyTemperature + ", while normal is " + NormalBodyTemperature,
                    "Blood Pressure: " + SystoleBloodPressure + "/" + DiastoleBloodPressure,
                    "Pulse: " + Pulse + "bpm",
                    "Breathing " + BreathRate + " times per minute",
                    "Blood: "+ BloodVolume +"ml",
                    "Lungs have "+ CurrentBreathVolume + ", and their capacity is " + MaximumBreathVolume
            };

            return message;
        }
    }
}
