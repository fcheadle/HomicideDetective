﻿using System.Collections.Generic;
using Engine.Maps;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;

namespace Engine.Creatures
{
    // Custom class for the player is used in this example just so we can handle input.  This could be done via a component, or in a main screen, but for simplicity we do it here.
    internal class Player : Creature
    {

        public int FOVRadius;

        public Player(Coord position)
            : base(position, Color.White, 1) => FOVRadius = 25;


        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            Direction moveDirection = Direction.NONE;
            if (info.IsKeyPressed(Keys.Space))
            {
                Settings.TogglePause();
            }
            // Simplified way to check if any key we care about is pressed and set movement direction.
            foreach (Keys key in Settings.MovementKeyBindings.Keys)
            {
                if (info.IsKeyPressed(key))
                {
                    moveDirection = Settings.MovementKeyBindings[key];
                    break;
                }
            }

            Position += moveDirection;

            if (moveDirection != Direction.NONE)
                return true;
            else
                return base.ProcessKeyboard(info);
        }

    }
}
