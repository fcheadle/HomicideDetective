using SadConsole;
using SadConsole.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components.UI
{
    interface IDisplay
    {
        Window Window { get; }
        Button MaximizeButton{ get; }
    }
}
