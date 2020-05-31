using GoRogue;
using System;
using System.Collections.Generic;

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
        public float SystoleBloodPressure { get; private set; }
        public float DiastoleBloodPressure { get; private set; }
        public float Pulse { get => _heartBeatsPerMinute; set => _heartBeatsPerMinute = value; }
        public float BreathRate { get => _breathsPerMinute; set => _breathsPerMinute = value; }
        public float BloodVolume { get; private set; }
        public float NormalBodyTemperature { get; private set; }
        public float CurrentBodyTemperature { get; private set; } //celsius
        public float LungCapacity { get; private set; } //cm^3
        public float CurrentBreathVolume { get; private set; } //cm^3
        public float TypicalBloodVolume { get; set; } //in ml
        private float _totalTime;

        private float _heartBeatsPerMinute;
        private float _breathsPerMinute;
        private float _heartBeatStatus;
        private float _halfBreathVolume;

        public HealthComponent(float systoleBloodPressure = 120, float diastoleBloodPressure = 80, float pulse = 85, float bodyTemperature = 96.7f, float lungCapacity = 1000, float bloodVolume = 6000)
            : base(isUpdate: true, isKeyboard: false, isDraw: false, isMouse: false)
        {
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

        public override void ProcessGameFrame()
        {
            BeatHeart(_totalTime);
            Breathe(_totalTime);
        }
        public override void Update(SadConsole.Console console, TimeSpan delta)
        {
            _totalTime += (float)delta.TotalMilliseconds;
            base.Update(console, delta);
        }

        private void Breathe(float ms)
        {
            //period is _breathsPerMinute
            ms /= 100;
            float period = _breathsPerMinute / 60;
            
            float delta = ms % period;
            float ratio = (float)Math.Sin(delta); //from -1 to 1
            CurrentBreathVolume = _halfBreathVolume * ratio + _halfBreathVolume;
        }

        private void BeatHeart(float ms)
        {
            //a graph that stays really close to 0 until we get close to zero, then it pulses up and down real quick-like
            //period goes from -15 to 15
            float period = _heartBeatsPerMinute / 60 * ms;
            _heartBeatStatus = (float)Math.Sin(Math.Sin(1/period));
        }

        public override string[] GetDetails()
        {
            string[] message = {
                "Body Temp: " + CurrentBodyTemperature,
                "Blood Pressure: " + SystoleBloodPressure + "/" + DiastoleBloodPressure,
                "Pulse: " + Pulse + "bpm"
            };


            return message;
        }
    }
}
