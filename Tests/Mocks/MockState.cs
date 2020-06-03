using Engine;
using Engine.Components;
using Engine.Components.Creature;
using Engine.Components.UI;
using Engine.Entities.Creatures;
using Engine.Maps;
using Engine.States;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using Game = Engine.Game;

namespace Tests.Mocks
{
    public class MockState : GameState
    {
        public ScrollingConsole MapRenderer { get; }
        public DisplayComponent<ThoughtsComponent> Thoughts { get => (DisplayComponent<ThoughtsComponent>)Player.GetComponent<DisplayComponent<ThoughtsComponent>>(); }
        public DisplayComponent<HealthComponent> Health { get => (DisplayComponent<HealthComponent>)Player.GetComponent<DisplayComponent<HealthComponent>>(); }
        public BasicEntity Player { get; }
        public ActorComponent Actor { get => (ActorComponent)Player.GetComponent<ActorComponent>(); }
        private CreatureFactory creatureFactory = new CreatureFactory();
        public MockState()
        {
            Map = new MockMap();
            Player = creatureFactory.Player(new Coord(15, 15));
            Map.ControlledGameObject = Player;
            Map.AddEntity(Map.ControlledGameObject);
            MapRenderer = Map.CreateRenderer(new Microsoft.Xna.Framework.Rectangle(0, 0, 80, 25), Global.FontDefault);
            Children.Add(MapRenderer);
            Children.Add(Thoughts.Page);
            Children.Add(Health.Page);

            Map.ControlledGameObject.IsFocused = true;
            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObjectChanged;

            Map.CalculateFOV(Actor.Position, Actor.FOVRadius);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
            Components.Add(new WeatherComponent(Map));
        }

        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }

        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            Map.CalculateFOV(Map.ControlledGameObject.Position, Actor.FOVRadius, Program.Settings.FOVRadius);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
        }

        public override void OnExit()
        {
        }
    }
}
