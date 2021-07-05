using System.Collections.Generic;
using GoRogue.MapGeneration;
using HomicideDetective.Places.Generation;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Primitives.GridViews;
using Xunit;

namespace HomicideDetective.Tests.Places.Generation
{
    public class GenerationStepTest
    {
        private GenerationContext Generate<T>() where T : GenerationStep, new()
            => new Generator(100, 100).ConfigAndGenerateSafe(gen => gen.AddStep(new T())).Context;

        private void AssertGeneratedMapHasRequiredComponents(GenerationContext context)
        {
            //required components:
            var regions = context.GetFirst<List<Region>>("regions");
            Assert.NotNull(regions);

            var wallFloor = context.GetFirst<ISettableGridView<MemoryAwareRogueLikeCell>>("WallFloor");
            Assert.NotNull(wallFloor);
        }
        
        [Fact]
        public void TestDownTownStep()
        {
            var step = Generate<DownTownStep>();
            AssertGeneratedMapHasRequiredComponents(step);
        }
        [Fact]
        public void TestGrassStep()
        {
            var step = Generate<GrassStep>();
            AssertGeneratedMapHasRequiredComponents(step);
        }
        [Fact]
        public void TestHouseStep()
        {
            var step = Generate<HouseStep>();
            AssertGeneratedMapHasRequiredComponents(step);
        }
        [Fact]
        public void TestParkFeaturesStep()
        {
            var step = Generate<ParkFeaturesStep>();
            AssertGeneratedMapHasRequiredComponents(step);
        }
        [Fact]
        public void TestStreetStep()
        {
            var step = Generate<DownTownStep>();
            AssertGeneratedMapHasRequiredComponents(step);
        }
    }
}