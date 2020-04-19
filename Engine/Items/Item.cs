using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Items
{
    internal class Item : BasicEntity
    {
        internal string Title;

        internal Item(Coord position, string name, int layer) : base(Color.White, Color.Transparent, name[0], position, layer, true, true)
        {
            Title = name;
        }

        internal (ColoredString title, ColoredString attributes) GetColoredString()
        {
            StringBuilder modifierString = new StringBuilder(20);

            modifierString.Append(Describe());
            return (Title.CreateColored(), ColoredString.Parse(modifierString.ToString()));
        }

        internal string Describe()
        {
            return "This item is 1 cm^3 and weighs one gram.";
        }
    }
}
