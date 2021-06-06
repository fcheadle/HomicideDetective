using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadRogue.Integration;

namespace HomicideDetective.UserInterface
{
    /// <summary>
    /// The blowing grass animation
    /// </summary>
    public class AnimatingGlyphComponent : IGameObjectComponent
    {
        //AnimatedConsole animation;
        protected readonly int _ogGlyph;
        protected readonly int[] _animationSteps;
        protected int _animationIndex;
        public IGameObject Parent { get; set; }
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
            var parent = (RogueLikeCell)Parent;
            parent.Appearance.Glyph = _ogGlyph;
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
            ((RogueLikeCell)Parent).Appearance.Glyph = _animationSteps[_animationIndex];
        }
    }
}