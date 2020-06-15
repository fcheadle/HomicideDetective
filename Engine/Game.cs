using Engine.Components;
using Engine.Components.Creature;
using Engine.Components.UI;
using Engine.Entities.Creatures;
using Engine.Entities.Items;
using Engine.Entities.Terrain;
using Engine.Maps;
using Engine.UserInterface;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Engine
{
    public class Game
    {
        public const double TimeIncrement = 100;
        public static Settings Settings => _settings; 
        public static ICreatureFactory CreatureFactory => _creatureFactory; 
        public static IItemFactory ItemFactory => _itemFactory; 
        public static ITerrainFactory TerrainFactory => _terrainFactory; 
        public static UserInterface.UserInterface UIManager => _uiManager; 
        public static SceneMap Map => UIManager.Map;
        public ScrollingConsole MapRenderer => UIManager.MapRenderer;
        public BasicEntity Player => UIManager.Player;
        public ActorComponent Actor => (ActorComponent)Player.GetComponent<ActorComponent>(); 
        public CSIKeyboardComponent KeyBoardComponent => (CSIKeyboardComponent)Player.GetComponent<CSIKeyboardComponent>();
        public PageComponent<ThoughtsComponent> Thoughts => (PageComponent<ThoughtsComponent>)Player.GetComponent<PageComponent<ThoughtsComponent>>(); 
        public PageComponent<HealthComponent> Health => (PageComponent<HealthComponent>)Player.GetComponent<PageComponent<HealthComponent>>(); 
        private static Settings _settings;
        private static ICreatureFactory _creatureFactory;
        private static ITerrainFactory _terrainFactory;
        private static IItemFactory _itemFactory;
        private static UserInterface.UserInterface _uiManager;

        public bool IsPaused { get => SadConsole.Global.CurrentScreen.IsPaused; set => SadConsole.Global.CurrentScreen.IsPaused = value; }

        public Game(Settings settings, ICreatureFactory creatureFactory, IItemFactory itemFactory, ITerrainFactory terrainFactory) 
        {
            ApplySettings(settings);
            SetCreatureFactory(creatureFactory);
            SetItemFactory(itemFactory);
            SetTerrainFactory(terrainFactory);
            Setup();
        }

        protected Game()
        {

        }

        protected void ApplySettings(Settings settings)
        {
            _settings = settings;
        }

        protected void SetCreatureFactory(ICreatureFactory creatureFactory)
        {
            _creatureFactory = creatureFactory;
        }

        protected void SetItemFactory(IItemFactory itemFactory)
        {
            _itemFactory = itemFactory;
        }

        protected void SetTerrainFactory(ITerrainFactory terrainFactory)
        {
            _terrainFactory = terrainFactory;
        }
        protected void Setup()
        {
            SadConsole.Game.Create(Settings.GameWidth, Settings.GameHeight);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
        }
        public virtual void Init()
        {
            _uiManager = new UserInterface.UserInterface();
            Global.CurrentScreen = UIManager;
        }
        public virtual void Start()
        {
            SadConsole.Game.Instance.Run();
        }
        public virtual void Stop()
        {
            SadConsole.Game.Instance.Dispose();
        }
        public virtual void Update(GameTime time)
        {
        }
    }
}