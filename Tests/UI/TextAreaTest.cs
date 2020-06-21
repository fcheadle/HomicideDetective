using Engine.UI;
using GoRogue;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.UI
{
    class TextAreaTests : TestBase
    {
        TextArea _area;
        [SetUp]
        public void SetUp()
        {
            _area = new TextArea(20, 20);
        }
        [Test]
        public void NewTextAreaTest()
        {
            Assert.NotNull(_area);
            Assert.AreEqual(" ", _area.Text);
            Assert.AreEqual(new Coord(0, 0), _area.CursorPosition);
        }
        [Test]
        public void NewLineTest()
        {
            Assert.True(_area.NewLine());
            Assert.AreEqual(new Coord(0, 1), _area.CursorPosition);
        }
        [Test]
        public void CheckForInvalidKeysTest()
        {
            Assert.False(_area.IsValidKey(Keys.Tab));
            Assert.False(_area.IsValidKey(Keys.Back));
            Assert.False(_area.IsValidKey(Keys.Delete));
            Assert.False(_area.IsValidKey(Keys.Escape));
            Assert.False(_area.IsValidKey(Keys.PageDown));
            Assert.True(_area.IsValidKey(Keys.A));
            Assert.True(_area.IsValidKey(Keys.B));
            Assert.True(_area.IsValidKey(Keys.C));
            Assert.True(_area.IsValidKey(Keys.Q));
            Assert.True(_area.IsValidKey(Keys.W));
            Assert.True(_area.IsValidKey(Keys.P));
            Assert.True(_area.IsValidKey(Keys.O));
            Assert.True(_area.IsValidKey(Keys.I));
            Assert.True(_area.IsValidKey(Keys.L));
            Assert.True(_area.IsValidKey(Keys.K));
            Assert.True(_area.IsValidKey(Keys.J));
            Assert.True(_area.IsValidKey(Keys.U));
            Assert.True(_area.IsValidKey(Keys.NumPad6));
            Assert.True(_area.IsValidKey(Keys.NumPad4));
            Assert.True(_area.IsValidKey(Keys.Space));
            Assert.True(_area.IsValidKey(Keys.OemBackslash));
            Assert.True(_area.IsValidKey(Keys.OemQuestion));
            Assert.True(_area.IsValidKey(Keys.OemQuotes));
        }
        [Test]
        public void WriteLineTest()
        {
            Coord startingPos = _area.CursorPosition;
            Coord expectedPos = startingPos + new Coord(0, 1);
            _area.WriteLine("Boy howdy, sure is hot today!");
            Assert.True(_area.Text.Contains("Boy howdy"));
            Assert.True(_area.Text.Contains("sure is hot today!"));
        }

        [Test]
        public void WriteTest()
        {
            _area.Write("f");
            Assert.True(_area.Text.Contains("f"));
            _area.Write("u");
            _area.Write("c");
            _area.Write("k");
            _area.Write(" ");
            _area.Write("y");
            _area.Write("o");
            _area.Write("u");

            Assert.True(_area.Text.Contains("fuck"));
            Assert.True(_area.Text.Contains("you"));
            Assert.False(_area.Text.Contains("fuckyou"));
        }
    }
}
