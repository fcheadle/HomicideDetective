using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using HomicideDetective.New.People;
using HomicideDetective.New.Places;
using HomicideDetective.New.Places.Generation;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;
using TheSadRogue.Integration.Maps;

namespace HomicideDetective.New
{
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        private const int MapWidth = 160;// for now
        private const int MapHeight = 50; // for now
        public static readonly Random GlobalRandom = new Random();
        
        private static RogueLikeMap _map;
        private static RogueLikeEntity _playerCharacter;
        private static ThoughtComponent _thoughts => _playerCharacter.AllComponents.GetFirst<ThoughtComponent>();
        private static SettableCellSurface _mapWindow;
        private static ScreenSurface _messageWindow;
        static void Main(/*string[] args*/)
        {
            Game.Create(Width, Height);
            Game.Instance.OnStart = Init;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        /// <summary>
        /// Runs before the game starts
        /// </summary>
        private static void Init()
        {
            _map = GenerateMap();
            _playerCharacter = GeneratePlayerCharacter();
            GameHost.Instance.Screen = _map;
            _messageWindow = GenerateMessageWindow();
        }

        private static RogueLikeMap GenerateMap()
        {
            var crimeScene = new CrimeScene(MapWidth, MapHeight);
            crimeScene.Generate();
            return crimeScene;
        }

        private static RogueLikeEntity GeneratePlayerCharacter()
        {
            var position = _map.WalkabilityView.Positions().First(p => _map.WalkabilityView[p]);
            var player = new Person(position);

            player.AllComponents.Add(new PlayerControlsComponent());
            player.IsFocused = true;
            _map.AddEntity(player);

            return player;
        }

        private static ScreenSurface GenerateMessageWindow()
        {
            _thoughts.Think("Detective PlayerName is on the case!");
            var page = new PageComponent<ThoughtComponent>(_thoughts);
            page.Window.Position = (Width - page.Window.Surface.Width, 0);//);
            page.Window.IsVisible = true;
            _playerCharacter.AllComponents.Add(page);

            _map.Children.Add(page.Window);
            return page.Window;
        }
    }
}