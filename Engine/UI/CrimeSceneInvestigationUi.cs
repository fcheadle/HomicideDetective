using Engine.Creatures.Components;
using Engine.Scenes;
using Engine.Scenes.Areas;
using Engine.UI.Components;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using SadConsole.Components;
using System.Collections.Generic;

namespace Engine.UI
{
    //refactor out upon alpha
    public class CrimeSceneInvestigationUi : UserInterface
    {
        public SceneMap Map { get; private set; }
        //public ScrollingConsole Display { get; private set; }
        public MagnifyingGlass LookingGlass { get; private set; }
        public ActorComponent Actor => (ActorComponent)Player.GetComponent<ActorComponent>();
        public CsiKeyboardComponent KeyBoardComponent => (CsiKeyboardComponent)Player.GetComponent<CsiKeyboardComponent>();
        public PageComponent<ThoughtsComponent> Thoughts => (PageComponent<ThoughtsComponent>)Player.GetComponent<PageComponent<ThoughtsComponent>>();
        public PageComponent<HealthComponent> Health => (PageComponent<HealthComponent>)Player.GetComponent<PageComponent<HealthComponent>>();

        public CrimeSceneInvestigationUi()
        {
            IsVisible = true;
            IsFocused = true;
            UseMouse = true;
            Init();
        }

        #region Init
        private void Init()
        {
            Map = new SceneMap(Game.Settings.MapWidth, Game.Settings.MapHeight);
            CreateMapRenderer();
            CreatePlayer();
            CreateControls();
        }

        private void CreateMapRenderer()
        {
            Display = Map.CreateRenderer(new Rectangle(0, 0, Game.Settings.GameWidth, Game.Settings.GameHeight), Global.FontDefault);
            Display.UseMouse = true;
            Display.FocusOnMouseClick = false;
            Children.Add(Display);
        }

        internal void ProcessTimeUnit()
        {
            //todo: foreach component in each entity, process time...
        }

        private void CreatePlayer()
        {
            Player = Game.CreatureFactory.Player(new Coord(20, 20));
            Player.IsFocused = true;
            Player.FocusOnMouseClick = true;
            Map.ControlledGameObject = Player;
            Map.AddEntity(Map.ControlledGameObject);
            Map.CalculateFOV(Map.ControlledGameObject.Position, Game.Settings.FovDistance);
            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObject_Changed;
            Display.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        private void CreateControls()
        {
            Controls = new ControlsConsole(Game.Settings.GameWidth, 3);
            Controls.Theme = new PaperWindowTheme();
            Controls.ThemeColors = ThemeColors.Clear;
            Controls.Position = new Coord(0, Game.Settings.GameHeight - 2);
            int currentX = 0;
            foreach (IConsoleComponent visible in Player.Components)
            {
                try
                {
                    IDisplay display = (IDisplay)visible;
                    if (display != null)
                    {
                        Children.Add(display.Window);
                        display.MaximizeButton.Position = new Coord(currentX, 0);
                        currentX += display.MaximizeButton.Width;
                        Controls.Add(display.MaximizeButton);
                    }
                }
                catch
                {
                    // ignored
                }
            }
            Children.Add(Controls);

            LookingGlass = new MagnifyingGlass(new Coord(Game.Settings.GameWidth / 2, Game.Settings.GameHeight / 2));
            Children.Add(LookingGlass);
        }
        #endregion

        void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            Map.CalculateFOV(Player.Position, Game.Settings.FovDistance, Game.Settings.FovRadius);
            List<string> output = new List<string>();
            string coords = "At " + Player.Position.X + ", " + Player.Position.Y + ";";
            output.Add(coords);
            string answer = "";
            

            foreach (Area area in Map.GetRegions(Player.Position))
            {
                answer += area + ", ";
            }
            output.Add(answer);
            Thoughts.Component.Think(output.ToArray());
            Health.Print();
            Display.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        private void ControlledGameObject_Changed(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }
        // Remove an Entity from the MapConsole every time the Map's Entity collection changes
        public void OnMapEntityRemoved(object sender, ItemEventArgs<IGameObject> args)
        {
            //MapRenderer.Children.Remove(args.Item);
        }

        // Add an Entity to the MapConsole every time the Map's Entity collection changes
        public void OnMapEntityAdded(object sender, ItemEventArgs<IGameObject> args)
        {
            //MapRenderer.Children.Add(args.Item);
        }
    }
}
