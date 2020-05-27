using GoRogue;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using System;
using System.Collections.Generic;

namespace Engine
{
    public interface ISettings
    {
        public static bool ShowingMenu { get; private set; }
        internal static int MapWidth { get; set; }
        internal static int MapHeight { get; set; }
        internal static int GameWidth { get; set; }
        internal static int GameHeight { get; set; }
        internal static bool IsPaused { get; private set; }
        internal static Random Random { get; set; }
        internal static Radius FOVRadius { get; set; }
        internal static FontMaster FontMaster { get; set; }
        internal static Font Font { get; set; }
        internal static Dictionary<Keys, Direction> MovementKeyBindings { get; }
        internal static Dictionary<Keys, GameActions> KeyBindings { get; }
    }
}
