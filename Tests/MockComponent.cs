using Engine.Components;

namespace Tests
{
    internal class MockComponent : Component
    {
        public int UpdateCounter = 0;
        internal MockComponent() : base(true, true, true, false)
        {

        }
        public override string[] GetDetails()
        {
            string[] answer =
            {
                "This is a test component.",
                "It is used for testing purposes only."
            };
            return answer;
        }

        public override void ProcessTimeUnit()
        {
            UpdateCounter++;
        }
    }
}
