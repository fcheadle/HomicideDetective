using SadConsole;
using SadConsole.Controls;

namespace Engine.UI
{
    interface IDisplay
    {
        Window Window { get; }
        Button MaximizeButton { get; }
    }
}
