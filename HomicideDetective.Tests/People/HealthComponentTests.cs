using System.Threading;
using HomicideDetective.People.Components;
using SadRogue.Primitives;
using Xunit;

namespace HomicideDetective.Tests
{
    public class HealthComponentTests
    {
        [Fact]
        public void NewHealthComponentTest()
        {
            HealthComponent component = new HealthComponent();
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
            HealthComponent component = new HealthComponent();
            var answer = component.GetDetails();
            Assert.True(4 < answer.Length);
        }

        [Fact]
        public void BreatheTest()
        {
            HealthComponent component = new HealthComponent();
            double prev = 0;
            for (int i = 0; i < 10; i++)
            {
                component.Breathe(i * 150.0);
                var breath = component.CurrentBreathVolume;
                Assert.True(0 < breath); // don't ever FULLY run out of breath
                Assert.True(1000 > breath);
                Assert.False(prev == breath);
                prev = breath;
            }
        }

        [Fact]
        public void HeartBeatTest()
        {
            HealthComponent component = new HealthComponent();
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
            HealthComponent component = new HealthComponent();
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
            HealthComponent component = new HealthComponent();
            component.BeatHeart(250);
            var heart = component.HeartMonitorStrings();
            Assert.Equal(5, heart.Length);
        }

        [Fact]
        public void MurderTest()
        {
            HealthComponent component = new HealthComponent();
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
            HealthComponent component = new HealthComponent();
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
