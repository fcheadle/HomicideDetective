using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components.UI
{
    class PaperTheme : WindowTheme
    {
        public PaperTheme()
        {
            ModalTint = Color.Tan;
            FillStyle = new Cell(Color.Blue, Color.Tan, '_');
            //BorderLineStyle = CellSurface.ConnectedLineThick;
            BorderLineStyle = CellSurface.ConnectedLineThick;
            TitleAreaY = 0;
        }

        public static Colors Colors => new Colors()
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
