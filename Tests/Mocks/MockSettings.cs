using Engine.Utilities;
using GoRogue;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using System;
using System.Collections.Generic;

namespace Tests
{
    internal class MockSettings : ISettings
    {
        public bool ShowingMenu { get; set; } = false;
        public int MapWidth { get; set; } = 100;
        public int MapHeight { get; set; } = 100;
        public int GameWidth { get; set; } = 100;
        public int GameHeight { get; set; } = 100;
        public bool IsPaused { get; set; } = false;
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

        public Dictionary<GameActions, Keys> KeyBindings { get; } = new Dictionary<GameActions, Keys>
        {
            {GameActions.RefocusOnPlayer,           Keys.Tab},
            {GameActions.TogglePause,               Keys.Space },
            {GameActions.ToggleMenu,                Keys.Escape},
            {GameActions.TakePhotograph,            Keys.P },
            {GameActions.LookAtEverythingInSquare,  Keys.L },
            {GameActions.DustItemForPrints,         Keys.D },
            {GameActions.GetItem,                   Keys.G },
            {GameActions.RemoveItemFromInventory,   Keys.R },
            {GameActions.Talk,                      Keys.T },
            {GameActions.ToggleInventory,           Keys.I },
            {GameActions.ToggleNotes,               Keys.N },
            {GameActions.LookAtPerson,              Keys.A },
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