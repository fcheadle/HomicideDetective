using Engine.Components;
using Engine.Utilities.Mathematics;
using GoRogue;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Creatures.Components
{
    public enum BloodType
    {
        O,
        A,
        B,
        AB
    }
    public class HealthComponent : Component
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
        private int _timeUnitsElapsed;
        private float _heartBeatsPerMinute;
        private float _breathsPerMinute;
        private float _heartBeatStatus;
        private float _halfBreathVolume;

        public HealthComponent(BasicEntity parent, float systoleBloodPressure = 120, float diastoleBloodPressure = 80, float pulse = 85, float bodyTemperature = 96.7f, float lungCapacity = 1000, float bloodVolume = 6000)
            : base(isUpdate: true, isKeyboard: false, isDraw: false, isMouse: false)
        {
            Parent = parent;
            Name = "Health";
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

        public override void Update(SadConsole.Console console, TimeSpan delta)
        {
            base.Update(console, delta);
        }

        private void Breathe(float ms)
        {
            //period is _breathsPerMinute

            float period = _breathsPerMinute / 60;

            float delta = ms * period;
            float ratio = (float)Math.Sin(delta); //from -1 to 1
            CurrentBreathVolume = (float)Math.Round(_halfBreathVolume * ratio + _halfBreathVolume, 1);
        }

        public Coord MonitorHeart()
        {
            //x position is between 0-23
            int x = _timeUnitsElapsed % 24;
            int y = (int)Math.Round(_heartBeatStatus);
            return new Coord(x, y);
        }
        private void BeatHeart(int timeUnits)
        {
            //a graph that stays really close to 0 until we get close to zero, then it pulses up and down real quick-like
            //period goes from -15 to 15
            timeUnits = timeUnits % 15;
            timeUnits -= 15;
            timeUnits = timeUnits == 0 ? 1 : timeUnits;
            float period = _heartBeatsPerMinute / 60 * timeUnits;
            _heartBeatStatus = Formulae.HeartBeat(period);
        }

        public override string[] GetDetails()
        {
            string[] message = {
                "Temp: " + CurrentBodyTemperature,
                "Pulse: " + Pulse + "bpm",
                "Breath: " + CurrentBreathVolume + "/" + LungCapacity,
                "Heart BP: " + _heartBeatStatus
            };

            return message.Concat(HeartMonitotStrings()).ToArray();
        }

        private string[] HeartMonitotStrings()
        {
            Coord beep = MonitorHeart();
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

        public override void ProcessTimeUnit()
        {
            _timeUnitsElapsed++;
            BeatHeart(_timeUnitsElapsed);
            Breathe(_timeUnitsElapsed);
        }
    }
}
