using System;
using HomicideDetective.Mysteries;
using HomicideDetective.UserInterface;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;

namespace HomicideDetective.Places
{
    public class Weather : RogueLikeComponentBase, IDetailed
    {
        private Place _place;
        public Place Place => _place;
        public string Name { get; }
        public string Description { get; }
        public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;

        //F(x, y, t) // f of xyt
        private Func<int, int, TimeSpan, double> Fxyt
            => (x, y, t) =>
                2 * Math.Cos(-t.TotalSeconds + Math.Sqrt((x + 180) * (x + 180) / 444 + (y + 90) * (y + 90) / 444));

        public Weather(Place place) : base(true, false, false, false)
        {
            Name = "Weather";
            Description = $"The weather of {place.Name}";
            _place = place;
        }

        public string[] Details => new[] {Description, $"Time Elapsed: {Elapsed.ToString()}"};
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
                    RogueLikeCell? terrain = _place.GetTerrainAt<RogueLikeCell>((x, y));
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
