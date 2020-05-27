using Engine.Components.Creature;
using Engine.Components.UI;
using Engine.Entities;
using Engine.Maps;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;

namespace Engine.States
{
    public class DebuggingState : GameState
    {
        public ScrollingConsole MapRenderer { get; }
        public DisplayComponent<ThoughtsComponent> Thoughts { get => Player.GetGoRogueComponent<DisplayComponent<ThoughtsComponent>>(); }
        public DisplayComponent<HealthComponent> Health { get => Player.GetGoRogueComponent<DisplayComponent<HealthComponent>>(); }
        public BasicEntity Player { get; }
        public ActorComponent Actor { get => Player.GetGoRogueComponent<ActorComponent>(); }

        public DebuggingState()
        {
            Map = new TestMap();
            Player = CreatureFactory.Player(new Coord(15, 15));
            Map.ControlledGameObject = Player;
            Map.AddEntity(Map.ControlledGameObject);
            MapRenderer = Map.CreateRenderer(new Microsoft.Xna.Framework.Rectangle(0, 0, 80, 25), Global.FontDefault);
            Children.Add(MapRenderer);
            Children.Add(Thoughts.Display);
            Children.Add(Health.Display);

            Map.ControlledGameObject.IsFocused = true;
            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObjectChanged;

            Map.CalculateFOV(Actor.Position, Actor.FOVRadius);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }

        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            Map.CalculateFOV(Map.ControlledGameObject.Position, Actor.FOVRadius, Settings.FOVRadius);
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
