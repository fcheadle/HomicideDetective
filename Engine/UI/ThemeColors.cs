using Microsoft.Xna.Framework;
using SadConsole.Themes;

namespace Engine.UI
{
    public static class ThemeColors
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

        public static Colors Clear => new Colors()
        {
            TitleText = Color.Black,
            Lines = Color.Transparent,
            TextBright = Color.White,
            Text = Color.LightGray,
            TextSelected = Color.LightBlue,
            TextSelectedDark = Color.DarkBlue,
            TextLight = Color.LightGray,
            TextDark = Color.DarkGray,
            TextFocused = Color.Blue,
            ControlBack = Color.Transparent,
            ControlBackLight = Color.Transparent,
            ControlBackSelected = Color.Transparent,
            ControlBackDark = Color.Transparent,
            ControlHostBack = Color.Transparent,
            ControlHostFore = Color.Black,
        };

        public static Colors Menu => new Colors()
        {
            TitleText = Color.White,
            Lines = Color.White,
            TextBright = Color.LightGray,
            Text = Color.Gray,
            TextSelected = Color.White,
            TextSelectedDark = Color.LightSkyBlue,
            TextLight = Color.LightGray,
            TextDark = Color.DarkGray,
            TextFocused = Color.LightGray,
            ControlBack = Color.Black,
            ControlBackLight = Color.Black,
            ControlBackSelected = Color.Black,
            ControlBackDark = Color.Black,
            ControlHostBack = Color.Black,
            ControlHostFore = Color.White,
        };
    }
}
