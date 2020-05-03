using Engine.Maps;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using XnaRect = Microsoft.Xna.Framework.Rectangle;
using TownMap = Engine.Maps.TownMap;
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
        TimeSpan _windInterval = TimeSpan.FromMilliseconds(100);
        Func<int, int, TimeSpan, bool> f = Calculate.RandomFunction4d();
        Dictionary<BlowsInWindComponent, bool> _windAffectedSpots = new Dictionary<BlowsInWindComponent, bool>();
        internal TownMap TownMap { get; }
        internal ScrollingConsole MapRenderer { get; }
        internal MessageConsole Messages { get; }
        internal DisplayStatsComponent Health { get => Player.GetGoRogueComponent<DisplayStatsComponent>(); }
        internal BasicEntity Player { get; }
        internal ActorComponent Actor { get => Player.GetGoRogueComponent<ActorComponent>(); }
        internal GameScreen(int mapWidth, int mapHeight, int viewportWidth, int viewportHeight)
        {
            TownMap = new TownMap(mapWidth, mapHeight);
            Player = CreatureFactory.Player(new Coord(33, 33));
            TownMap.ControlledGameObject = Player;
            TownMap.AddEntity(TownMap.ControlledGameObject);
            
            Messages = new MessageConsole(24, viewportHeight - 24);
            Messages.Position = new Coord(viewportWidth - 24, 0);

            MapRenderer = TownMap.CreateRenderer(new XnaRect(0, 0, viewportWidth - 25, viewportHeight), Global.FontDefault);
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
            if(_elapsed / _windCounter > 100)
            {
                _elapsed = default;
                f = Calculate.RandomFunction4d();
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
                                    if (f(x, y, _elapsed))
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
            Area[] areas = Player.GetCurrentRegions().ToArray();
            if (areas != null)
                Messages.Print(areas);
            Health.Print();
            MapRenderer.CenterViewPortOnPoint(TownMap.ControlledGameObject.Position);
        }
    }
}