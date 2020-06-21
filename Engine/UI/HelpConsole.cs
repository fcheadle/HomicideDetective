using Engine.Components.UI;
using Engine.Utilities;
using SadConsole;
using SadConsole.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.UI
{
    public class HelpConsole : ControlsConsole
    {
        public TextBox SearchField { get; private set; }
        public DrawingSurface LibraryDisplayer { get; private set; }
        public Dictionary<Enum, string> Documentation { get; private set; }
        public HelpConsole() : base(Game.Settings.GameWidth, Game.Settings.GameHeight)
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

            IsVisible = false;
            Add(SearchField);
            //Add(LibraryDisplayer);//currently causing some exceptions that are hard to debug
        }
    }
}
