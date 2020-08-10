using SadConsole.Controls;
using System;
using System.Collections.Generic;

namespace Engine.UI
{
    public class HelpPanel : MenuPanel
    {
        public TextBox SearchField { get; private set; }
        public DrawingSurface LibraryDisplayer { get; private set; }
        public Dictionary<Enum, string> Documentation { get; private set; }
        public HelpPanel() : base(Game.Settings.GameWidth, Game.Settings.GameHeight)
        {
            SearchField = new TextBox(Width / 2);
            LibraryDisplayer = new DrawingSurface(Width, Height - 4);
            Theme = new MenuControlsTheme();
            ThemeColors = UI.ThemeColors.Menu;
            //LibraryDisplayer.Theme = Theme;
            Documentation = new Dictionary<Enum, string>();
            foreach(var keyBinding in Game.Settings.KeyBindings)
            {
                Documentation.Add(keyBinding.Key, keyBinding.Value.ToString());
                Documentation.Add(keyBinding.Value, keyBinding.Key.ToString());
            }
            LibraryDisplayer.Theme = new MenuButtonTheme();
            LibraryDisplayer.ThemeColors = ThemeColors;
            IsVisible = false;
            Add(SearchField);
            Add(LibraryDisplayer);//currently causing some exceptions that are hard to debug
        }
    }
}
