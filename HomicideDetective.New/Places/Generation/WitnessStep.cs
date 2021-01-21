using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.New.People;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;

namespace HomicideDetective.New.Places.Generation
{
    public class WitnessStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var rooms = context.GetFirstOrNew<List<Region>>
                (() => new List<Region>(), "rooms");
            
            var map = context.GetFirstOrNew<ISettableGridView<RogueLikeCell>>
                (() => new ArrayView<RogueLikeCell>(context.Width, context.Height), "house");
            
            var people = context.GetFirstOrNew<List<Person>>
                (() => new List<Person>(), "people");

            foreach (var room in rooms)
            {
                var position = room.InnerPoints.Positions.First(p => map[p] is not null && map[p].IsWalkable);
                // people.Add(new RogueLikeEntity(position, 'p'));
                people.Add(new Person(position));

                yield return null;
            }
        }
    }
}