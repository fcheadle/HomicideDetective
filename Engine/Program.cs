using Engine.Components;
using Engine.Components.Creature;
using Engine.Extensions;
using Engine.UI;
using Microsoft.Xna.Framework;
using SadConsole;
using System;

namespace Engine
{
    public class Program
    {
        private static GameState _state;
        internal static GameScreen MapScreen { get; private set; }
        internal static TimeSpan ActCounter { get; private set; } = TimeSpan.FromSeconds(0);

        internal static void Main()
        {
            SadConsole.Game.Create(Settings.GameWidth, Settings.GameHeight);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Update(GameTime obj)
        {
            ActCounter += obj.ElapsedGameTime;
            MapScreen.BlowWind(obj.ElapsedGameTime);
            MapScreen.Player.GetGoRogueComponent<KeyboardComponent>().ProcessGameFrame();
            MapScreen.MapRenderer.IsDirty = true;// (obj.ElapsedGameTime);
            if (ActCounter > TimeSpan.FromMilliseconds(250))
            {
                foreach (SadConsole.BasicEntity creature in MapScreen.TownMap.GetCreatures())
                {
                    creature.GetGoRogueComponent<ActorComponent>().Act();
                }
                ActCounter = TimeSpan.FromMilliseconds(0);
            }
        }

        private static void Init()
        {
            //SetColors();
            //SadConsole.Themes.Library.Default.SetControlTheme(typeof(SadConsole.Controls.DrawingSurface) ,new Theme());
            MapScreen = new GameScreen(Settings.MapWidth, Settings.MapHeight, Settings.GameWidth, Settings.GameHeight);
            SadConsole.Global.CurrentScreen = MapScreen;
        }

        //private static void SetColors()
        //{
        //    var colors = SadConsole.Themes.Library.Default.Colors.Clone();
        //    colors.Appearance_ControlNormal = new Cell(Color.White, Color.Black);
        //    colors.Appearance_ControlFocused = new Cell(Color.White, Color.Gray);
        //    colors.Appearance_ControlMouseDown = new Cell(Color.White, Color.DarkGray);
        //    colors.Appearance_ControlOver = new Cell(Color.White, Color.Gray);
        //    colors.Appearance_ControlSelected = new Cell(Color.White, Color.Gray);
        //    colors.TextLight = Color.LightBlue;
        //    colors.ControlBack = Color.Tan;
        //    colors.ControlHostBack = Color.Tan;
        //    colors.Text = Color.DarkBlue;
        //    colors.TitleText = Color.DarkBlue;

        //    SadConsole.Themes.Library.Default.Colors = colors;
        //}

        public static void Start()
        {
            Main();
        }


        //static void SwitchState(GameState newState)
        //{
        //    if (_state != null) _state.OnExit();
        //    _state = newState;
        //    _state.OnEnter();
        //}
    }
}
