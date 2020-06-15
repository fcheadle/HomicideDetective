using Engine.Components;
using Engine.Utilities;
using Engine.Utilities.Mathematics;
using GoRogue;
using SadConsole;
using System;

namespace Engine.Scenes.Terrain.Components
{
    public class WeatherComponent : Component
    {
        //add this to the game's map, not individual tiles... although, it isn't working for some reason.
        public SceneMap Area => Game.Map;
        private int _width;
        private int _height;
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
            _elapsed += TimeSpan.FromMilliseconds(100);
            BlowWind();
        }

        private void BlowWind()
        {
            for (int x = 0; x < Area.Width; x++)
            {
                for (int y = 0; y < Area.Height; y++)
                {
                    BasicTerrain terrain = Area.GetTerrain<BasicTerrain>(new Coord(x, y));
                    if (terrain != null)
                    {
                        if (terrain.HasComponent<AnimateGlyphComponent>())
                        {
                            double z = Fxyt(x, y, _elapsed);
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
