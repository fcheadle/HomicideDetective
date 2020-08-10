using Engine;

namespace Tests.Mocks
{
    internal class MockComponent : ComponentBase
    {
        public int UpdateCounter;
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
