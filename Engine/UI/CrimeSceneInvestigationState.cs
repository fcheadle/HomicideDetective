using Engine.Maps;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using Engine.Entities;
using Engine.Components.UI;
using Engine.Components.Creature;
using System;
using Engine.Components.Terrain;
using System.Collections.Generic;
using Engine.Maps.Areas;
using Console = SadConsole.Console;

namespace Engine.UI
{
    public class CrimeSceneInvestigationState : GameState
    {
        TimeSpan _elapsed = default;
        TimeSpan _windCounter = default;
        TimeSpan _windInterval = TimeSpan.FromMilliseconds(75);
        Func<int, int, TimeSpan, bool> fxyt = Calculate.RandomFunction4d();
        public TownMap TownMap { get; }
        public ScrollingConsole MapRenderer { get; }
        public DisplayComponent<ThoughtsComponent> Thoughts { get => Player.GetGoRogueComponent<DisplayComponent<ThoughtsComponent>>(); }
        public DisplayComponent<HealthComponent> Health { get => Player.GetGoRogueComponent<DisplayComponent<HealthComponent>>(); }
        public BasicEntity Player { get; }
        public ActorComponent Actor { get => Player.GetGoRogueComponent<ActorComponent>(); }
        public CrimeSceneInvestigationState(int mapWidth, int mapHeight, int viewportWidth, int viewportHeight)
        {
            TownMap = new TownMap(mapWidth, mapHeight);
            Player = CreatureFactory.Player(new Coord(15, 15));
            TownMap.ControlledGameObject = Player;
            TownMap.AddEntity(TownMap.ControlledGameObject);
            MapRenderer = TownMap.CreateRenderer(new Microsoft.Xna.Framework.Rectangle(0, 0, viewportWidth - 25, viewportHeight), Global.FontDefault);

            Children.Add(MapRenderer);
            Children.Add(Thoughts.Display);
            Children.Add(Health.Display);
            
            TownMap.ControlledGameObject.IsFocused = true; 
            TownMap.ControlledGameObject.Moved += Player_Moved;
            TownMap.ControlledGameObjectChanged += ControlledGameObjectChanged;

            TownMap.CalculateFOV(Actor.Position, Actor.FOVRadius);
            MapRenderer.CenterViewPortOnPoint(TownMap.ControlledGameObject.Position);
            Map = TownMap;
        }

        public void BlowWind(TimeSpan t)
        {
            _elapsed += t;
            _windCounter += t;
            if (Convert.ToInt32(_elapsed.TotalMilliseconds) % (_windInterval.TotalMilliseconds * 100) <= 1)
            {
                fxyt = Calculate.RandomFunction4d();
                _windInterval = TimeSpan.FromMilliseconds(Calculate.Percent() * 3 + 50);
            }
            if (_windCounter > _windInterval)
            {
                _windCounter = default;
                for (int x = 0; x < TownMap.Width; x++)
                {
                    for (int y = 0; y < TownMap.Height; y++)
                    {
                        BasicTerrain terrain = TownMap.GetTerrain<BasicTerrain>(new Coord(x, y));
                        if (terrain != null)
                        {
                            if (terrain.HasComponent<BlowsInWindComponent>())
                            {
                                BlowsInWindComponent c = terrain.GetComponent<BlowsInWindComponent>();
                                if (c != null)
                                {
                                    if (fxyt(x, y, _elapsed))
                                    {
                                        c.Blow();
                                    }
                                    c.Update(t);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }
        void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            TownMap.CalculateFOV(TownMap.ControlledGameObject.Position, Actor.FOVRadius, Settings.FOVRadius);
            List<string> output = new List<string>();
            output.Add("At coordinate " + Player.Position.X + ", " + Player.Position.Y);
            foreach (Area area in GetRegions(Player.Position))
            {
                output.Add(area.ToString());
                output.Add(area.Orientation.ToString());
            }
            Thoughts.Print(output.ToArray());
            Health.Print(Health.Component.Details());
            MapRenderer.CenterViewPortOnPoint(TownMap.ControlledGameObject.Position);
        }

        private IEnumerable<Area> GetRegions(Coord position)
        {
            foreach (Area area in TownMap.Regions)
            {
                if (area.InnerPoints.Contains(position))
                    yield return area;
            }
        }

        public override void OnEnter()
        {
            throw new NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new NotImplementedException();
        }

        public override void OnExit()
        {
            throw new NotImplementedException();
        }
    }
}