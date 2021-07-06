using System;
using GoRogue.Components.ParentAware;
using SadRogue.Integration.Components;
using SadRogue.Integration.Maps;
using SadRogue.Primitives.GridViews;
// ReSharper disable PossibleLossOfFraction

namespace HomicideDetective.Places.Weather
{
    /// <summary>
    /// The weather component added to
    /// </summary>
    public class WeatherController : ParentAwareComponentBase<RogueLikeMap>
    {
        public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;
        private int _windSpeed = 0;

        public WeatherController()
        {
            _windSpeed = new Random().Next(50, 125);
        }

        public void Animate()
        {
            Elapsed += TimeSpan.FromMilliseconds(100);
            if(Elapsed.TotalMilliseconds % 1000 <= 100)
                MakeWaves();
            if (Elapsed.TotalMilliseconds % 1000 <= _windSpeed)
                BlowWind();
        }

        private void BlowWind()
        {
            var plains = Parent.GoRogueComponents.GetFirstOrDefault<WindyPlain>();
            if (plains is not null)
            {
                plains.DetermineNextStates();
                foreach (var cell in plains.Cells)
                {
                    var position = cell.Position - plains.Body.Position;
                    var wind = cell.GoRogueComponents.GetFirst<BlowsInWind>();
                    if(Parent.Contains(position))
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
            var pond = Parent.GoRogueComponents.GetFirstOrDefault<BodyOfWater>();
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
