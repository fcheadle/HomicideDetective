using SadConsole.Controls;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.UI
{
    class Theme : SadConsole.Themes.ThemeBase
    {
        public Theme()
        {
        }

        public override ThemeBase Clone()
        {
            throw new NotImplementedException();
        }

        public override void UpdateAndDraw(ControlBase control, TimeSpan time)
        {
            throw new NotImplementedException();
        }
    }
}
