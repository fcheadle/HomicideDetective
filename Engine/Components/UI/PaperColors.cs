using Microsoft.Xna.Framework;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components.UI
{
    public static class ThemeColor
    {
        public static Colors Paper => new Colors()
        {
            TitleText = Color.Black,
            Lines = Color.Blue,
            TextBright = Color.LightBlue,
            Text = Color.Blue,
            TextSelected = Color.Cyan,
            TextSelectedDark = Color.DarkBlue,
            TextLight = Color.White,
            TextDark = Color.Black,
            TextFocused = Color.Blue,
            ControlBack = Color.Tan,
            ControlBackLight = Color.Khaki,
            ControlBackSelected = Color.DarkKhaki,
            ControlBackDark = Color.SaddleBrown,
            ControlHostBack = Color.Tan,
            ControlHostFore = Color.Blue,
        };
    }
}
