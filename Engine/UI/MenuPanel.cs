using Engine.Components.UI;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.UI
{
    public class MenuPanel : ControlsConsole
    {
        public MenuPanel(int width, int height) : base(width, height)
        {
            Theme = new MenuControlsTheme();
            ThemeColors = Engine.UI.ThemeColors.Menu;
        }
    }
}
