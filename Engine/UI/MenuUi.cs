using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.UI
{
    public class MenuUi : ContainerConsole
    {
        public ScrollingConsole MenuRenderer { get; private set; } //the main box that covers the screen
        public BasicEntity Selector { get; private set; } //the cursor
        public ScrollingConsole TitleConsole { get; private set; } // H O M I C I D E    D E T E C E T I V E 
        public ControlsConsole MainOptionsConsole { get; private set; } //the main menu that appears when you pause
        public ControlsConsole HelpOptionsConsole { get; private set; } //console that holds the search box / cheats menu
        public ControlsConsole NewGameOptionsConsole { get; private set; } //quickstart / advanced options
        public ControlsConsole NewGameAdvancedOptionsConsole { get; private set; } //quickstart / advanced options
        public ControlsConsole SettingsOptionsConsole { get; private set; }

        public MenuUi()
        {
            UseMouse = true;
            UseKeyboard = true;
            CreateMenuRenderer();
            CreateTitleConsole();
            InitMainOptions();
            InitHelpOptions();
            InitSettingsOptions();
            InitNewGameOptions();
            Hide();
        }

        #region initilization 
        private void CreateMenuRenderer()
        {
            MenuRenderer = new ControlsConsole(Game.Settings.GameWidth, Game.Settings.GameHeight);
            MenuRenderer.IsVisible = false;
            MenuRenderer.IsFocused = false;
            Children.Add(MenuRenderer);
        }

        private void CreateTitleConsole()
        {
            //takes up the top 1/3 of the screen
            TitleConsole = new ControlsConsole(Game.Settings.GameWidth, Game.Settings.GameHeight / 3);
            TitleConsole.FillWithRandomGarbage();//for debugging purposes
            TitleConsole.Print(2, 2, "!!! Homicide Detective !!!", Color.White, Color.Black);
            MenuRenderer.Children.Add(TitleConsole);
        }

        private BasicEntity MenuSelector(Coord position)
        {
            BasicEntity cursor = new BasicEntity(Color.White, Color.Black, 16, position, 1, true, true);
            return cursor;
        }

        private void InitMainOptions()
        {
            //the middle 1/3 by 1/3 section of the screen
            MainOptionsConsole = new ControlsConsole(Game.Settings.GameWidth / 3, Game.Settings.GameHeight / 3);
            MainOptionsConsole.Position = new Coord(0, Game.Settings.GameHeight / 3);
            MainOptionsConsole.Fill(Color.Blue, Color.Tan, '_');//for debugging purposes
            //add buttons
            //if( /* there is a saved or current game */)
            //{
            Button cont= MenuButton("Continue", ContinueButton_Click);
            cont.Position = new Coord(2, 2);
            MainOptionsConsole.Add(cont);
            //}

            Button newGame = MenuButton("New Game", ContinueButton_Click);
            newGame.Position = new Coord(2, 4);
            MainOptionsConsole.Add(newGame);

            Button settings = MenuButton("Settings", ContinueButton_Click);
            settings.Position = new Coord(2, 6);
            MainOptionsConsole.Add(settings);

            Button help = MenuButton("Help", ContinueButton_Click);
            help.Position = new Coord(2, 8);
            help.IsVisible = true;
            MainOptionsConsole.Add(help);

            MainOptionsConsole.Children.Add(MenuSelector(MainOptionsConsole.Position + new Coord(0,2)));
            MainOptionsConsole.IsVisible = true;
            MainOptionsConsole.UseKeyboard = true;
            MainOptionsConsole.UseMouse = true;
            MenuRenderer.Children.Add(MainOptionsConsole);
            Selector = MenuSelector(MainOptionsConsole.Controls[0].Position);
            MainOptionsConsole.Children.Add(Selector);
        }
        private void InitHelpOptions()
        {
            HelpOptionsConsole = new ControlsConsole(Game.Settings.GameWidth, Game.Settings.GameHeight);
            //text box.... TODO
        }

        private void InitSettingsOptions()
        {
            SettingsOptionsConsole = new ControlsConsole(Game.Settings.GameWidth, Game.Settings.GameHeight);
            //foreach variable in Settings... maybe use reflection for that so I can never forget going forward?
        }

        private void InitNewGameOptions()
        {
            NewGameOptionsConsole = new ControlsConsole(Game.Settings.GameWidth, Game.Settings.GameHeight);
            Button quick = MenuButton("Quickstart", QuickStartButton_Click);
            quick.Position = new Coord(0, 2);
            Button advanced = MenuButton("Advanced", AdvancedStartButton_Click);
            advanced.Position = new Coord(0, 4);

            NewGameOptionsConsole.Add(quick);
            NewGameOptionsConsole.Add(advanced);

            Children.Add(NewGameOptionsConsole);

            NewGameAdvancedOptionsConsole = new ControlsConsole(Game.Settings.GameWidth, Game.Settings.GameHeight);
            Button start = MenuButton("Start", AdvancedStartButton_Click);
            start.Position = new Coord(0, 2);
            NewGameAdvancedOptionsConsole.Add(start);

            Children.Add(NewGameAdvancedOptionsConsole);
        }

        private void AdvancedStartButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void QuickStartButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region utilties
        public void Hide()
        {
            IsVisible = false;
            IsFocused = false;
            Global.CurrentScreen = Game.UIManager;
        }
        public void Show()
        {
            IsVisible = true;
            IsFocused = true;
            Global.CurrentScreen = this;
        }
        public void Toggle()
        {
            if (IsVisible)
                Hide();
            else
                Show();
        }
        #endregion

        #region event handlers
        private void HelpButton_Click(object sender, EventArgs e)
        {
            //open help console
            HelpOptionsConsole.IsVisible = true;
            HelpOptionsConsole.IsFocused = true;
            MainOptionsConsole.Position = new Coord(Game.Settings.GameWidth / 3, Game.Settings.GameHeight / 3);
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
            //close menu
            Global.CurrentScreen = Game.UIManager;
        }

        private Button MenuButton(string name, EventHandler onClick)
        {
            Button btn = new Button(name.Length + 2);
            btn.IsVisible = true;
            btn.IsEnabled = true;
            btn.Surface = new CellSurface(btn.Width, 1);
            btn.Surface.Print(2, 0, name);
            btn.Click += onClick;
            return btn;
        }
        #endregion
    }
}
