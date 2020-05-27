using Engine.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    internal class MockComponent : ComponentBase
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

        public override void ProcessGameFrame()
        {
            UpdateCounter++;
        }
    }
}
