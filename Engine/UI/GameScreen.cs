using Engine.Maps;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using System.Linq;
using Engine.Entities;
using Engine.Extensions;
using Engine.Components.UI;
using Engine.Components.Creature;
using System;
using Engine.Components.Terrain;
using System.Collections.Generic;

namespace Engine.UI
{
    internal class GameScreen : ContainerConsole
    {
        TimeSpan _elapsed = default;
        TimeSpan _windCounter = default;
        TimeSpan _windInterval = TimeSpan.FromMilliseconds(75);
        int _xOffset;
        int _yOffset;
        Func<int, int, TimeSpan, bool> f = Calculate.RandomFunction4d();
        internal TownMap TownMap { get; }
        internal ScrollingConsole MapRenderer { get; }
        internal MessageConsole Messages { get; }
        internal DisplayStatsComponent Health { get => Player.GetGoRogueComponent<DisplayStatsComponent>(); }
        internal BasicEntity Player { get; }
        internal ActorComponent Actor { get => Player.GetGoRogueComponent<ActorComponent>(); }
        internal GameScreen(int mapWidth, int mapHeight, int viewportWidth, int viewportHeight)
        {
            TownMap = new TownMap(mapWidth, mapHeight);
            Player = CreatureFactory.Player(new Coord(15, 15));
            TownMap.ControlledGameObject = Player;
            TownMap.AddEntity(TownMap.ControlledGameObject);
            Messages = new MessageConsole(24, viewportHeight - 24);
            Messages.Position = new Coord(viewportWidth - 24, 0);

            MapRenderer = TownMap.CreateRenderer(new Microsoft.Xna.Framework.Rectangle(0, 0, viewportWidth - 25, viewportHeight), Global.FontDefault);
            Children.Add(MapRenderer);
            Children.Add(Messages);
            Children.Add(Health.Console);
            
            TownMap.ControlledGameObject.IsFocused = true; 
            TownMap.ControlledGameObject.Moved += Player_Moved;
            TownMap.ControlledGameObjectChanged += ControlledGameObjectChanged;

            TownMap.CalculateFOV(Actor.Position, Actor.FOVRadius);
            MapRenderer.CenterViewPortOnPoint(TownMap.ControlledGameObject.Position);
        }

        internal void BlowWind(TimeSpan t)
        {
            _elapsed += t;
            _windCounter += t;
            if (Convert.ToInt32(_elapsed.TotalMilliseconds) % (_windInterval.TotalMilliseconds * 100) <= 1)// && Convert.ToInt32(_elapsed.TotalSeconds) % 10 <= 1)
            {
                f = Calculate.RandomFunction4d();
                _windInterval = TimeSpan.FromMilliseconds(Calculate.Percent() * 2 + 50);
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
                                    if (f(x + _xOffset, y + _yOffset, _elapsed))
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
        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            TownMap.CalculateFOV(TownMap.ControlledGameObject.Position, Actor.FOVRadius, Settings.FOVRadius);
            List<string> output = new List<string>();
            output.Add("At coordinate " + Player.Position.X + ", " + Player.Position.Y);
            foreach (Area area in Player.GetCurrentRegions())
            {
                output.Add(area.ToString());
                output.Add(area.Orientation.ToString());
            }
            Messages.Print(output.ToArray());
            Health.Print();
            MapRenderer.CenterViewPortOnPoint(TownMap.ControlledGameObject.Position);
        }
    }
}