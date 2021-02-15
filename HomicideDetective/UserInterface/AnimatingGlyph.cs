using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using TheSadRogue.Integration;

namespace HomicideDetective.UserInterface
{
    //Refactor this into a "blowing grass" entity that disposes of itself when finished
    public class AnimatingGlyph : IGameObjectComponent
    {
        //AnimatedConsole animation;
        readonly int _ogGlyph;
        readonly int[] _animationSteps;
        int _animationIndex;
        public IGameObject Parent { get; set; }
        public bool Blowing { get; private set; }

        public AnimatingGlyph(int glyph, int[] animationSteps)
        {
            _ogGlyph = glyph;
            _animationSteps = animationSteps;
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
                    Stop();
                }
            }
        }
    }
}