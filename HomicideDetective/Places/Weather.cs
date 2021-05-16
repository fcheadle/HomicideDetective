using System;
using System.Collections.Generic;
using GoRogue;
using HomicideDetective.Mysteries;
using HomicideDetective.UserInterface;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Integration.Maps;

namespace HomicideDetective.Places
{
    public class Weather : RogueLikeComponentBase, IDetailed
    {
        private RogueLikeMap _map;
        public string Name { get; }
        public string Description { get; }
        public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;
        public static List<Func<int, int, TimeSpan, double>> WindPatterns = new List<Func<int, int, TimeSpan, double>>()
        {
            (x,y,t) => 2 * Math.Cos(-t.TotalMilliseconds / 1111 + Math.Sqrt((x-70)*(x-70) / 111 + (y-90)*(y-90) / 111)), //concentric waves from (90,90)
            (x,y,t) => Math.Sin(x + t.TotalMilliseconds / 111) + Math.Cos(y - t.TotalMilliseconds / 555), //waves zig-zagging south
            (x,y,t) => Math.Cos(t.TotalMilliseconds / 666 + Math.Sqrt(Math.Abs((x * y) + y))), //waves going northwest
            (x,y,t) => 1.5 * Math.Sin(x / 3.33 + t.TotalSeconds) + Math.Cos(y * 3.33 + t.TotalSeconds), //bubbles northwest
            (x,y,t) => Math.Sin(x + t.TotalMilliseconds / 444) + Math.Cos(y + t.TotalMilliseconds / 333),//bubbles going northwest
            (x,y,t) => -Math.Cos(x - t.TotalSeconds) - Math.Sin(y + t.TotalSeconds), //NW bubbles - so so
            (x,y,t) => Math.Tan(y*.625 + x*1.75 - t.TotalSeconds / 100) % 2.01,//odd lines marching west, unbound
            (x,y,t) => -Math.Cos(x * 3.45 - t.TotalMilliseconds / 777) - Math.Sin(y*0.77 - t.TotalMilliseconds / 77), //east waves - beautiful
            (x,y,t) => Math.Sin(x - t.TotalMilliseconds / 650) + Math.Cos(y - t.TotalMilliseconds / 325), //bubbles going south-southeast - beautiful
            (x,y,t) => 2 * Math.Cos(-t.TotalSeconds + Math.Sqrt((x+180)*(x+180)/444 + (y+90)*(y+90)/444)), // waves going southeast - beautiful
            (x,y,t) => 2 * Math.Cos(-t.TotalSeconds + Math.Sqrt(x*x/66 + (y+13)*(y+80)/222)), // waves going southeast - beautiful
            (x,y,t) => 2 * Math.Cos(-t.TotalSeconds + Math.Sqrt((x+222)*(x+222)/222 + (y+15)*(y+15)/66)), // waves going southeast - beautiful
            (x,y,t) => 2 * Math.Cos(t.TotalSeconds + Math.Sqrt((x-2222)*(x-2222)/222 + (y-1000)*(y-1000)/66)), // waves going southeast - beautiful
            (x,y,t) => 2 * Math.Cos(t.TotalSeconds + Math.Sqrt((x-1111)*(x-1111)/33 + (y-555)*(y-555)/99)), // waves going southeast - beautiful  
            (x,y,t) => 2 * Math.Cos(1.75*t.TotalSeconds + Math.Sqrt((x-2222)*(x-2222)/33 + (y-1000)*(y-1000)/99)), // waves going southeast - beautiful
            (x,y,t) => 2 * Math.Sin(x * (y / 8) + y * Math.Sin(y / (x+1)) + t.TotalSeconds),//bizarre and cool
        };
        //F(x, y, t) // f of xyt
        private Func<int, int, TimeSpan, double> Fxyt;

        public Weather(RogueLikeMap place) : base(true, false, false, false)
        {
            Name = "Weather";
            Description = $"The weather.";
            _map = place;
            Fxyt = WindPatterns.RandomItem();
        }

        public List<string> Details => new List<string>() {Description, $"Time Elapsed: {Elapsed.ToString()}"};
        public void ProcessTimeUnit()
        {
            Elapsed += TimeSpan.FromMilliseconds(100);
            BlowWind();
            if (Elapsed.Milliseconds % 10000 > 9000)
            {
                Fxyt = WindPatterns.RandomItem();
            }
        }

        private void BlowWind()
        {
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    RogueLikeCell? terrain = _map.GetTerrainAt<RogueLikeCell>((x, y));
                    var component = terrain?.GoRogueComponents.GetFirstOrDefault<AnimatingGlyph>();
                    if (component != null)
                    {
                        double z = Fxyt(x, y, Elapsed);
                        if (z > 1.75 || z < -1.75)
                            component.Start();

                        component.ProcessTimeUnit();
                    }
                }
            }
        }
    }
}
