using HomicideDetective.UserInterface;
using SadRogue.Primitives;

namespace HomicideDetective.Places.Weather
{
    public class BlowsInWind : AnimatingGlyphComponent
    {
        private static readonly int[] _eastGlyphs = new int[] {'"', 174, 27};
        private static readonly int[] _westGlyphs = new int[] {'"', 175, 26};
        private static readonly int[] _northGlyphs = new int[] {'"', 94, 24};
        private static readonly int[] _southGlyphs = new int[] {'"', 118, 25};
        public MovesInWaves.States State = MovesInWaves.States.Off;
        
        public BlowsInWind(Direction direction) : base('"',
            direction == Direction.Left ? _eastGlyphs :
            direction == Direction.Right ? _westGlyphs :
            direction == Direction.Up ? _northGlyphs : _southGlyphs)
        {
        }
        
        public override void SetAppearance()
        {
            switch (State)
            {
                case MovesInWaves.States.Dying: _animationIndex = 2; break;
                case MovesInWaves.States.Off: _animationIndex = 0; break;
                case MovesInWaves.States.On: _animationIndex = 1; break;
            }
            
            base.SetAppearance();
        }
    }
}