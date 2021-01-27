using System;
using SadConsole.Components;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;

namespace HomicideDetective.Places.Components
{
    //Requires significant refactor on alpha
    public class WeatherComponent : RogueLikeComponentBase, IHaveDetails
    {
        private Place _place;
        public string Name { get; }
        public string Description { get; }
        public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;
        private Func<int, int, TimeSpan, double> Fxyt 
            => (x, y, t) => 2 * Math.Cos(-t.TotalSeconds + Math.Sqrt((x + 180) * (x + 180) / 444 + (y + 90) * (y + 90) / 444));
        
        public WeatherComponent(Place place) : base(true, false, false, false)
        {
            _place = place;
            Timer timer = new Timer(TimeSpan.FromMilliseconds(100))
            {
                Repeat = true,
                IsPaused = false,
            };
            
            timer.TimerElapsed += (sender, args) =>
            {
                ProcessTimeUnit();
            };
            
            place.AllComponents.Add(timer);
        }
        
        public string[] GetDetails()
        {
            string[] answer =
            {
                "This is the weather component. It's parent is a map, not a regular entity."
            };
            return answer;
        }

        public void ProcessTimeUnit()
        {
            Elapsed += TimeSpan.FromMilliseconds(100);
            BlowWind();
        }

        private void BlowWind()
        {
            for (int x = 0; x < _place.Width; x++)
            {
                for (int y = 0; y < _place.Height; y++)
                {
                    RogueLikeCell terrain = _place.GetTerrainAt<RogueLikeCell>((x, y));
                    if (terrain != null)
                    {
                        var component = terrain.GoRogueComponents.GetFirstOrDefault<ChangingGlyphComponent>();
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
}
