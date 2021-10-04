using System.Collections.Generic;
using HomicideDetective.People;
using Xunit;
using Xunit.Abstractions;

namespace HomicideDetective.Tests.People
{
    public class FingerprintTests
    {
        public static readonly IEnumerable<object[]> IntData = new List<object[]>()
        {
            new object[] {79},
            new object[] {144},
            new object[] {169},
            new object[] {4026},
            new object[] {10050},
            new object[] {69696},
            new object[] {999999},
        };
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

        [Theory]
        [MemberData(nameof(IntData))]
        public void GenerateFingerprintTest(int seed)
        {
            var print = new Fingerprint(seed);
            print.Generate();

            foreach (var answer in print.PatternMap())
            {
                _testOutputHelper.WriteLine(answer);
            }
        }
    }
}