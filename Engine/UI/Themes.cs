using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components.UI
{
    class PaperWindowTheme : WindowTheme
    {
        public PaperWindowTheme()
        {
            ModalTint = Color.Tan;
            FillStyle = new Cell(Color.Blue, Color.Tan, '_');
            BorderLineStyle = new int[]{
                0,
                0,
                0,
                '_',
                0,
                '_',
                '_',
                '_',
                '_',
                0,
                0,
                0,
                0,
            };
            //cutom border:
            TitleAreaY = 0;
        }
    }

    internal class PaperButtonTheme : ButtonTheme
    {
        public PaperButtonTheme()
        {

        }

        public override ThemeBase Clone()
        {
            throw new NotImplementedException();
        }

        public override void UpdateAndDraw(ControlBase control, TimeSpan time)
        {
            //throw new NotImplementedException();
        }
    }

    class MenuTheme : ControlsConsoleTheme
    {
        private readonly Color semitransparent = new Color(Color.Black, 128);
        public MenuTheme()
        {
            this.FillStyle = new Cell(Color.Transparent, semitransparent);
        }
    }
    class MenuButtonTheme : ButtonTheme
    {

        public MenuButtonTheme()
        {

        }

        public override ThemeBase Clone()
        {
            throw new NotImplementedException();
        }

        public override void UpdateAndDraw(ControlBase control, TimeSpan time)
        {
            //throw new NotImplementedException();
        }
    }
}
