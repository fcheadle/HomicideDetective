using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using HomicideDetective.People.Speech;
using HomicideDetective.Places;
using HomicideDetective.Words;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using Console = SadConsole.Console;

namespace HomicideDetective.UserInterface
{
    public class GameContainer : ScreenObject
    {
        public RogueLikeMap Map { get; set; }
        public PageWindow MessageWindow { get; set; }
        
        public DateTime CurrentTime 
            => new (_currentYear, _currentMonth, _currentDay, _currentHour, _currentMinute, _currentSecond);

        public CommandContext Context { get; private set; }
        public CommandContext DefaultContext { get; } = CommandContext.CrimeSceneInvestigationContext();
        private int _currentSecond = 0;
        private int _currentMinute = 0;
        private int _currentHour = 18;
        private int _currentDay = 5;
        private int _currentMonth = 7;
        private int _currentYear = 1970;
        private readonly int _commandDelay = 5;
        private int _commandDelayCounter = 0;
        //public List<KeyCommand> Commands => _commands;
        //private readonly List<KeyCommand> _commands = new();
        
        //map properties
        public const int MapWidth = 100;
        public const int MapHeight = 60;

        //ui and game properties
        public RogueLikeEntity PlayerCharacter;
        public Mystery Mystery;

        private readonly Console _loadingScreen;
        private readonly List<string> _loadingStepsCompleted = new();
        
        public GameContainer()
        {
            IsFocused = true;
            Mystery = new Mystery(new Random().Next(), 1);
            Mystery.SetDimensions(MapWidth, MapHeight, Program.Width - 40, Program.Height);
            Context = DefaultContext;
            _loadingScreen = new Console(Program.Width, Program.Height * 10);
            Children.Add(_loadingScreen);
        }

        private void OpenMystery()
        {
            Children.Remove(_loadingScreen);
            Map = InitMap();
            PlayerCharacter = InitPlayerCharacter();
            MessageWindow = InitMessageWindow();
            
            var victim = Mystery.Victim!.GoRogueComponents.GetFirst<Substantive>();
            var caseDetails = new StringBuilder($"{CurrentTime}\r\n");
            caseDetails.Append($"The Mystery of {victim.Name}\r\n");
            caseDetails.Append($"{victim.Name} was found dead at {victim.Pronouns.Possessive} home. ");
            caseDetails.Append($"{Mystery.SceneOfCrimeInfo.Description}\r\n");
            caseDetails.Append($"{victim.Pronouns.Possessive} friends mourn for {victim.Pronouns.Objective}. ");
            caseDetails.Append($"Perhaps you should ask {victim.Pronouns.Possessive} family and coworkers for clues.\r\n");
            caseDetails.Append("\r\n");
            caseDetails.Append(DefaultContext.ToString());
            MessageWindow.Write(caseDetails.ToString());
            PlayerCharacter.Position = (0,0);
        }

        public override void Update(TimeSpan delta)
        {
            if (!Mystery.GenerationComplete)
            {
                var step = Mystery.DoGenerationStep();
                _loadingStepsCompleted.Add(step);
                
                var count = _loadingStepsCompleted.Count > Program.Height ? Program.Height : _loadingStepsCompleted.Count;
                _loadingScreen.Print(0, count, step);
                
                if (count >= Program.Height)
                    _loadingScreen.ShiftUp();
                
                if(step.Contains("Good luck"))
                    OpenMystery();
            }
        }
        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            if(_commandDelayCounter >= _commandDelay)
            {
                ResolveMovement(keyboard);

                if (Context.ProcessKeyboard(keyboard))
                    _commandDelayCounter = 0;
                else
                    _commandDelayCounter++;
            }
            else
            {
                _commandDelayCounter++;
            }

            return true;
        }

        private void ResolveMovement(Keyboard keyboard)
        {
            //motions
            if (keyboard.IsKeyDown(Keys.Left))
                Interact(PlayerCharacter.Position + Direction.Left);

            if (keyboard.IsKeyDown(Keys.Up))
                Interact(PlayerCharacter.Position + Direction.Up);

            if (keyboard.IsKeyDown(Keys.Right))
                Interact(PlayerCharacter.Position + Direction.Right);

            if (keyboard.IsKeyDown(Keys.Down))
                Interact(PlayerCharacter.Position + Direction.Down);
        }

        private void Interact(Point position)
        {
            if (Context != DefaultContext)
                Context = DefaultContext;
            
            if (PlayerCharacter.CanMove(position))
            {
                PlayerCharacter.Position = position;
                _commandDelayCounter = 0;
            }
            else if (Map.Contains(position))
            {
                var entities = Map.Entities.GetItemsAt(position);
                foreach (var entity in entities)
                {
                    var substantive = entity.GoRogueComponents.GetFirstOrDefault<ISubstantive>();
                    if (substantive != null)
                    {
                        if(substantive.Type == SubstantiveTypes.Person)
                            UserInterface.Commands.TalkTo(position);
                        if(substantive.Type == SubstantiveTypes.Thing)
                            UserInterface.Commands.LookAt(position);
                    }
                }

                _commandDelayCounter = 0;
            }
        }

        private RogueLikeMap InitMap()
        {
            var map = Mystery.CurrentLocation;
            Children.Add(map);
            return map!;
        }
        private RogueLikeEntity InitPlayerCharacter()
        {
            //creation
            var position = Map.RandomFreeSpace();
            var player = new RogueLikeEntity(position, 1, false);
            
            //general personhood
            var thoughts = new Memories();
            player.AllComponents.Add(thoughts);
            player.AllComponents.Add(new SpeechComponent(Constants.MalePronouns));

            //add to map and make view center on player
            Map.AddEntity(player);
            Map.DefaultRenderer?.SadComponents.Add(new SurfaceComponentFollowTarget { Target = player });

            player.PositionChanged += (s,e) =>
            {
                ProcessUnitOfTime(s,e);
                Mystery.CurrentLocation!.PlayerFOV.Calculate(PlayerCharacter.Position, 12, Mystery.CurrentLocation.DistanceMeasurement);
            };

            return player;
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
            if (game is not null)
            {
                var date = game.CurrentTime + TimeSpan.FromSeconds(5);
                game._currentDay = date.Day;
                game._currentHour = date.Hour;
                game._currentMinute = date.Minute;
                game._currentMonth = date.Month;
                game._currentYear = date.Year;
                game._currentSecond = date.Second;
                //todo
            }
        }

        public void NextMap()
        {
            Children.Remove(Map);
            Map.RemoveEntity(PlayerCharacter);
            Mystery.NextMap();
            Map = Mystery.CurrentLocation!;
            PlayerCharacter.Position = Map.RandomFreeSpace();
            if(!Map.Entities.Contains(PlayerCharacter))
                Map.AddEntity(PlayerCharacter);
            
            Map.DefaultRenderer?.SadComponents.Add(new SurfaceComponentFollowTarget { Target = PlayerCharacter });
            //Weather = new Weather(Map);
            Children.Add(Map);
        }

        public void SetContext(ConversationContext context)
        {
            Context = context;
            Commands.PrintCommands();
        }
    }
}