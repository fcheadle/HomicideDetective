// using Engine.Utilities;
// using GoRogue;
// using SadConsole;
// using SadConsole.Controls;
// using SadConsole.Input;
// // ReSharper disable UnusedParameter.Local
// // ReSharper disable UnusedMember.Local
//
// namespace Engine.UI.Components
// {
//     public class MagnifyingGlassComponent : ComponentBase
//     {
//         const int Width = 3;
//         const int Height = 3;
//         //private Window _lookingGlass;
//         public DrawingSurface Surface;
//         public GameAction Purpose;
//         public MagnifyingGlassComponent(BasicEntity parent, Coord position, GameAction purpose = GameAction.LookAtEverythingInSquare): base(false, true, true, true)
//         {
//             Parent = parent;
//             //_lookingGlass = new Window(3, 3);
//             //CellSurface surface = new CellSurface(3, 3);
//             //surface.Position = new Coord(-1, -1);
//             //surface.OnDraw = (ds) =>
//             //{
//             //    ds.Surface.Effects.UpdateEffects(Global.GameTimeElapsedUpdate);
//             //    ds.Surface.Fill(Color.Gold, Color.Transparent, 0);
//             //    ds.Surface.SetGlyph(0, 1, '(');
//             //    ds.Surface.SetGlyph(2, 1, ')');
//             //    ds.Surface.SetGlyph(2, 2, '\\');
//             //};
//             //surface.MouseMove += MoveWithMouse;
//             //surface.MouseButtonClicked += MouseButtonClicked;
//             //surface.IsFocused = true;
//             //surface.IsVisible = true;
//             //Game.Container.Children.Add(surface);
//         }
//
//         private void MouseButtonClicked(object sender, MouseEventArgs mouse)
//         {
//             //Take action on person/place/thing?
//
//             //destroy the magnifying glass
//             Parent.RemoveComponent(this);
//         }
//
//         private void MoveWithMouse(object sender, MouseEventArgs mouse)
//         {
//             Surface.Position = mouse.MouseState.CellPosition - new Coord(1,1);
//             Surface.IsVisible = true;
//             Surface.IsFocused = true;
//         }
//
//         public override string[] GetDetails()
//         {
//             return new[]
//             {
//                 "Just a magnifying glass.",
//                 "You shouldn't be seeing this message."
//             };
//         }
//
//         public override void ProcessTimeUnit()
//         {
//             //do nothing?
//         }
//     }
// }
