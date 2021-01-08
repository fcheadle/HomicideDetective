using SadConsole;
using SadConsole.Controls;

namespace HomicideDetective.Old.UI
{
    interface IDisplay
    {
        Window Window { get; }
        Button MaximizeButton { get; }
    }
}
