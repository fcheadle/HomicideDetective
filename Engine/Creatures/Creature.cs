using Engine.Components;
using Engine.UI;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;
using System.IO;

namespace Engine.Creatures
{
    internal class Creature : BasicEntity
    {
        protected int baseVisibilityDistance = 25;
        protected int baseLightSourceDistance = 20;
        public string Title = "Creature";
        public string Description = "A creature of some sort. Everything from rats to people to bears.";

        public Items.Inventory Inventory = new Items.Inventory();
        protected SpeechConsole Dialogue;
        public readonly Font Voice;

        //protected Region currentRegion;


        protected Creature(Coord position, Color foreground, int glyph)
            : base(foreground, Color.Black, glyph, position, 1, isWalkable: false, isTransparent: true)
        {
            ComponentsUpdate.Add(new ThoughtProcess());
            List<string> fonts = new List<string>();

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

        public static Creature Person(Coord position) => new Creature(position, Color.White, 1);
        public static Creature Animal(Coord position) => new Creature(position, Color.Gray, 224);
    }
}
