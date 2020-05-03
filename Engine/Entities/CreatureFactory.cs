using Engine.Components;
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
            BasicEntity critter = new BasicEntity(Color.White, Color.Black, 1, coord, Calculate.EnumIndexFromValue(MapLayers.PLAYER), true, true);

            critter.AddGoRogueComponent(new HealthComponent());
            critter.AddGoRogueComponent(new ActorComponent());
            critter.AddGoRogueComponent(new KeyboardComponent());

            return critter;
        }

        public static BasicEntity Animal(Coord position)
        {
            BasicEntity critter = new BasicEntity(Color.Gray, Color.Black, 224, position, 3, true, true);
            critter.AddGoRogueComponent(new HealthComponent() { BodyTemperature = 102.5 });
            return critter;
        }
    }
}
