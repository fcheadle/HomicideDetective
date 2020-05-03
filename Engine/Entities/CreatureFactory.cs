using Engine.Components;
using Engine.Components.Creature;
using Engine.Components.UI;
using Engine.Extensions;
using Engine.Maps;
using Engine.UI;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System;

namespace Engine.Entities
{
    public static class CreatureFactory
    {
        public static BasicEntity Person(Coord position)
        {
            BasicEntity critter = new BasicEntity(Color.Gray, Color.Black, 2, position, 3, true, true);
            critter.AddGoRogueComponent(new HealthComponent());
            critter.AddGoRogueComponent(new ActorComponent());
            return critter;

        }
        internal static BasicEntity Player(Coord coord)
        {
            BasicEntity critter = new BasicEntity(Color.White, Color.Black, 1, coord, Calculate.EnumIndexFromValue(MapLayer.Player), true, true);

            critter.AddGoRogueComponent(new HealthComponent());
            critter.AddGoRogueComponent(new DisplayStatsComponent(new Coord(Settings.GameWidth - 24, Settings.GameHeight - 24)));
            critter.AddGoRogueComponent(new ActorComponent());
            critter.AddGoRogueComponent(new KeyboardComponent());

            return critter;
        }

        public static BasicEntity Animal(Coord position)
        {
            BasicEntity critter = new BasicEntity(Color.Gray, Color.Black, 224, position, 3, true, true);
            critter.AddGoRogueComponent(new HealthComponent());
            return critter;
        }
    }
}
