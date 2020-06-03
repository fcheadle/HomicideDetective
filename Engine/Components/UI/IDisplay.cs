using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components.UI
{
    interface IDisplay
    {
        ScrollingConsole Console { get; }
        Window Window { get; }
    }
}
