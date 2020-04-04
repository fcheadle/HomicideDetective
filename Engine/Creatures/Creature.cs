using System.Collections.Generic;
using Engine.Interface;
using GoRogue;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Maps;
using SadConsole.Tiles;

namespace Engine.Creatures
{
    internal abstract class Creature : BasicEntity
    {
        protected int baseVisibilityDistance = 25;
        protected int baseLightSourceDistance = 20;
        public string Title = "Creature";
        public string Description = "A creature of some sort. Everything from rats to people to bears.";

        public Items.Inventory Inventory = new Items.Inventory();
        protected SpeechConsole Dialogue;

        //protected Region currentRegion;


        protected Creature(Coord position, Color foreground, int glyph)
            : base(foreground, Color.Black, glyph, position, 1, isWalkable: false, isTransparent: true)
        {
            
        }
    }
}
