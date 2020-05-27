namespace Engine.Components.Creature
{
    public class ThoughtsComponent : ComponentBase
    {
        public ThoughtsComponent() : base(true, false, false, false)
        {
        }

        public override string[] GetDetails()
        {
            string[] answer = {
                "This is a thought component.",
                "The entity with this component has a thought process."
            };
            return answer;
        }

        public override void ProcessGameFrame()
        {
            //throw new NotImplementedException();
        }
    }
}
