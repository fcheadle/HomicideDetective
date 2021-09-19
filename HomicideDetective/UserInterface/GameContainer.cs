using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GoRogue;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;
using SadRogue.Integration;
using SadRogue.Integration.Components.Keybindings;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;

namespace HomicideDetective.UserInterface
{
    public class GameContainer : ScreenObject
    {
        public RogueLikeMap Map { get; set; }
        public PageWindow MessageWindow { get; set; }

        public DateTime CurrentTime 
            => new DateTime(_currentYear, _currentMonth, _currentDay, _currentHour, _currentMinute, 0);

        private int _currentMinute;
        private int _currentHour;
        private int _currentDay;
        private int _currentMonth;
        private int _currentYear;
        
        public ReadOnlyDictionary<Keys, string> ActionNames => _actionNames.AsReadOnly();
        private readonly Dictionary<Keys, string> _actionNames = new();

        //map properties
        public const int MapWidth = 100;
        public const int MapHeight = 60;

        //ui and game properties
        public RogueLikeEntity PlayerCharacter;
        public Mystery Mystery;

        public GameContainer()
        {
            _currentDay = 5;
            _currentHour = 18;
            _currentMinute = 0;
            _currentMonth = 7;
            _currentYear = 1970;
            Mystery = new Mystery(1, 1);
            Mystery.Generate(MapWidth, MapHeight, Program.Width - 40, Program.Height);
            Map = InitMap();
            PlayerCharacter = InitPlayerCharacter();
            MessageWindow = InitMessageWindow();

            var victim = Mystery.Victim.GoRogueComponents.GetFirst<Substantive>();
            var caseDetails = $"{CurrentTime}\r\n";
            caseDetails += $"The Mystery of {victim.Name}\r\n";
            caseDetails += $"{victim.Name} was found dead at {Mystery.SceneOfCrimeInfo.Name}. ";
            caseDetails += $"{Mystery.SceneOfCrimeInfo.Description}\r\n";
            caseDetails += $"{victim.PronounPossessive} friends mourn for {victim.Pronoun}. ";
            caseDetails += $"Perhaps you should ask {victim.PronounPossessive} family and coworkers for clues.\r\n";
            
            MessageWindow.Write(caseDetails);
        }

        private RogueLikeMap InitMap()
        {
            var map = Mystery.CurrentLocation;
            Children.Add(map);
            return map;
        }
        private RogueLikeEntity InitPlayerCharacter()
        {
            //creation
            var position = Mystery.RandomFreeSpace(Map);
            var player = new RogueLikeEntity(position, 1, false)
            {
                IsFocused = true,
            };
            
            //general personhood
            var thoughts = new Memories();
            player.AllComponents.Add(thoughts);
            player.AllComponents.Add(new Speech());
            
            //player controls
            var motionControl = InitKeyCommands();
            player.AllComponents.Add(motionControl);
            
            //add to map and make view center on player
            Map.AddEntity(player);
            Map.DefaultRenderer?.SadComponents.Add(new SurfaceComponentFollowTarget { Target = player });

            player.PositionChanged += ProcessUnitOfTime;
            player.PositionChanged += (s,e) =>
            {
                Mystery.CurrentLocation.PlayerFOV.Calculate(PlayerCharacter.Position, 12, Mystery.CurrentLocation.DistanceMeasurement);
            };

            return player;
        }
        private PlayerKeybindingsComponent InitKeyCommands()
        {
            var motionControl = new PlayerKeybindingsComponent();
            AddNamedAction(motionControl, Keys.T, KeyCommands.Talk, "Talk");
            AddNamedAction(motionControl, Keys.L, KeyCommands.LookAround, "Look Around");
            AddNamedAction(motionControl, Keys.I, KeyCommands.Inspect, "Inspect");
            AddNamedAction(motionControl, Keys.M, KeyCommands.NextMap, "Fast Travel");
            AddNamedAction(motionControl, Keys.OemQuestion, KeyCommands.PrintCommands, "Print This");
            AddNamedAction(motionControl, Keys.PageUp, PageWindow.BackPage, "Back Page");
            AddNamedAction(motionControl, Keys.PageDown, PageWindow.ForwardPage, "Forward Page");
            AddNamedAction(motionControl, Keys.Insert, PageWindow.WriteGarbage, "Think Garbage"); //for testing purposes
            AddNamedAction(motionControl, Keys.Home, PageWindow.UpOneLine, "Scroll Up");
            AddNamedAction(motionControl, Keys.End, PageWindow.DownOneLine, "Scroll Down");
            return motionControl;
        }
        private PageWindow InitMessageWindow()
        {
            //create the component and feed it the player's current thoughts
            var page = new PageWindow(40, Program.Height);
            
            //size the background surface to perfection
            page.BackgroundSurface.Position = (Program.Width - page.TextSurface.Surface.Width - 1, 0);
            page.BackgroundSurface.IsVisible = true;
            Children.Add(page.BackgroundSurface);
            return page;
        }
        public static void ProcessUnitOfTime(object? sender, ValueChangedEventArgs<Point> valueChangedEventArgs)
        {
            var game = Program.CurrentGame;
            var date = game.CurrentTime + TimeSpan.FromMinutes(1);
            game._currentDay = date.Day;
            game._currentHour = date.Hour;
            game._currentMinute = date.Minute;
            game._currentMonth = date.Month;
            game._currentYear = date.Month;
            //todo
        }

        public void NextMap()
        {
            Children.Remove(Map);
            Map.RemoveEntity(PlayerCharacter);
            Mystery.NextMap();
            Map = Mystery.CurrentLocation;
            PlayerCharacter.Position = Mystery.RandomFreeSpace(Map);
            if(!Map.Entities.Contains(PlayerCharacter))
                Map.AddEntity(PlayerCharacter);
            
            Map.DefaultRenderer?.SadComponents.Add(new SurfaceComponentFollowTarget { Target = PlayerCharacter });
            //Weather = new Weather(Map);
            Children.Add(Map);
        }
        
        
        public void AddNamedAction(PlayerKeybindingsComponent component, Keys key, Action action, string name)
        {
            _actionNames.Add(key, name);
            component.AddAction(key, action);
        }
    }
}