using Engine.Components.Creature;
using Engine.UI;
using GoRogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components.UI
{
    internal class DisplayStatsComponent : ComponentBase
    {
        internal MessageConsole Console;
        internal HealthComponent Health => Parent.GetComponent<HealthComponent>();
        internal PhysicalComponent PhysicalStats => Parent.GetComponent<PhysicalComponent>();
        public DisplayStatsComponent(Coord position) : base(true, false, true, false)
        {
            Console = new MessageConsole(24, 24);
            Console.Position = position;
            Console.IsVisible = true;
        }

        public override void ProcessGameFrame()
        {

        }

        public override void Draw(SadConsole.Console console, TimeSpan delta)
        {
            //base.Draw(console, delta);
        }

        internal void Print()
        {
            string[] message =
            {
                "Current Body Temp is " + Health.CurrentBodyTemperature + ", while normal is " + Health.NormalBodyTemperature,
                "Blood Pressure: " + Health.SystoleBloodPressure + "/" + Health.DiastoleBloodPressure,
                "Pulse: " + Health.Pulse + "bpm",
                "Breathing " + Health.BreathRate + " times per minute",
                "Blood: "+ Health.BloodVolume +"ml",
                "Lungs have "+ Health.CurrentBreathVolume + ", and their capacity is " + Health.MaximumBreathVolume
            };


            Console.Print(message);
        }
    }
}
