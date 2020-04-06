using GoRogue;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Settings
    {
        public static int MapWidth { get; set; } = 320;
        public static int MapHeight { get; set; } = 320;
        public static int GameWidth { get; set; } = 120;
        public static int GameHeight { get; set; } = 36;
        public static bool IsPaused { get; set; } = false;
        public static Random Random { get; set; } = new Random();
        public static Radius FOVRadius { get; set; } = Radius.CIRCLE;
        public static SadConsole.FontMaster FontMaster { get; set; }
        public static SadConsole.Font Font { get; set; }

        public static readonly Dictionary<Keys, Direction> MovementKeyBindings = new Dictionary<Keys, Direction>
        {
            { Keys.NumPad7, Direction.UP_LEFT },    { Keys.NumPad8, Direction.UP },     { Keys.NumPad9, Direction.UP_RIGHT },
            { Keys.NumPad4, Direction.LEFT },                                           { Keys.NumPad6, Direction.RIGHT },
            { Keys.NumPad1, Direction.DOWN_LEFT },  { Keys.NumPad2, Direction.DOWN },   { Keys.NumPad3, Direction.DOWN_RIGHT },
            { Keys.Up, Direction.UP }, { Keys.Down, Direction.DOWN }, { Keys.Left, Direction.LEFT }, { Keys.Right, Direction.RIGHT }
        };

        //public static readonly Dictionary<Keys, > KeyBindings = new Dictionary<Keys, > { };
    }
}
