using System.Collections.Generic;
using System.Linq;
using GoRogue.Random;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using HomicideDetective.People.Components;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;
using TheSadRogue.Integration.Maps;

namespace HomicideDetective
{
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        public const int MapWidth = 80;// for now
        public const int MapHeight = 25; // for now
        
        private static Mystery CurrentMystery;
        private static List<Mystery> _mysteries;
        private static RogueLikeMap _map;
        private static RogueLikeEntity _playerCharacter;
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
            CurrentMystery = new Mystery(GlobalRandom.DefaultRNG.Next(), 1);
            CurrentMystery.CommitMurder();
            _map = CurrentMystery.CurrentScene;
            _playerCharacter = GeneratePlayerCharacter();
            GameHost.Instance.Screen = _map;
            _messageWindow = GenerateMessageWindow();
        }
        
        private static RogueLikeEntity GeneratePlayerCharacter()
        {
            var position = _map.WalkabilityView.Positions().First(p => _map.WalkabilityView[p]);
            var player = new Person(position, "Detective", "Player", "You", 2400, 2400, "positively tiny", "massive beyond belief");
            
            var controls = new PlayerControlsComponent();
            var speech = player.AllComponents.GetFirst<Speech>();
            controls.AddKeyCommand(Keys.Left, speech.TalkLeft());
            controls.AddKeyCommand(Keys.Right, speech.TalkRight());
            controls.AddKeyCommand(Keys.Up, speech.TalkUp());
            controls.AddKeyCommand(Keys.Down, speech.TalkDown());
            
            player.AllComponents.Add(controls);
            player.IsFocused = true;
            _map.AddEntity(player);

            return player;
        }

        private static ScreenSurface GenerateMessageWindow()
        {
            var thoughts = _playerCharacter.AllComponents.GetFirst<Thoughts>();
            thoughts.Think($"Case number 1, {CurrentMystery.Victim.Name}.");
            var page = new PageComponent<Thoughts>(thoughts);
            page.Window.Position = (Width - page.Window.Surface.Width, 0);
            page.Window.IsVisible = true;
            _playerCharacter.AllComponents.Add(page);

            _map.Children.Add(page.Window);
            return page.Window;
        }
    }
}