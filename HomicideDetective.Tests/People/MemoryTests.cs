using System;
using System.Collections.Generic;
using HomicideDetective.People;
using Xunit;

namespace HomicideDetective.Tests.People
{
    public class MemoryTests
    {
        private readonly string _occurrence = "test began";
        private readonly string _where = "bin/debug/";
        private readonly IEnumerable<string> _who = new []{"Nathan Fielder", "Jenna Friedman", "Eric Andre"};
        private readonly IEnumerable<string> _what = new []{"a camera", "a television crew", "gumption"};
        private readonly DateTime _startDate = DateTime.Now;
        
        [Fact]
        public void MemoryAloneNoItemsTest()
        {
            var memory = new Memory(_startDate, _occurrence, _where);
            Assert.Equal(_occurrence, memory.Occurrence);
            Assert.Empty(memory.Who);
            Assert.Empty(memory.What);
            Assert.False(memory.Private);
            Assert.True(memory.When < DateTime.Now);
            
            memory = new Memory(_startDate, _occurrence, _where, isPrivate: true);
            Assert.Equal(_occurrence, memory.Occurrence);
            Assert.Empty(memory.Who);
            Assert.Empty(memory.What);
            Assert.True(memory.Private);
            Assert.True(memory.When < DateTime.Now);
        }

        [Fact]
        public void MemoryWithOthersNoItemsTest()
        {
            var memory = new Memory(_startDate, _occurrence, _where, _who);
            Assert.Empty(memory.What);
            Assert.False(memory.Private);
            Assert.True(memory.When < DateTime.Now);
            
            foreach (var name in _who)
                Assert.Contains(name, memory.Who);
        }

        [Fact]
        public void MemoryAloneWithItemsTest()
        {
            var memory = new Memory(_startDate, _occurrence, _where, null, _what);
            Assert.Empty(memory.Who);
            Assert.False(memory.Private);
            Assert.True(memory.When < DateTime.Now);
            
            foreach (var name in _what)
                Assert.Contains(name, memory.What);
        }

        [Fact]
        public void MemoryWithOthersAndItemsTest()
        {
            var memory = new Memory(_startDate, _occurrence, _where, _who, _what);
            Assert.False(memory.Private);
            Assert.True(memory.When < DateTime.Now);
            
            foreach (var name in _who)
                Assert.Contains(name, memory.Who);
            
            foreach (var name in _what)
                Assert.Contains(name, memory.What);
        }

        [Fact]
        public void GetPrintableStringTest()
        {
            var memory = new Memory(_startDate, _occurrence, _where);
            var answer = memory.GetPrintableString();
            Assert.DoesNotContain("were there", answer);
            Assert.DoesNotContain("were involved", answer);
            
            memory = new Memory(_startDate, _occurrence, _where, _who);
            answer = memory.GetPrintableString();
            Assert.Contains("were there", answer);
            Assert.DoesNotContain("were involved", answer);
            
            memory = new Memory(_startDate, _occurrence, _where, null, _what);
            answer = memory.GetPrintableString();
            Assert.DoesNotContain("were there", answer);
            Assert.Contains("were involved", answer);
            
            memory = new Memory(_startDate, _occurrence, _where, _who, _what);
            answer = memory.GetPrintableString();
            Assert.Contains("were there", answer);
            Assert.Contains("were involved", answer);

        }
    }
}