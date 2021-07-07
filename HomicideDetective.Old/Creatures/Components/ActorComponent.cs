using GoRogue;

namespace HomicideDetective.Old.Creatures.Components
{
    public class ActorComponent : ComponentBase
    {
        public int FovRadius = 15;

        public Coord Position => Parent.Position;
        public ActorComponent(EntityBase parent) : base(isUpdate: true, isKeyboard: false, isDraw: false, isMouse: false)
        {
            Parent = parent;
            Name = "Actor Component";
        }
        public void Act()
        {
            //if (Parent.GetComponent<CSIKeyboardComponent>() != null)
            //    return;

            //Determine whether or not we have a path
            // if (_path == null)
            // {
            //     //DecideWhatToDo();
            // }
        }

        // private void DecideWhatToDo()
        // {
        //     //just move in a random direction for now
        //     List<Direction> directions = new List<Direction>();
        //     directions.Add(Direction.UP);
        //     directions.Add(Direction.LEFT);
        //     directions.Add(Direction.RIGHT);
        //     directions.Add(Direction.DOWN);
        //     Direction d = directions.RandomItem();
        //     Parent.MoveIn(d);
        // }

        public void Interact(EntityBase sender)
        {
        }

        public override string[] GetDetails()
        {
            string[] answer = {
                "This is an actor.",
                "It can move from place to place."
            };
            return answer;
        }

        public override void ProcessTimeUnit()
        {
            Act();
        }
    }
}
