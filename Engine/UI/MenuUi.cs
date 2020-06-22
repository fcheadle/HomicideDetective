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
        public ControlsConsole MainOptions { get; private set; } //the main menu that appears when you pause
        public HelpConsole HelpOptions { get; private set; } //console that holds the search box / cheats menu
        public ControlsConsole NewGameOptions { get; private set; } //quickstart / advanced options
        public ControlsConsole NewGameAdvancedOptions { get; private set; } //name / color / glyph / option for tutorial
        public ControlsConsole SettingsOptions { get; private set; }

        readonly int width = Game.Settings.GameWidth / 3;
        readonly int height = Game.Settings.GameHeight / 3;

        readonly Coord middlePosition;
        readonly Coord openSubMenuOffset;
        public MenuUi()
        {
            IsVisible = false;
            IsFocused = false;
            middlePosition = new Coord(width, height);
            openSubMenuOffset = new Coord(width / 2, height / 2);
            UseMouse = true;
            UseKeyboard = true;
            InitDisplay();
            InitTitleConsole();
            InitControls(Game.Settings.GameWidth, 2 * height);
            InitMainOptions();
            InitHelpOptions();
            InitNewGameOptions();
            InitNewGameAdvancedOptions();
            InitSettingsOptions();
        }

        #region initilization
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
        protected override void InitControls(int width, int height)
        {
            base.InitControls(width, height);
            Controls.Position = new Coord(0, height);
            Controls.Theme = new MenuControlsTheme();
            Controls.ThemeColors = ThemeColors.Menu;

            ControlledGameObject = MenuSelector(Controls.Position);
            Controls.Children.Add(ControlledGameObject);
        }
        private void InitMainOptions()
        {
            MainOptions = new MenuPanel(width, height);
            MainOptions.Add(MakeButton("Continue", ContinueButton_Click));
            MainOptions.Add(MakeButton("New Game", ContinueButton_Click));
            MainOptions.Add(MakeButton("Settings", ContinueButton_Click));
            MainOptions.Add(MakeButton("Help", ContinueButton_Click));

            int i = 0;
            foreach(var button in MainOptions)
            {
                button.IsVisible = true;
                button.IsEnabled = true;
                button.Position = new Coord(width, 2 + (i * 2));
                i++;
            }
        } 
        private void InitHelpOptions()
        {
            HelpOptions = new HelpConsole();
            HelpOptions.Position = new Coord(0, height);
            HelpOptions.IsVisible = false;
            Controls.Children.Add(HelpOptions);
        }        
        
        private void InitNewGameOptions()
        {
            NewGameOptions = new MenuPanel(width, height);

            NewGameOptions.Add(MakeButton("Quickstart", QuickStartButton_Click));
            NewGameOptions.Add(MakeButton("Advanced", AdvancedStartButton_Click));
            int i = 0;
            foreach(var button in NewGameOptions)
            {
                button.Position = new Coord(0, 2 + 2 * i);
                button.IsVisible = false;
                i++;
            }
        }
        private void InitNewGameAdvancedOptions()
        {
            NewGameAdvancedOptions = new ControlsConsole(width, height);
            NewGameAdvancedOptions.Add(MakeButton("Start", AdvancedStartButton_Click)); 
        }
        private void InitSettingsOptions()
        {
            SettingsOptions = new MenuPanel(width, height * 2);
            //foreach variable in Settings...?
            int count = 0;
            PropertyInfo[] properties = typeof(Settings).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                Button btn = MakeButton(property.Name, Except); //temporary
                btn.Position = new Coord(0, 2 + 2 * count);
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
