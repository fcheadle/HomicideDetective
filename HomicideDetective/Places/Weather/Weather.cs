using System;
using SadRogue.Integration.Components;
using SadRogue.Integration.Maps;
using SadRogue.Primitives.GridViews;

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
        private int _windSpeed = 0;

        public WeatherController(RogueLikeMap place) : base(true, false, false, false)
        {
            _map = place;
            _windSpeed = new Random().Next(75, 150);
        }

        public void ProcessTimeUnit()
        {
            Elapsed += TimeSpan.FromMilliseconds(100);
            if(Elapsed.TotalMilliseconds % 1000 <= 100)
                MakeWaves();
            if (Elapsed.TotalMilliseconds % 1000 <= _windSpeed)
                BlowWind();
        }

        private void BlowWind()
        {
            var plains = _map.GoRogueComponents.GetFirstOrDefault<WindyPlain>();
            if (plains is not null)
            {
                plains.DetermineNextStates();
                foreach (var cell in plains.Cells)
                {
                    var position = cell.Position - plains.Body.Position;
                    var wind = cell.GoRogueComponents.GetFirst<BlowsInWind>();
                    if(_map.Contains(position))
                    {
                        wind.State = plains.NextState[position];
                        wind.SetAppearance();
                        plains.CurrentState[position] = plains.NextState[position];
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
