using GoRogue;
using GoRogue.Pathing;
using SadConsole;
using System;
using System.Collections.Generic;

namespace Engine.Components.Creature
{
    public class ActorComponent : ComponentBase
    {
        private Path _path;
        private BasicEntity _target;
        public int FOVRadius = 15;
        public Coord Position => Parent.Position;
        public ActorComponent() : base(isUpdate: true, isKeyboard: false, isDraw: false, isMouse: false)
        {
        }

        public void Act()
        {
            if (((BasicEntity)Parent).GetGoRogueComponent<KeyboardComponent>() != null)
                return;

            //Determine whether or not we have a path
            if (_path == null)
            {
                DecideWhatToDo();
            }
            //just move in a random direction for now
            List<Direction> directions = new List<Direction>();
            directions.Add(Direction.UP);
            directions.Add(Direction.LEFT);
            directions.Add(Direction.RIGHT);
            directions.Add(Direction.DOWN);
            Direction d = directions.RandomItem();
            Parent.MoveIn(d);
        }

        private void DecideWhatToDo()
        {
            //throw new NotImplementedException();
        }

        public void Interact(BasicEntity sender)
        {
        }

        public override void ProcessGameFrame()
        {
            throw new NotImplementedException();//not getting called?
        }

        public override string[] GetDetails()
        {
            string[] answer = {
                "This is an actor.",
                "It can move from place to place.",
                "Current target: " + _target.Name
            };
            return answer;
        }
    }
}
