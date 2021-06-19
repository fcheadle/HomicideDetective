using GoRogue.Components.ParentAware;
using SadRogue.Integration;

namespace HomicideDetective.UserInterface
{
    /// <summary>
    /// The blowing grass animation
    /// </summary>
    public class AnimatingGlyphComponent : ParentAwareComponentBase<RogueLikeCell>
    {
        //AnimatedConsole animation;
        protected readonly int _ogGlyph;
        protected readonly int[] _animationSteps;
        protected int _animationIndex;
        public bool Animating { get; private set; }

        public AnimatingGlyphComponent(int glyph, int[] animationSteps)
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
            Parent.Appearance.Glyph = _ogGlyph;
            _animationIndex = 0;
        }

        public virtual void ProcessTimeUnit()
        {
            if (Animating)
            {
                if (_animationIndex >= _animationSteps.Length)
                    Stop();
                
                else
                {
                    SetAppearance();
                    _animationIndex++;   
                }
            }
        }

        public virtual void SetAppearance()
        {
            Parent.Appearance.Glyph = _animationSteps[_animationIndex];
        }
    }
}