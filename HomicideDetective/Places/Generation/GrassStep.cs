using System;
using System.Collections.Generic;
using GoRogue;
using GoRogue.MapGeneration;
using HomicideDetective.Places.Weather;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using SadRogue.Integration.FieldOfView.Memory;

namespace HomicideDetective.Places.Generation
{
    public class GrassStep : GenerationStep
    {
        //different patterns that grass can blow in
        public static List<Func<int, int, double>> TerrainGenerationFormulae = new List<Func<int, int, double>>()
        {
            (x,y) => 59.00 * (Math.Sin(x + x * 2.25) + Math.Cos(y + y/7)), //fave
            (x,y) => 59.00 * Math.Cos(Math.Sqrt(x *x + y * y) * 2), //subtle
            (x,y) => 59.00 * Math.Sin(x * Math.Sin(y) / 5) + Math.Sin(y * Math.Sin(x) / 5), //natural
            (x,y) => 59.00 * Math.Sin(x * Math.Sin(y)) + Math.Sin(y * Math.Sin(x) / 8), //moderate speed, natural-ish
            (x,y) => 59.00 * Math.Sin(x * Math.Sin(y)/4) + Math.Sin(y * Math.Sin(x) * Math.PI), //moderate speed, very natural seeming
            (x,y) => 59.50 * Math.Sin(x * Math.Sin(y) * Math.PI) + Math.Sin(y * Math.Sin(x) / 8), //moderate speed, very natural seeming
        };
                
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var f = TerrainGenerationFormulae.RandomItem();
            var map = context.GetFirstOrNew<ISettableGridView<MemoryAwareRogueLikeCell>>
                (() => new ArrayView<MemoryAwareRogueLikeCell>(context.Width, context.Height), "WallFloor");
            var plains = context.GetFirstOrNew(() => new List<WindyPlain>(), "plains");
            
            //just make a blank collection of regions for further use
            context.GetFirstOrNew(() => new List<Region>(), "regions");

            var windDirection = RandomDirection();
            var rect = new Rectangle((5, 5), (map.Width - 5, map.Height - 5));
            var plain = new WindyPlain(rect, windDirection);
            plain.SeedStartingPattern();
            plains.Add(plain);
            
            //iterate the whole map
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    //figure out grass color
                    int green = (int) (Math.Abs(f(i, j)));
                    Color color = new Color(0, green, 0);

                    //create grass
                    var cell =  new MemoryAwareRogueLikeCell((i, j), color, Color.Black, '"', 0);
                    map[i, j] = cell;
                    
                    //set up weather
                    if(plain.Body.Contains((i,j)))
                    {
                        cell.GoRogueComponents.Add(new BlowsInWind(windDirection));
                        plain.Cells.Add(cell);
                    }
                }
            }

            yield return null;
        }

        private Direction RandomDirection()
        {
            var dirs = new[]
            {
                Direction.Left,
                Direction.Right,
                Direction.Down,
                Direction.Up
            };

            return dirs.RandomItem();
        }
    }
}