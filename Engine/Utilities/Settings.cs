using Engine.Utilities;
using GoRogue;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using System;
using System.Collections.Generic;

namespace Engine
{
    public class Settings : ISettings
    {
        public int MapWidth { get; set; } = 360;
        public int MapHeight { get; set; } = 360;
        public int GameWidth { get; set; } = 120;
        public int GameHeight { get; set; } = 40;
        public bool IsPaused { get; set; } = false;
        public bool ShowingMenu { get; set; } = false;
        public Random Random { get; set; } = new Random();
        public Radius FOVRadius { get; set; } = Radius.CIRCLE;
        public FontMaster FontMaster { get; set; }
        public Font Font { get; set; }
        public Dictionary<Keys, Direction> MovementKeyBindings { get; } = new Dictionary<Keys, Direction>
        {
            { Keys.NumPad7, Direction.UP_LEFT },    { Keys.NumPad8, Direction.UP },     { Keys.NumPad9, Direction.UP_RIGHT },
            { Keys.NumPad4, Direction.LEFT },                                           { Keys.NumPad6, Direction.RIGHT },
            { Keys.NumPad1, Direction.DOWN_LEFT },  { Keys.NumPad2, Direction.DOWN },   { Keys.NumPad3, Direction.DOWN_RIGHT },
            { Keys.Up, Direction.UP }, { Keys.Down, Direction.DOWN }, { Keys.Left, Direction.LEFT }, { Keys.Right, Direction.RIGHT },
            { Keys.W, Direction.UP }, { Keys.S, Direction.DOWN }, { Keys.A, Direction.LEFT }, { Keys.D, Direction.RIGHT }
        };

        public Dictionary<Keys, GameActions> KeyBindings { get; } = new Dictionary<Keys, GameActions>
        {
            {Keys.Space, GameActions.TogglePause }, //not implements
            {Keys.Escape, GameActions.ToggleMenu }, //not implemented
            {Keys.P, GameActions.TakePhotograph }, //not implemented
            {Keys.L, GameActions.LookAtEverythingInSquare }, //not implemented
            {Keys.D, GameActions.DustItemForPrints }, //not implemented
            {Keys.G, GameActions.GetItem },//not implemented
            {Keys.R, GameActions.RemoveItemFromInventory },//not implemented
            {Keys.T, GameActions.Talk },//not implemented
            {Keys.I, GameActions.ToggleInventory },//not implemented
            {Keys.N, GameActions.ToggleNotes },//not implemented
            {Keys.A, GameActions.LookAtPerson },//not implemented
        };

        public void ToggleMenu()
        {
            ShowingMenu = !ShowingMenu;
            IsPaused = ShowingMenu;
        }

        public void TogglePause()
        {
            IsPaused = !IsPaused;
        }
    }
}
