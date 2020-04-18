using Engine.Components;
using Engine.UI;
using Engine.Utils;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;
using System.IO;

namespace Engine.Creatures
{
    internal enum BloodTypes
    {
        O,
        A,
        B,
        AB
    }

    internal class Creature : BasicEntity, IActor
    {
        protected int baseVisibilityDistance = 25;
        protected int baseLightSourceDistance = 20;
        public string Title = "Creature";
        public string Description = "A creature of some sort. Everything from rats to people to bears.";

        public Items.Inventory Inventory = new Items.Inventory();
        protected SpeechConsole Dialogue;
        public readonly Font Voice;
        public int SystoleBloodPressure { get => _systoleBloodPressure; set => _systoleBloodPressure = value; }
        public int DiastoleBloodPressure { get => _diastoleBloodPressure; set => _diastoleBloodPressure = value; }
        public int Pulse { get => _pulse; set => _pulse = value; }
        public int BloodVolume { get => _bloodVolume; set => _bloodVolume = value; }
        public double BodyTemperature { get => _bodyTemperature; set => _bodyTemperature = value; }
        public int Mass { get => _mass; set => _mass = value; }
        public int Volume { get => _volume; set => _volume = value; }

        private int _awareness;
        private int _systoleBloodPressure;
        private int _diastoleBloodPressure;
        private int _pulse;
        private int _bloodVolume;
        private double _bodyTemperature;
        private int _mass;
        private int _volume;

        //protected Region currentRegion;


        protected Creature(Coord position, Color foreground, int glyph)
            : base(foreground, Color.Black, glyph, position, 1, isWalkable: false, isTransparent: true)
        {
            ComponentsUpdate.Add(new ThoughtProcess());
            //List<string> fonts = new List<string>();
            //foreach(string file in Directory.GetFiles("fonts"))
            //{
            //    if(file.EndsWith(".font"))
            //        fonts.Add(file);
            //}
            //if (fonts.Count == 0)
            //    Voice = Global.FontDefault;
            //else
            //{
            //    string font = fonts.RandomItem();
            //    Voice = Global.LoadFont(font).GetFont(Font.FontSizes.One);
            //}
            //Dialogue = new SpeechConsole(Voice, "this is my test string", position);
        }

        public static Creature Person(Coord position)
        {
            Color color;
            int chance = Calculate.Chance();
            if (chance < 30)
                color = Colors.MutateBy(Color.Brown, Color.Black);
            else if (chance < 60)
                color = Color.DarkKhaki;
            else if (chance < 80)
                color = Color.Khaki;
            else if (chance < 90)
                color = Color.Tan;
            else if (chance < 99)
                color = Colors.MutateBy(Color.Tan, Color.White);
            else
                color = Color.White;
            Creature critter = new Creature(position, color, 1);

            //default healthy stats
            critter.SystoleBloodPressure = 120;
            critter.DiastoleBloodPressure = 80;
            critter.Pulse = 85; //bpm
            critter.BloodVolume = 5000; //milliliters
            critter.BodyTemperature = 98.6;
            return critter;

        }
        public static Creature Animal(Coord position)
        {
            Creature critter = new Creature(position, Color.Gray, 224);

            //default is dog i guess?
            critter.SystoleBloodPressure = 120;
            critter.DiastoleBloodPressure = 80;
            critter.Pulse = 85; //bpm
            critter.BloodVolume = 5000; //milliliters
            critter.BodyTemperature = 102.5;
            return critter;
        }

        public void Act()
        {
            throw new System.NotImplementedException();
        }
    }
}
