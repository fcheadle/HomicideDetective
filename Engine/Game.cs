using Engine.Components;
using Engine.Components.Creature;
using Engine.Components.UI;
using Engine.Entities.Creatures;
using Engine.Entities.Items;
using Engine.Entities.Terrain;
using Engine.Maps;
using Engine.Maps.Areas;
using Engine.UI;
using Engine.Utilities;
using GoRogue;
using GoRogue.GameFramework;
using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
    public class Game : IGame
    {
        public const double TimeIncrement = 100;
        public ISettings Settings { get; }
        public ICreatureFactory CreatureFactory { get; }
        public IItemFactory ItemFactory { get; }
        public ITerrainFactory TerrainFactory { get; }
        public SceneMap Map { get; private set; }
        public ScrollingConsole MapRenderer { get; private set; }
        public ContainerConsole Container { get; private set; }
        public DisplayComponent<ThoughtsComponent> Thoughts { get => (DisplayComponent<ThoughtsComponent>)Player.GetComponent<DisplayComponent<ThoughtsComponent>>(); }
        public DisplayComponent<HealthComponent> Health { get => (DisplayComponent<HealthComponent>)Player.GetComponent<DisplayComponent<HealthComponent>>(); }
        public BasicEntity Player { get => Map.ControlledGameObject; }
        public ActorComponent Actor { get => (ActorComponent)Player.GetComponent<ActorComponent>(); }

        public Game(ISettings settings, ICreatureFactory creatureFactory, IItemFactory itemFactory, ITerrainFactory terrainFactory) 
        {
            Settings = settings;
            CreatureFactory = creatureFactory;
            ItemFactory = itemFactory;
            TerrainFactory = terrainFactory;
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
            //readonly fields
            Map = new SceneMap(Settings.MapWidth, Settings.MapHeight);
            var player = CreatureFactory.Player(new Coord(15, 15));
            Map.ControlledGameObject = player;
            Map.ControlledGameObject.IsFocused = true;
            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObjectChanged;
            Map.AddEntity(Map.ControlledGameObject);
            Map.CalculateFOV(Actor.Position, Actor.FOVRadius);
            MapRenderer = Map.CreateRenderer(new GoRogue.Rectangle(0, 0, Settings.GameWidth, Settings.GameHeight), Global.FontDefault);
            MapRenderer.UseMouse = true;
            MapRenderer.FocusOnMouseClick = true;
            Container = new ContainerConsole
            {
                UseMouse = true
            };
            Container.Children.Add(MapRenderer);

            foreach(Component visible in Player.Components)
            {
                try
                {
                    IDisplay display = (IDisplay)visible;
                    if (display != null)
                    {
                        Container.Children.Add(display.Window);
                    }
                }
                catch { } //dont care
            }
            //Container.Children.Add(Thoughts.Display);//if you don't add a component to a console, it won't update
            //Container.Children.Add(Health.Display);//if you don't add a component to a console, it won't update
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
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