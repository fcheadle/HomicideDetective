using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadRogue.Integration;

namespace HomicideDetective.UserInterface
{
    /// <summary>
    /// The blowing grass animation
    /// </summary>
    public class AnimatingGlyph : IGameObjectComponent
    {
        //AnimatedConsole animation;
        readonly int _ogGlyph;
        readonly int[] _animationSteps;
        int _animationIndex;
        public IGameObject Parent { get; set; }
        public bool Animating { get; private set; }

        public AnimatingGlyph(int glyph, int[] animationSteps)
        {
            _ogGlyph = glyph;
            _animationSteps = animationSteps;
        }

        public void Start()
        {
            Animating = true;
        }

        public void Stop()
        {
            Animating = false;
            var parent = (RogueLikeCell)Parent;
            parent.Appearance.Glyph = _ogGlyph;
        }

        public void ProcessTimeUnit()
        {
            if (Animating)
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