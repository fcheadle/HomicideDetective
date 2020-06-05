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
    }
}
