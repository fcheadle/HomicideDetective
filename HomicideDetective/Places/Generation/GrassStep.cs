using System;
using System.Collections.Generic;
using GoRogue;
using GoRogue.MapGeneration;
using HomicideDetective.UserInterface;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using SadRogue.Integration;

namespace HomicideDetective.Places.Generation
{
    public class GrassStep : GenerationStep
    {
        public static List<Func<int, int, double>> TerrainGenerationFormulae = new List<Func<int, int, double>>()
        {
            (x,y) => 59.00 * (Math.Sin(x + x * 2.25) + Math.Cos(y + y/7)), //fast, personal fave
            (x,y) => 59.00 * Math.Cos(Math.Sqrt(x *x + y * y) * 2), //fast, small concentric circles
            (x,y) => 59.00 * Math.Sin(x * Math.Sin(y) / 5) + Math.Sin(y * Math.Sin(x) / 5), //moderate speed, natural-ish, symetrical with respect to x,y
            (x,y) => 59.00 * Math.Sin(x * Math.Sin(y)) + Math.Sin(y * Math.Sin(x) / 8), //moderate speed, natural-ish
            (x,y) => 59.00 * Math.Sin(x * Math.Sin(y)/4) + Math.Sin(y * Math.Sin(x) * Math.PI), //moderate speed, very natural seeming
            (x,y) => 59.50 * Math.Sin(x * Math.Sin(y) * Math.PI) + Math.Sin(y * Math.Sin(x) / 8), //moderate speed, very natural seeming
            (x,y) => 59.19 * (Math.Sin((x + y) *0.66) + Math.Cos((y + x) * 1.33)), //moderate, Diagonal Ridges
        };
                
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var f = TerrainGenerationFormulae.RandomItem();
            var map = context.GetFirstOrNew<ISettableGridView<RogueLikeCell>>
                (() => new ArrayView<RogueLikeCell>(context.Width, context.Height), "grass");

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    int green = (int) (Math.Abs(f(i, j)));
                    Color color = new Color(0, green, 0);

                    var cell =  new RogueLikeCell((i, j), color, Color.Black, '"', 0);
                    cell.GoRogueComponents.Add(new AnimatingGlyph('"', new int[]{ '\\', '|', '/'}));
                    map[i, j] = cell;
                }
            }

            yield return null;
        }
    }
}