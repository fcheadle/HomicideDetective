using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using TheSadRogue.Integration;

namespace HomicideDetective.Places.Components
{
    //Refactor this into a "blowing grass" entity that disposes of itself when finished
    public class ChangingGlyphComponent : IGameObjectComponent
    {
        //AnimatedConsole animation;
        readonly int _ogGlyph;
        readonly int[] _animationSteps;
        int _animationIndex;
        public IGameObject Parent { get; set; }
        public bool Blowing { get; internal set; }

        public ChangingGlyphComponent(int glyph, int[] animationSteps)
        {
            _ogGlyph = glyph;
            _animationSteps = animationSteps;
        }
        public string[] GetDetails()
        {
            string[] answer =
            {
                "This entity blows in the wind."
            };
            return answer;
        }

        public void Start()
        {
            Blowing = true;
        }

        public void Stop()
        {
            Blowing = false;
            var parent = (RogueLikeCell)Parent;
            parent.Appearance.Glyph = _ogGlyph;
        }

        public void ProcessTimeUnit()
        {
            if (Blowing)
            {
                var parent = (RogueLikeCell)Parent;
                parent.Appearance.Glyph = _animationSteps[_animationIndex];
                _animationIndex++;
                if (_animationIndex >= _animationSteps.Length)
                {
                    _animationIndex = 0;
                    parent.Appearance.Glyph = _ogGlyph;
                    Blowing = false;
                }
            }
        }
    }
}