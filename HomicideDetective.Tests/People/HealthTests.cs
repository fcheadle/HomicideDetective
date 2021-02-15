using System;
using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class HealthTests
    {
        [Fact]
        public void NewHealthComponentTest()
        {
            Health component = new Health();
            Assert.True(0 < component.SystoleBloodPressure);
            Assert.True(0 < component.DiastoleBloodPressure);
            Assert.True(0 < component.Pulse);
            Assert.True(0 < component.BreathRate);
            Assert.True(0 < component.NormalBodyTemperature);
            Assert.True(0 < component.CurrentBodyTemperature);
            Assert.True(0 < component.LungCapacity);
            Assert.True(0 < component.CurrentBreathVolume);
            Assert.True(0 < component.BloodVolume);
            Assert.True(0 < component.TypicalBloodVolume);
        }
        
        [Fact]
        public void GetDetailsTest()
        {
            Health component = new Health();
            var answer = component.Details;
            Assert.True(3 < answer.Length);
        }

        [Fact]
        public void BreatheTest()
        {
            Health component = new Health();
            double prev = 0;
            for (int i = 0; i < 10; i++)
            {
                component.Breathe(i * 150.0);
                var breath = component.CurrentBreathVolume;
                Assert.True(0 < breath); // don't ever FULLY run out of breath
                Assert.True(1000 > breath);
                Assert.False(Math.Abs(prev - breath) < 0.005);
                prev = breath;
            }
        }

        [Fact]
        public void HeartBeatTest()
        {
            Health component = new Health();
            double prev = 0;
            for (int i = 0; i < 30; i++)
            {
                component.BeatHeart(i);
                var heart = component.CurrentHeartStatus;
                Assert.True(-2 < heart);
                Assert.True(2 > heart);
                Assert.NotEqual(prev, heart);
                prev = heart;
            }
        }

        [Fact]
        public void MonitorHeartTest()
        {
            Health component = new Health();
            for (int i = 0; i < 30; i++)
            {
                component.BeatHeart(i * 250);
                var heart = component.MonitorHeart();
                Assert.True(-2 <= heart.Y);
                Assert.True(2 >= heart.Y);
            }
        }

        [Fact]
        public void MonitorHeartStringsTest()
        {
            Health component = new Health();
            component.BeatHeart(250);
            var heart = component.HeartMonitorStrings();
            Assert.Equal(5, heart.Length);
        }

        [Fact]
        public void MurderTest()
        {
            Health component = new Health();
            component.ProcessTimeUnit();
            var heart = component.CurrentHeartStatus;
            var breath = component.CurrentBreathVolume;
            
            component.Murder();

            Assert.False(component.Alive);
            
            component.ProcessTimeUnit();
            Assert.Equal(heart, component.CurrentHeartStatus);
            Assert.Equal(breath, component.CurrentBreathVolume);
            
            
            component.ProcessTimeUnit();
            Assert.Equal(heart, component.CurrentHeartStatus);
            Assert.Equal(breath, component.CurrentBreathVolume);
        }

        [Fact]
        public void ProcessTimeUnitTest()
        {
            Health component = new Health();
            double prevCardio = 0;
            double prevRespiratory = 0;
            for (int i = 0; i < 30; i++)
            {
                component.ProcessTimeUnit();
                var heart = component.CurrentHeartStatus;
                var breath = component.CurrentBreathVolume;

                Assert.True(-2 < heart);
                Assert.True(2 > heart);
                Assert.NotEqual(prevCardio, heart);
                Assert.NotEqual(prevRespiratory, breath);
                prevCardio = heart;
                prevRespiratory = breath;
            }
        }
    }
}
