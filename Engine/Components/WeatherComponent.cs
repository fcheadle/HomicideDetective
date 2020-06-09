using Engine.Components.Terrain;
using Engine.Maps;
using Engine.Utilities;
using GoRogue;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components
{
    public class WeatherComponent : Component
    {
        //add this to the game's map, not individual tiles
        public SceneMap Area; //Area aka parent
        Func<int, int, TimeSpan, double> Fxyt;// F of x, y, and t
        Direction.Types WindDirection;
        float WindStrength; //meters per second
        public WeatherComponent(SceneMap area) : base(true, false, false, false)
        {
            Area = area;
            WindDirection = EnumUtils.RandomEnumValue<Direction.Types>();
            WindStrength = Calculate.Percent() / 10.01f; //arbitrary?
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
            BlowWind();

        }

        private void BlowWind()
        {
            for (int x = 0; x < Area.Width; x++)
                for (int y = 0; y < Area.Height; y++)
                    if (Area.GetTerrain<BasicTerrain>(new Coord(x, y)) != null)
                        if (Area.GetTerrain<BasicTerrain>(new Coord(x, y)).HasComponent<AnimateGlyphComponent>())
                        {
                            double z = Fxyt(x, y, _elapsed);
                            if (z > 1.75 || z < -1.75)
                                Area.GetTerrain<BasicTerrain>(new Coord(x, y)).GetComponent<AnimateGlyphComponent>().Start();
                        }
        }
    }
}
