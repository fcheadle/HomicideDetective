using Engine.Components.UI;
using Engine.UI.Components;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Engine.UI
{
    public class MenuUi : UserInterface
    {
        //public ScrollingConsole Display { get; private set; } //the main box that covers the screen
        //public BasicEntity ControlledGameObject { get; private set; } //the cursor
        public ScrollingConsole TitleConsole { get; private set; } // H O M I C I D E    D E T E C E T I V E 
        public List<Button> MainOptions { get; private set; } //the main menu that appears when you pause
        public HelpConsole HelpOptions { get; private set; } //console that holds the search box / cheats menu
        public List<Button> NewGameOptions { get; private set; } //quickstart / advanced options
        public List<ControlBase> NewGameAdvancedOptions { get; private set; } //name / color / glyph / option for tutorial
        public List<ControlBase> SettingsOptions { get; private set; }

        readonly int width = Game.Settings.GameWidth / 3;
        readonly int height = Game.Settings.GameHeight / 3;
        readonly int middlePositionX = Game.Settings.GameWidth / 3;
        readonly int middlePositionY = Game.Settings.GameHeight / 3;
        readonly int rightPositionX = (Game.Settings.GameWidth / 3) * 2;
        readonly int bottomPositionY = (Game.Settings.GameHeight / 3) * 2;
        readonly int oneSubMenuOpenOffsetX = Game.Settings.GameWidth / 6;

        readonly Coord middlePosition;
        readonly Coord openSubMenuOffset;
        public MenuUi()
        {
            IsVisible = false;
            IsFocused = false;
            middlePosition = new Coord(middlePositionX, middlePositionY);
            openSubMenuOffset = new Coord(oneSubMenuOpenOffsetX, middlePositionY);
            UseMouse = true;
            UseKeyboard = true;
            InitDisplay();
            InitTitleConsole();
            InitControls(Game.Settings.GameWidth, middlePositionY);
            InitMainOptions();
            InitHelpOptions();
            InitNewGameOptions();
            InitNewGameAdvancedOptions();
            InitSettingsOptions();
        }

        private void InitNewGameAdvancedOptions()
        {
            NewGameAdvancedOptions = new List<ControlBase>();
            Controls.Add(MakeButton("Start", AdvancedStartButton_Click)); 
            
            //start.Position = new Coord(rightPositionX, 2);
            //NewGameAdvancedOptions.Add(start);
            //start.IsVisible = false;
            //for(int i = 0; i < NewGameOptions.Count; i++)
            //{
            //    NewGameOptions[i].P
            //    btn.IsVisible = false;
            //    Controls.Add(btn);
            //}
        }
        #region initilization 
        protected override void InitControls(int width, int height)
        {
            base.InitControls(width, height);
            Controls.Position = new Coord(0, middlePositionY);
            Controls.Theme = new MenuControlsTheme();
            Controls.ThemeColors = ThemeColors.Menu;

            ControlledGameObject = MenuSelector(Controls.Position);
            Controls.Children.Add(ControlledGameObject);
        }
        private void InitTitleConsole()
        {
            //takes up the top 1/3 of the screen
            TitleConsole = new ScrollingConsole(Game.Settings.GameWidth, height);
            TitleConsole.IsVisible = true;

            TitleConsole.Position = new Coord(0, 0);
            TitleConsole.FillWithRandomGarbage();//for debugging purposes
            TitleConsole.Print(2, 2, "!!! H O M I C I D E   D E T E C T I V E !!!", Color.White, Color.Black);
            TitleConsole.IsVisible = true;
            Display.Children.Add(TitleConsole);
        }


        private void InitMainOptions()
        {
            MainOptions = new List<Button>();

            MakeButton("Continue", ContinueButton_Click);

            MakeButton("New Game", ContinueButton_Click);

            MakeButton("Settings", ContinueButton_Click);

            MakeButton("Help", ContinueButton_Click);

            for (int i = 0; i < MainOptions.Count; i++)
            {
                MainOptions[i].IsVisible = true;
                MainOptions[i].IsEnabled = true;
                MainOptions[i].Position = new Coord(middlePositionX, 2 + (i * 2));
            }
        }
        private void InitHelpOptions()
        {
            HelpOptions = new HelpConsole();
            HelpOptions.Position = new Coord(Game.Settings.GameWidth / 4, middlePositionY);
            HelpOptions.IsVisible = false;
            Controls.Children.Add(HelpOptions);
        }

        private void InitSettingsOptions()
        {
            SettingsOptions = new List<ControlBase>();
            //foreach variable in Settings...?
            int count = 0;
            PropertyInfo[] properties = typeof(Settings).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                Button btn = MakeButton(property.Name, Except); //temporary
                btn.Position = new Coord(rightPositionX, 2 + 2 * count);
                btn.IsVisible = false;
                SettingsOptions.Add(btn);
                count++;
            }

            foreach(Button button in SettingsOptions)
            {
                Controls.Add(button);
            }


        }
        private void Except(object sender, EventArgs e) => throw new Exception();//temporary
        private void InitNewGameOptions()
        {
            NewGameOptions = new List<Button>();

            MakeButton("Quickstart", QuickStartButton_Click);
            MakeButton("Advanced", AdvancedStartButton_Click);
            for(int i = 0; i < NewGameOptions.Count; i++)
            {
                NewGameOptions[i].Position = new Coord(18, 2 + 2 * i);
                NewGameOptions[i].IsVisible = false;
            }
        }
        private BasicEntity MenuSelector(Coord position)
        {
            BasicEntity cursor = new BasicEntity(Color.White, Color.Black, 16, position, 1, true, true);
            cursor.Components.Add(new MenuKeyboardComponent(cursor));
            return cursor;
        }
        #endregion

        #region utilties
        public override void Hide()
        {
            base.Hide();
            Global.CurrentScreen = Game.UIManager;
            Game.UIManager.ControlledGameObject.IsFocused = true;
        }
        public override void Show()
        {
            base.Show();
            Global.CurrentScreen = this;
            ControlledGameObject.IsFocused = true;
        }
        #endregion

        #region event handlers

        private void AdvancedStartButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void QuickStartButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void HelpButton_Click(object sender, EventArgs e)
        {
            //open help console
            HelpOptions.IsVisible = true;
            HelpOptions.IsFocused = true;
            //MainOptions.Position = new Coord(Game.Settings.GameWidth / 3, Game.Settings.GameHeight / 3);
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            //open settings console
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            // open quickstart or advanced start console
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            Hide();
        }
        #endregion
    }
}
