using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Tests
{
    abstract class TestBase
    {
        public bool Finished { get; set; } = false;
        public bool Running { get; set; } = false;

        protected List<Action<GameTime>> _completedTests = new List<Action<GameTime>>();
        protected List<Action<GameTime>> _notCompletedTests = new List<Action<GameTime>>();
        protected static MockGame _game;
    }
}
