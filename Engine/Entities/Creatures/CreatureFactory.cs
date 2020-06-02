using Engine.Components;
using Engine.Components.Creature;
using Engine.Components.UI;
using Engine.Maps;
using GoRogue;
using GoRogue.GameFramework;
using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;

namespace Engine.Entities.Creatures
{
    public class CreatureFactory : ICreatureFactory
    {
        public BasicEntity Person(Coord position)
        {
            BasicEntity critter = new BasicEntity(Color.White, Color.Black, 2, position, 3, true, true);
            AllCreaturesComponents(critter);
            PersonComponents(critter);
            return critter;
        }
        public BasicEntity Player(Coord position)
        {
            BasicEntity critter = new BasicEntity(Color.White, Color.Black, 1, position, 3, true, true);
            AllCreaturesComponents(critter);
            PersonComponents(critter);
            //critter.Components.Add(new DisplayComponent<HealthComponent>(critter, position + 1));
            //critter.Components.Add(new DisplayComponent<ThoughtsComponent>(critter, position + 1));
            critter.Components.Add(new KeyboardComponent(critter));
            critter.SetGlyph(0, 0, 1);
            critter.IsDirty = true;
            return critter;
        }

        public BasicEntity Animal(Coord position)
        {
            BasicEntity critter = new BasicEntity(Color.Gray, Color.Black, 224, position, 3, true, true);
            AllCreaturesComponents(critter);
            return critter;
        }
        private void AllCreaturesComponents(BasicEntity critter)
        {
            critter.Components.Add(new PhysicalComponent(critter));
            critter.Components.Add(new HealthComponent(critter));
        }
        private void PersonComponents(BasicEntity critter)
        {
            critter.Components.Add(new ThoughtsComponent(critter));
            critter.Components.Add(new PersonalityComponent(critter));
            critter.Components.Add(new EmotionsComponent(critter));
            critter.Components.Add(new ActorComponent(critter));
            critter.Components.Add(new SpeechComponent(critter));
        }
    }
}
