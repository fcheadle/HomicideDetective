using Engine.Components.Creature;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Tests.Components.Creature
{
    class HealthComponentTests : TestBase
    {

        //sometimes you just need to store values for checking during updates
        HealthComponent _component;
        string[] _answer;
        float _breath;
        float _minimum = 0.0f; 
        float _maximum;
        float _heartBeatStatus;
        DateTime _start;
        DateTime _previous;
        [Test]
        public void NewHealthComponentTest()
        {
            _game = new MockGame(NewHealthComponent);
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void NewHealthComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _component = MockGame.Player.GetGoRogueComponent<HealthComponent>();
            Assert.NotNull(_component);
            Assert.NotNull(_component.SystoleBloodPressure);
            Assert.Less(0, _component.SystoleBloodPressure);
            Assert.NotNull(_component.DiastoleBloodPressure);
            Assert.Less(0, _component.DiastoleBloodPressure);
            Assert.NotNull(_component.Pulse);
            Assert.Less(0, _component.Pulse);
            Assert.NotNull(_component.BreathRate);
            Assert.Less(0, _component.BreathRate);
            Assert.NotNull(_component.NormalBodyTemperature);
            Assert.Less(0, _component.NormalBodyTemperature);
            Assert.NotNull(_component.CurrentBodyTemperature);
            Assert.Less(0, _component.CurrentBodyTemperature);
            Assert.NotNull(_component.LungCapacity);
            Assert.Less(0, _component.LungCapacity);
            Assert.NotNull(_component.CurrentBreathVolume);
            Assert.Less(0, _component.CurrentBreathVolume);
            Assert.NotNull(_component.BloodVolume);
            Assert.Less(0, _component.BloodVolume);
            Assert.NotNull(_component.TypicalBloodVolume);
            Assert.Less(0, _component.TypicalBloodVolume);
        }
        [Test]
        public void GetDetailsTest()
        {
            _game = new MockGame(GetDetails);
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _component = MockGame.Player.GetGoRogueComponent<HealthComponent>();
            _answer = _component.GetDetails();
            _maximum = _component.LungCapacity;
            Assert.AreEqual(3, _answer.Length);
        }

        [Test]
        public void BreathingTest()
        {
            _game = new MockGame(StartBreathing);
            MockGame.RunOnce();
            _breath = _component.CurrentBreathVolume;
            Thread.Sleep(100);
            MockGame.RunOnce();
            MockGame.RunOnce();
            _start = DateTime.Now;
            _previous = _start;
            _game.SwapUpdate(JustBreathe);
            MockGame.Start();

        }

        private void StartBreathing(GameTime time)
        {
            _component = MockGame.Player.GetGoRogueComponent<HealthComponent>();

        }

        private void JustBreathe(GameTime time)
        {
            if (DateTime.Now - _previous > TimeSpan.FromSeconds(1))
            {
                _previous = DateTime.Now;
                _component = MockGame.Player.GetGoRogueComponent<HealthComponent>();
                Assert.AreNotEqual(_breath, _component.CurrentBreathVolume);
                _breath = _component.CurrentBreathVolume;
                Assert.Less(0, _breath);
                Assert.Greater(_maximum, _breath);
            }
            if (DateTime.Now - _start > TimeSpan.FromSeconds(5))
            {
                MockGame.Stop();
            }
        }
        [Test]
        public void HeartBeatTest()
        {
            _game = new MockGame(StartBeating);
            MockGame.RunOnce();
            _breath = _component.CurrentBreathVolume;
            _game.SwapUpdate(BeatHeart);
            MockGame.RunOnce();
            MockGame.RunOnce();
            MockGame.RunOnce();
            MockGame.RunOnce();
            MockGame.RunOnce();
            MockGame.RunOnce();
        }

        private void StartBeating(GameTime time)
        {
            _component = MockGame.Player.GetGoRogueComponent<HealthComponent>();
            _component.ProcessGameFrame();
        }

        private void BeatHeart(GameTime time)
        {
            if (DateTime.Now - _previous > TimeSpan.FromMilliseconds(100))
            {
                _previous = DateTime.Now;
                _component = MockGame.Player.GetGoRogueComponent<HealthComponent>();
                Assert.AreNotEqual(_breath, _component.CurrentBreathVolume);
                _breath = _component.CurrentBreathVolume;
                Assert.Less(0, _breath);
                Assert.Greater(_maximum, _breath);
            }
            if(DateTime.Now - _start > TimeSpan.FromSeconds(5))
            {
                MockGame.Stop();
            }
        }
    }
}
