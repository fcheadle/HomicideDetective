using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using System;

namespace HomicideDetective.Old.UI
{
    //refactor out and only support one UI, which is itself a ScreenSurface
    public class UserInterface : ContainerConsole
    {
        public virtual ScrollingConsole Display { get; protected set; }    
        public virtual BasicEntity Player { get; protected set; }
        public virtual ControlsConsole Controls { get; protected set; }
        public virtual ThemeBase Theme { get; protected set; }
        public virtual Colors Colors { get; protected set; }

        #region initilization 
        protected virtual void InitDisplay()
        {
            Display = new ScrollingConsole(Game.Settings.GameWidth, Game.Settings.GameHeight);
            Display.IsVisible = true;
            Display.IsFocused = false;
            Children.Add(Display);
        }

        protected virtual void InitControls(int width, int height)
        {
            Controls = new ControlsConsole(width, height);
            Controls.IsVisible = true;
            Controls.IsFocused = false;
            Children.Add(Controls);
        }
        protected virtual Button MakeButton(string name, EventHandler onClick)
        {
            Button btn = new Button(name.Length + 2);
            btn.Name = name;
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

        #region utilties
        public virtual void Hide()
        {
            IsVisible = false;
            IsFocused = false;
        }
        public virtual void Show()
        {
            IsVisible = true;
            IsFocused = true;
        }
        #endregion
    }
}
