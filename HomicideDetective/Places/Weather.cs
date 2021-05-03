﻿// using System;
// using System.Collections.Generic;
// using HomicideDetective.Mysteries;
// using HomicideDetective.UserInterface;
// using SadRogue.Integration;
// using SadRogue.Integration.Components;
//
// namespace HomicideDetective.Places
// {
//     public class Weather : RogueLikeComponentBase, IDetailed
//     {
//         private Scene _place;
//         public Scene Place => _place;
//         public string Name { get; }
//         public string Description { get; }
//         public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;
//
//         //F(x, y, t) // f of xyt
//         private Func<int, int, TimeSpan, double> Fxyt
//             => (x, y, t) =>
//                 2 * Math.Cos(-t.TotalSeconds + Math.Sqrt((x + 180) * (x + 180) / 444 + (y + 90) * (y + 90) / 444));
//
//         public Weather(Scene place) : base(true, false, false, false)
//         {
//             Name = "Weather";
//             Description = $"The weather.";
//             _place = place;
//         }
//
//         public List<string> Details => new List<string>() {Description, $"Time Elapsed: {Elapsed.ToString()}"};
//         public void ProcessTimeUnit()
//         {
//             Elapsed += TimeSpan.FromMilliseconds(100);
//             BlowWind();
//         }
//
//         private void BlowWind()
//         {
//             for (int x = 0; x < _place.Width; x++)
//             {
//                 for (int y = 0; y < _place.Height; y++)
//                 {
//                     RogueLikeCell? terrain = _place.GetTerrainAt<RogueLikeCell>((x, y));
//                     var component = terrain?.GoRogueComponents.GetFirstOrDefault<AnimatingGlyph>();
//                     if (component != null)
//                     {
//                         double z = Fxyt(x, y, Elapsed);
//                         if (z > 1.75 || z < -1.75)
//                             component.Start();
//
//                         component.ProcessTimeUnit();
//                     }
//                 }
//             }
//         }
//     }
// }
