using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;

namespace HomicideDetective.New.Scenes.Generation
{
    public class HouseGenerationStep : GenerationStep
    
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<RogueLikeCell>>
                (() => new ArrayView<RogueLikeCell>(context.Width, context.Height), "house");
            
            var floorSpace = new Rectangle(4, 4, map.Width - 8, map.Height - 8);
            
            foreach (var room in floorSpace.BisectRecursive(8))
            {
                var region = Region.FromRectangle("room", room);
                foreach (var point in region.InnerPoints.Positions.Where(p=> map.Contains(p)))
                {
                    bool alt = (point.X + point.Y) % 2 == 0;
                    int glyph = alt ? 9 : 10;
                    Color fore = Color.Yellow;
                    Color back = Color.DarkGoldenrod;
                    map[point] = new RogueLikeCell(point, fore, back, glyph, 0);
                }

                
                foreach (var point in region.OuterPoints.Positions.Where(p=> map.Contains(p)))
                {
                    map[point] = new RogueLikeCell(point, Color.Red, Color.White, 178, 0, false, false);
                }

                yield return null;
            }
        }
    }
}