using GoRogue;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using System;
using System.Collections.Generic;

namespace Engine.Utilities
{
    public interface ISettings
    {
        public bool ShowingMenu { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public int GameWidth { get; set; }
        public int GameHeight { get; set; }
        public bool IsPaused { get; set; }
        public Random Random { get; set; }
        public Radius FOVRadius { get; set; }
        public FontMaster FontMaster { get; set; }
        public Font Font { get; set; }
        public Dictionary<Keys, Direction> MovementKeyBindings { get; }
        public Dictionary<GameActions, Keys> KeyBindings { get; }

        public void TogglePause();
        public void ToggleMenu();
    }
}
