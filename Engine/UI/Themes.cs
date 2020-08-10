using System;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace Engine.UI
{
    public class PaperWindowTheme : WindowTheme
    {
        public PaperWindowTheme()
        {
            //SetForeground(Color.Blue);
            //SetBackground(Color.Tan);
            ModalTint = Color.Tan;
            FillStyle = new Cell(Color.Blue, Color.Tan, '_');
            BorderLineStyle = new[]{
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
            TitleAreaY = 0;
        }
    }

    public class PaperButtonTheme : ButtonTheme
    {
        public PaperButtonTheme()
        {
            SetForeground(Color.Blue);
            SetBackground(Color.Tan); 
            
        }
        
        public override ThemeBase Clone()
        {
            return new PaperButtonTheme();
        }

        public override void UpdateAndDraw(ControlBase control, TimeSpan time)
        {
            //throw new NotImplementedException();
        }
    }

    class MenuControlsTheme : ControlsConsoleTheme
    {
        //private readonly Color _semitransparent = new Color(Color.Black, 128);
        public MenuControlsTheme()
        {
            FillStyle = new Cell(Color.Transparent, Color.Black);
        }
    }
    class MenuButtonTheme : ButtonTheme
    {
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
