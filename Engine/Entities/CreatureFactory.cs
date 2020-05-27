using Engine.Components;
using Engine.Components.Creature;
using Engine.Components.UI;
using Engine.Maps;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Engine.Entities
{
    public static class CreatureFactory
    {
        public static BasicEntity Person(Coord position)
        {
            BasicEntity critter = new BasicEntity(Color.Gray, Color.Black, 2, position, 3, true, true);
            critter.AddGoRogueComponent(new HealthComponent());
            critter.AddGoRogueComponent(new ThoughtsComponent());
            critter.AddGoRogueComponent(new ActorComponent());
            return critter;

        }
        internal static BasicEntity Player(Coord coord)
        {
            BasicEntity critter = new BasicEntity(Color.White, Color.Black, 1, coord, Calculate.EnumIndexFromValue(MapLayer.Player), true, true);

            critter.AddGoRogueComponent(new HealthComponent());
            critter.AddGoRogueComponent(new DisplayComponent<HealthComponent>(new Coord(Settings.GameWidth - 24, Settings.GameHeight - 24)));
            critter.AddGoRogueComponent(new DisplayComponent<ThoughtsComponent>(new Coord(Settings.GameWidth - 24, 0)));
            critter.AddGoRogueComponent(new HealthComponent());
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
