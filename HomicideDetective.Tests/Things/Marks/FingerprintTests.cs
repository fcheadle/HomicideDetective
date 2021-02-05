using System;
using HomicideDetective.Things.Marks;
using Xunit;
using Xunit.Abstractions;

namespace HomicideDetective.Tests.Things.Marks
{
    public class FingerprintTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public FingerprintTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void NewFingerprintTest()
        {
            var print = new Fingerprint(128);
            Assert.Equal("fingerprint", print.Name);
        }

        [Fact]
        public void GenerateFingerprintTest()
        {
            var print = new Fingerprint(128);
            print.Generate();

            foreach (var answer in print.GetDetails())
            {
                _testOutputHelper.WriteLine(answer);
            }
        }
    }
}