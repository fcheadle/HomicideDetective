using Engine.Components.UI;
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
            middlePosition = new Coord(middlePositionX, middlePositionY);
            openSubMenuOffset = new Coord(oneSubMenuOpenOffsetX, middlePositionY);
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
            MenuRenderer.IsVisible = true;
            MenuRenderer.IsFocused = false;
            Children.Add(MenuRenderer);
        }

        private void CreateTitleConsole()
        {
            //takes up the top 1/3 of the screen
            TitleConsole = new ScrollingConsole(Game.Settings.GameWidth, height);
            TitleConsole.IsVisible = true;
            
            TitleConsole.Position = new Coord(0, 0);
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
            MainOptionsConsole = new ControlsConsole(width, height);
            MainOptionsConsole.Position = middlePosition;
            MainOptionsConsole.Fill(Color.Blue, Color.Tan, '_');//for debugging purposes
            MainOptionsConsole.Theme = new MenuTheme();
            MainOptionsConsole.ThemeColors = ThemeColors.Menu;
            //add buttons
            //if( /* there is a saved or current game */)
            //{
            Button cont= MenuButton("Continue", ContinueButton_Click);
            cont.Position = new Coord(2, 2);
            cont.IsVisible = true;
            MainOptionsConsole.Add(cont);
            //}

            Button newGame = MenuButton("New Game", ContinueButton_Click);
            newGame.Position = new Coord(2, 4);
            newGame.IsVisible = true;
            MainOptionsConsole.Add(newGame);

            Button settings = MenuButton("Settings", ContinueButton_Click);
            settings.Position = new Coord(2, 6);
            settings.IsVisible = true;
            MainOptionsConsole.Add(settings);

            Button help = MenuButton("Help", ContinueButton_Click);
            help.Position = new Coord(2, 8);
            help.IsVisible = true;
            MainOptionsConsole.Add(help);

            MainOptionsConsole.Children.Add(MenuSelector(MainOptionsConsole.Position + new Coord(0,2)));
            MainOptionsConsole.IsVisible = true;
            MainOptionsConsole.UseKeyboard = true;
            MainOptionsConsole.UseMouse = true;
            
            Selector = MenuSelector(MainOptionsConsole.Controls[0].Position);
            MainOptionsConsole.Children.Add(Selector);
            MenuRenderer.Children.Add(MainOptionsConsole);
        }
        private void InitHelpOptions()
        {
            HelpOptionsConsole = new ControlsConsole(width, height);
            HelpOptionsConsole.Theme = new MenuTheme();
            HelpOptionsConsole.ThemeColors = ThemeColors.Menu;
            HelpOptionsConsole.Position = new Coord(Game.Settings.GameWidth / 2, middlePositionY);
            HelpOptionsConsole.IsVisible = false;
            MenuRenderer.Children.Add(HelpOptionsConsole);
            //text box.... TODO
        }

        private void InitSettingsOptions()
        {
            SettingsOptionsConsole = new ControlsConsole(width, height);
            SettingsOptionsConsole.Theme = new MenuTheme();
            SettingsOptionsConsole.ThemeColors = ThemeColors.Menu;
            SettingsOptionsConsole.Position = new Coord(Game.Settings.GameWidth / 2, middlePositionY);
            SettingsOptionsConsole.IsVisible = false;
            MenuRenderer.Children.Add(SettingsOptionsConsole);
            //foreach variable in Settings... maybe use reflection for that so I can never forget going forward?
        }

        private void InitNewGameOptions()
        {
            NewGameOptionsConsole = new ControlsConsole(width, height);
            NewGameOptionsConsole.Theme = new MenuTheme();
            NewGameOptionsConsole.ThemeColors = ThemeColors.Menu;
            NewGameOptionsConsole.Position = new Coord(Game.Settings.GameWidth / 2, middlePositionY);
            NewGameOptionsConsole.IsVisible = false;
            Button quick = MenuButton("Quickstart", QuickStartButton_Click);
            quick.Position = new Coord(0, 2);
            Button advanced = MenuButton("Advanced", AdvancedStartButton_Click);
            advanced.Position = new Coord(0, 4);

            NewGameOptionsConsole.Add(quick);
            NewGameOptionsConsole.Add(advanced);

            MenuRenderer.Children.Add(NewGameOptionsConsole);

            NewGameAdvancedOptionsConsole = new ControlsConsole(width, height);
            NewGameAdvancedOptionsConsole.Theme = new MenuTheme();
            NewGameAdvancedOptionsConsole.ThemeColors = ThemeColors.Menu;
            NewGameAdvancedOptionsConsole.Position = new Coord(rightPositionX, middlePositionY);
            NewGameAdvancedOptionsConsole.IsVisible = false;
            Button start = MenuButton("Start", AdvancedStartButton_Click);
            start.Position = new Coord(0, 2);
            NewGameAdvancedOptionsConsole.Add(start);

            MenuRenderer.Children.Add(NewGameAdvancedOptionsConsole);
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
            btn.Theme = new MenuButtonTheme();
            btn.ThemeColors = ThemeColors.Menu;
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
