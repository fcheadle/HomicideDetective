using Engine.Utilities;
using GoRogue;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using System;
using System.Collections.Generic;

namespace Engine
{
    public class Settings// : ISettings
    {
        public GameMode Mode { get; set; } = GameMode.RealTimeWithPause;
        public int MapWidth { get; set; } = 360;
        public int MapHeight { get; set; } = 360;
        public int GameWidth { get; set; } = 120;
        public int GameHeight { get; set; } = 40;
        public bool IsPaused { get; set; } = false;
        public bool ShowingMenu { get; set; } = false;
        public Random Random { get; set; } = new Random();
        public Radius FovRadius { get; set; } = Radius.CIRCLE;
        public Distance DistanceType { get; set; }  = Distance.MANHATTAN;
        public double FovDistance { get; set; } = 20.00;
        //public FontMaster FontMaster { get; set; } = Global.;
        public Font Font { get; set; } = Global.FontDefault;
        public Dictionary<Keys, Direction> MovementKeyBindings { get; } = new Dictionary<Keys, Direction>
        {
            { Keys.NumPad7, Direction.UP_LEFT },    { Keys.NumPad8, Direction.UP },     { Keys.NumPad9, Direction.UP_RIGHT },
            { Keys.NumPad4, Direction.LEFT },                                           { Keys.NumPad6, Direction.RIGHT },
            { Keys.NumPad1, Direction.DOWN_LEFT },  { Keys.NumPad2, Direction.DOWN },   { Keys.NumPad3, Direction.DOWN_RIGHT },
            { Keys.Up, Direction.UP }, { Keys.Down, Direction.DOWN }, { Keys.Left, Direction.LEFT }, { Keys.Right, Direction.RIGHT },
            { Keys.W, Direction.UP }, { Keys.S, Direction.DOWN }, { Keys.A, Direction.LEFT }, { Keys.D, Direction.RIGHT }
        };

        public Dictionary<Keys, GameAction> KeyBindings { get; } = new Dictionary<Keys, GameAction>
        {
            {Keys.Tab, GameAction.RefocusOnPlayer},                           
            {Keys.Space, GameAction.TogglePause},
            {Keys.Escape, GameAction.ToggleMenu },
            {Keys.P, GameAction.TakePhotograph},
            {Keys.L, GameAction.LookAtEverythingInSquare},
            {Keys.D, GameAction.DustItemForPrints},
            {Keys.G, GameAction.GetItem},
            {Keys.R, GameAction.RemoveItemFromInventory},
            {Keys.T, GameAction.Talk},
            {Keys.I, GameAction.ToggleInventory},
            {Keys.N, GameAction.ToggleNotes},
            {Keys.A, GameAction.LookAtPerson},
            {Keys.Left, GameAction.MoveLeft },
            {Keys.Right, GameAction.MoveRight },
            {Keys.Up, GameAction.MoveUp },
            {Keys.Down, GameAction.MoveDown },
        };

        public Dictionary<GameAction, Keys> ActionsBindings()
        {
            Dictionary<GameAction, Keys> bindings = new Dictionary<GameAction, Keys>();

            foreach(GameAction action in Enum.GetValues(typeof(GameAction)))
            {
                foreach(KeyValuePair<Keys, GameAction> keybind in KeyBindings)
                {
                    if(keybind.Value == action)
                    {
                        bindings.Add(action, keybind.Key);
                    }
                }
            }

            return bindings;
        }
    }
}
