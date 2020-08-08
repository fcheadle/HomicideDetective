﻿using Engine;
using Engine.Components;
using Engine.Components.UI;
using Engine.Creatures;
using Engine.Creatures.Components;
using Engine.UI.Components;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Tests
{
    internal class MockCreatureFactory : ICreatureFactory
    {
        public EntityBase Person(Coord position)
        {
            EntityBase critter = new EntityBase(Color.White, Color.Black, 2, position, 3, true, true);
            AllCreaturesComponents(critter);
            PersonComponents(critter);
            return critter;
        }
        public EntityBase Player(Coord position)
        {
            EntityBase critter = new EntityBase(Color.White, Color.Black, 1, position, 3, true, true);
            AllCreaturesComponents(critter);
            PersonComponents(critter);
            PlayerComponents(critter);
            return critter;
        }
        public EntityBase Animal(Coord position)
        {
            EntityBase critter = new EntityBase(Color.Gray, Color.Black, 224, position, 3, true, true);
            AllCreaturesComponents(critter);
            return critter;
        }
        private void AllCreaturesComponents(EntityBase critter)
        {
            critter.Components.Add(new HealthComponent(critter));
        }
        private void PersonComponents(EntityBase critter)
        {
            critter.Components.Add(new ThoughtsComponent(critter));
            critter.Components.Add(new PersonalityComponent(critter));
            critter.Components.Add(new EmotionsComponent(critter));
            critter.Components.Add(new ActorComponent(critter));
            critter.Components.Add(new SpeechComponent(critter));
        }
        private void PlayerComponents(EntityBase critter)
        {
            critter.Components.Add(new PageComponent<HealthComponent>(critter, critter.Position + 3));
            critter.Components.Add(new PageComponent<ThoughtsComponent>(critter, critter.Position + 5));
            critter.Components.Add(new NotePadComponent(critter, critter.Position + 7));
            critter.Components.Add(new CSIKeyboardComponent(critter));
        }
    }
}