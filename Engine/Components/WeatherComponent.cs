using Engine.Components.Terrain;
using Engine.Maps;
using Engine.States;
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
        SceneMap Area;
        Func<int, int, TimeSpan, bool> Fxyt;// F of x, y, and t
        Direction.Types WindDirection;
        float WindStrength; //meters per second
        public WeatherComponent(SceneMap area) : base(true, false, false, false)
        {
            Area = area;
            WindDirection = EnumUtils.RandomEnumValue<Direction.Types>();
            WindStrength = Calculate.Percent() / 10.01f; //arbitrary?
            Fxyt = Calculate.RandomFunction4d();//todo... this, but better
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
                            if (Fxyt(x, y, _elapsed))
                                Area.GetTerrain<BasicTerrain>(new Coord(x, y)).GetComponent<AnimateGlyphComponent>().Start();
        }
    }
}
