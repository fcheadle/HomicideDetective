using Engine.Components.UI;
using Engine.Creatures.Components;
using Engine.Scenes;
using Engine.Scenes.Areas;
using Engine.UI.Components;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using SadConsole.Components;
using System;
using System.Collections.Generic;

namespace Engine.UI
{
    public class UserInterface : ContainerConsole
    {
        public SceneMap Map { get; private set; }
        public ScrollingConsole MapRenderer { get; private set; }
        public ControlsConsole Controls { get; private set; }
        public MagnifyingGlass LookingGlass { get; private set; }
        public BasicEntity Player => Map.ControlledGameObject;
        public ActorComponent Actor => (ActorComponent)Player.GetComponent<ActorComponent>();
        public CSIKeyboardComponent KeyBoardComponent => (CSIKeyboardComponent)Player.GetComponent<CSIKeyboardComponent>();
        public PageComponent<ThoughtsComponent> Thoughts => (PageComponent<ThoughtsComponent>)Player.GetComponent<PageComponent<ThoughtsComponent>>();
        public PageComponent<HealthComponent> Health => (PageComponent<HealthComponent>)Player.GetComponent<PageComponent<HealthComponent>>();

        public UserInterface()
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
            MapRenderer = Map.CreateRenderer(new Rectangle(0, 0, Game.Settings.GameWidth, Game.Settings.GameHeight), Global.FontDefault);
            MapRenderer.UseMouse = true;
            MapRenderer.FocusOnMouseClick = false;
            Children.Add(MapRenderer);
        }

        internal void ProcessTimeUnit()
        {
            //todo: foreach component in each entity, process time...
        }

        private void CreatePlayer()
        {
            Map.ControlledGameObject = Game.CreatureFactory.Player(new Coord(20, 20));
            Map.ControlledGameObject.IsFocused = true;
            Map.ControlledGameObject.FocusOnMouseClick = true;
            Map.AddEntity(Map.ControlledGameObject);
            Map.CalculateFOV(Map.ControlledGameObject.Position, Game.Settings.FovDistance);
            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObject_Changed;
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        private void CreateControls()
        {
            Controls = new ControlsConsole(Game.Settings.GameWidth, 3);
            Controls.Theme = new PaperWindowTheme();
            Controls.ThemeColors = ThemeColor.Clear;
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
                catch { } //dont care
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
            output.Add("At coordinate " + Player.Position.X + ", " + Player.Position.Y);
            foreach (Area area in Map.GetRegions(Player.Position))
            {
                output.Add(area.ToString());
                output.Add(area.Orientation.ToString());
            }
            Thoughts.Component.Think(output.ToArray());
            Health.Print();
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }
        public override void Update(TimeSpan timeElapsed)
        {
            base.Update(timeElapsed);
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
