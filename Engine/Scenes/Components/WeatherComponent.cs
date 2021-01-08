using Engine.Utilities;
using Engine.Utilities.Mathematics;
using GoRogue;
using SadConsole;
using System;

namespace Engine.Scenes.Components
{
    //Requires significant refactor on alpha
    public class WeatherComponent : ComponentBase
    {
        private SceneMap _map => Game.Map;
        Func<int, int, TimeSpan, double> Fxyt;// F of x, y, and t
        Direction.Types WindDirection;
        float WindStrength; //meters per second
        public WeatherComponent() : base(true, false, false, false)
        {
            WindDirection = EnumUtils.RandomEnumValue<Direction.Types>();
            WindStrength = Calculate.PercentValue() / 10.01f; //arbitrary?
            Fxyt = Formulae.RandomWindPattern();//todo... this, but better
        }

        public override string[] GetDetails()
        {
            string[] answer =
            {
                "This is the weather component. It's parent is a map, not a regular entity.",
                "\nWind is currently blowing " + WindDirection.ToString(),
                "at a speed of " + WindStrength + "mph"
            };
            return answer;
        }

        public override void ProcessTimeUnit()
        {
            Elapsed += TimeSpan.FromMilliseconds(100);
            BlowWind();
            Game.UiManager.Display.IsDirty = true;
        }

        private void BlowWind()
        {
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    BasicTerrain terrain = _map.GetTerrain<BasicTerrain>(new Coord(x, y));
                    if (terrain != null)
                    {
                        if (terrain.HasComponent<AnimateGlyphComponent>())
                        {
                            double z = Fxyt(x, y, Elapsed);
                            var component = terrain.GetComponent<AnimateGlyphComponent>();
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
