using System;
using System.Collections.Generic;
using GoRogue;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Integration.Maps;
// ReSharper disable PossibleLossOfFraction

namespace HomicideDetective.Places.Weather
{
    /// <summary>
    /// The weather component added to
    /// </summary>
    public class WeatherController : RogueLikeComponentBase
    {
        private RogueLikeMap _map;
        public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;
        
        //the possible ways for wind to blow
        public static List<Func<int, int, TimeSpan, double>> WindPatterns = new List<Func<int, int, TimeSpan, double>>()
        {
            (x,y,t) => Math.Tan(y*.625 + x*1.75 - t.TotalSeconds / 100) % 2.01,//natural
            (x,y,t) => -Math.Cos(x - t.TotalSeconds) - Math.Sin(y + t.TotalSeconds), //acceptable, imperfect
            ////(x,y,t) => Math.Sin(x + t.TotalMilliseconds / 111) + Math.Cos(y - t.TotalMilliseconds / 555), //too busy
            ////(x,y,t) => 1.5 * Math.Sin(x / 3.33 + t.TotalSeconds) + Math.Cos(y * 3.33 + t.TotalSeconds), //too busy
            ////(x,y,t) => Math.Sin(x + t.TotalMilliseconds / 444) + Math.Cos(y + t.TotalMilliseconds / 333),//too math-y
            //// (x,y,t) => -Math.Cos(x * 3.45 - t.TotalMilliseconds / 777) - Math.Sin(y*0.77 - t.TotalMilliseconds / 77), //east waves - beautiful, but too busy
            //// (x,y,t) => Math.Sin(x - t.TotalMilliseconds / 650) + Math.Cos(y - t.TotalMilliseconds / 325), //too math-y
        };
        
        //F(x, y, t) // f of xyt
        private Func<int, int, TimeSpan, double> WindPattern;

        public WeatherController(RogueLikeMap place) : base(true, false, false, false)
        {
            _map = place;
            WindPattern = WindPatterns.RandomItem();
        }

        public void ProcessTimeUnit()
        {
            Elapsed += TimeSpan.FromMilliseconds(100);
            BlowWind();
            if(Elapsed.TotalMilliseconds % 1000 <= 100)
                MakeWaves();
        }

        private void BlowWind()
        {
            //iterate over the whole map
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    //if the cell has a BlowsInWind component...
                    RogueLikeCell? terrain = _map.GetTerrainAt<RogueLikeCell>((x, y));
                    var component = terrain?.GoRogueComponents.GetFirstOrDefault<BlowsInWind>();
                    if (component != null)
                    {
                        //perform the function and blow wind on a subset of tiles
                        double z = WindPattern(x, y, Elapsed);
                        if (z > 1.75 || z < -1.75)
                            component.Start();

                        component.ProcessTimeUnit();
                    }
                }
            }
        }
        private void MakeWaves()
        {
            var pond = _map.GoRogueComponents.GetFirstOrDefault<BodyOfWater>();
            if (pond is not null)
            {
                pond.DetermineNextStates();
                foreach (var cell in pond.Cells)
                {
                    var position = cell.Position - pond.Body.Position;
                    var waves = cell.GoRogueComponents.GetFirst<MovesInWaves>();
                    waves.State = pond.NextState[position];
                    waves.SetAppearance();
                    pond.CurrentState[position] = pond.NextState[position];
                }
            }
        }
    }
}
