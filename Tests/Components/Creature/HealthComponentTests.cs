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
        float _minimumHeartStatus = 0;
        float _currentHeartStatus;
        float _maximumHeartStatus = 0;
        DateTime _start;
        DateTime _previous;
        [Test]
        public void NewHealthComponentTest()
        {
            _game = new MockGame(NewHealthComponent);
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void NewHealthComponent(GameTime time)
        {
            _component = (HealthComponent)MockGame.Player.GetComponent<HealthComponent>();
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
            _component.ProcessTimeUnit();
            _breath = _component.CurrentBreathVolume;
            //_currentHeartStatus = _component.MonitorHeart().Y;
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
            _component = (HealthComponent)MockGame.Player.GetComponent<HealthComponent>();
            _answer = _component.GetDetails();
            _maximum = _component.LungCapacity;
            Assert.AreEqual(4, _answer.Length);
        }

        [Test]
        public void BreathingTest()
        {
            _game = new MockGame(NewHealthComponent);
            MockGame.RunOnce();
            MockGame.RunOnce();
            MockGame.RunOnce();
            _game.SwapUpdate(JustBreathe);
            _start = DateTime.Now;
            _previous = _start;
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(15);
                MockGame.RunOnce();
            }
        }
        private void JustBreathe(GameTime time)
        {
            if (DateTime.Now - _previous > TimeSpan.FromSeconds(2))
            {
                _previous = DateTime.Now;
                _component = (HealthComponent)MockGame.Player.GetComponent<HealthComponent>();
                Assert.AreNotEqual(_breath, _component.CurrentBreathVolume);
                _breath = _component.CurrentBreathVolume;
                Assert.Less(0, _breath);
                Assert.Greater(_maximum, _breath);
            }
        }
        [Test]
        public void HeartBeatTest()
        {
            _game = new MockGame(NewHealthComponent);
            MockGame.RunOnce();
            MockGame.RunOnce();
            MockGame.RunOnce();
            _game.SwapUpdate(BeatHeart);
            _start = DateTime.Now;
            _previous = _start;
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(15);
                MockGame.RunOnce();
            }
            Assert.LessOrEqual(-2, Math.Round(_minimumHeartStatus));
            Assert.GreaterOrEqual(2, Math.Round(_maximumHeartStatus));
        }
        private void BeatHeart(GameTime time)
        {
            _previous = DateTime.Now;
            _component = (HealthComponent)MockGame.Player.GetComponent<HealthComponent>();
            _currentHeartStatus = _component.MonitorHeart().Y;
            if (_minimumHeartStatus > _currentHeartStatus)
                _minimumHeartStatus = _currentHeartStatus;
            if (_maximumHeartStatus < _currentHeartStatus)
                _maximumHeartStatus = _currentHeartStatus;

        }
    }
}
