﻿using Engine.Components;
using Engine.Components.Creature;
using Engine.Components.UI;
using Engine.Entities.Creatures;
using Engine.Entities.Items;
using Engine.Entities.Terrain;
using Engine.Maps;
using Engine.Maps.Areas;
using Engine.Utilities;
using GoRogue;
using GoRogue.GameFramework;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
    public class Game : IGame
    {
        public const double TimeIncrement = 100;
        public ISettings Settings { get => _settings; }
        public ICreatureFactory CreatureFactory { get => _creatureFactory; }
        public IItemFactory ItemFactory { get => _itemFactory; }
        public ITerrainFactory TerrainFactory { get => _terrainFactory; }
        public SceneMap Map { get; private set; }
        public ScrollingConsole MapRenderer { get; private set; }
        public ContainerConsole Container { get; private set; }
        public BasicEntity Player { get => Map.ControlledGameObject; }
        public ActorComponent Actor { get => (ActorComponent)Player.GetComponent<ActorComponent>(); }
        public CSIKeyboardComponent KeyBoardComponent { get => (CSIKeyboardComponent)Player.GetComponent<CSIKeyboardComponent>(); }
        public PageComponent<ThoughtsComponent> Thoughts { get => (PageComponent<ThoughtsComponent>)Player.GetComponent<PageComponent<ThoughtsComponent>>(); }
        public PageComponent<HealthComponent> Health { get => (PageComponent<HealthComponent>)Player.GetComponent<PageComponent<HealthComponent>>(); }
        private static ISettings _settings;
        private static ICreatureFactory _creatureFactory;
        private static ITerrainFactory _terrainFactory;
        private static IItemFactory _itemFactory;
        public bool IsPaused { get => SadConsole.Global.CurrentScreen.IsPaused; set => SadConsole.Global.CurrentScreen.IsPaused = value; }
        private int _fovRadius;
        public Game(ISettings settings, ICreatureFactory creatureFactory, IItemFactory itemFactory, ITerrainFactory terrainFactory) 
        {
            _settings = settings;
            _creatureFactory = creatureFactory;
            _itemFactory = itemFactory;
            _terrainFactory = terrainFactory;
            Setup();
        }

        public void Setup()
        {
            SadConsole.Game.Create(Settings.GameWidth, Settings.GameHeight);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
        }
        public void Init()
        {
            Map = new SceneMap(Settings.MapWidth, Settings.MapHeight);
            //just in case weird things happened, move this to after player declaration?
            MapRenderer = Map.CreateRenderer(new GoRogue.Rectangle(0, 0, Settings.GameWidth, Settings.GameHeight), Global.FontDefault); 
            MapRenderer.UseMouse = true;
            MapRenderer.FocusOnMouseClick = false;
            Map.ControlledGameObject = CreatureFactory.Player(new Coord(15, 15));
            Map.ControlledGameObject.IsFocused = true;
            Map.ControlledGameObject.FocusOnMouseClick = true;
            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObjectChanged;
            Map.AddEntity(Map.ControlledGameObject);
            Map.CalculateFOV(Actor.Position, Actor.FOVRadius);
            _fovRadius = Actor.FOVRadius;
            
            Container = new ContainerConsole();
            Container.Children.Add(MapRenderer);
            ControlsConsole Controls = new ControlsConsole(Settings.GameWidth, 3);
            Controls.Theme = new PaperWindowTheme();
            Controls.ThemeColors = ThemeColor.Clear;
            Controls.Position = new Coord(0, Settings.GameHeight - 2);
            int currentX = 0;
            foreach(IConsoleComponent visible in Player.Components)
            {
                try
                {
                    IDisplay display = (IDisplay)visible;
                    if (display != null)
                    {
                        Container.Children.Add(display.Window);
                        display.MaximizeButton.Position = new Coord(currentX, 0);
                        currentX += display.MaximizeButton.Surface.Width;
                        Controls.Add(display.MaximizeButton);
                    }
                }
                catch { } //dont care
            }
            Container.Children.Add(Controls);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
            Container.Components.Add(new WeatherComponent(Map));
            
            Global.CurrentScreen = Container;
        }
        public void Start()
        {
            SadConsole.Game.Instance.Run();
        }
        public void Stop()
        {
            SadConsole.Game.Instance.Dispose();
        }
        public void Update(GameTime time)
        {
            //is it good practice to do keyboard interception here?
            if (Global.KeyboardState.IsKeyReleased(Settings.KeyBindings[GameActions.RefocusOnPlayer]))
            {
                Player.IsFocused = true;
            }
            //Container.IsDirty = true;
            MapRenderer.IsDirty = true;
        }

        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }
        void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            Map.CalculateFOV(Map.ControlledGameObject.Position, Actor.FOVRadius, Settings.FOVRadius);
            List<string> output = new List<string>();
            output.Add("At coordinate " + Player.Position.X + ", " + Player.Position.Y);
            foreach (Area area in GetRegions(Player.Position))
            {
                output.Add(area.ToString());
                output.Add(area.Orientation.ToString());
            }
            Thoughts.Print(output.ToArray());
            Health.Print();
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        private IEnumerable<Area> GetRegions(Coord position)
        {
            foreach (Area area in Map.Regions)
            {
                if (area.InnerPoints.Contains(position))
                    yield return area;
            }
        }
    }
}