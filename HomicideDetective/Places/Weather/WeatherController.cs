using System;
using SadConsole;
using SadRogue.Integration.Maps;
using SadRogue.Primitives.GridViews;

// ReSharper disable PossibleLossOfFraction

namespace HomicideDetective.Places.Weather
{
    /// <summary>
    /// The weather component added to
    /// </summary>
    public class WeatherController : SadConsole.Components.UpdateComponent//ParentAwareComponentBase<RogueLikeMap>
    {
        private RogueLikeMap Parent;
        public int Elapsed { get; private set; }
        private int _windSpeed;

        public WeatherController()
        {
            _windSpeed = new Random().Next(1, 5);
        }

        public override void OnAdded(IScreenObject host)
        {
            if (host is RogueLikeMap map)
                Parent = map;
            else
                throw new ArgumentException($"Weather Controller must be added to a parent of type {typeof(RogueLikeMap)}");
        }

        public override void Update(IScreenObject host, TimeSpan delta)
        {
            Elapsed++;
            if(Elapsed % 10 == 0)
                MakeWaves();
            if (Elapsed % 15 <= _windSpeed)
                BlowWind();
        }

        private void BlowWind()
        {
            var plains = Parent!.GoRogueComponents.GetFirstOrDefault<WindyPlain>();
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
            var pond = Parent!.GoRogueComponents.GetFirstOrDefault<BodyOfWater>();
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
