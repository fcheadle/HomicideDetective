using GoRogue.Components.ParentAware;
using SadRogue.Integration;

namespace HomicideDetective.Places.Weather
{
    /// <summary>
    /// The blowing grass animation
    /// </summary>
    public class ReactsToWeatherComponent : ParentAwareComponentBase<RogueLikeCell>
    {
        //AnimatedConsole animation;
        protected readonly int OgGlyph;
        protected readonly int[] AnimationSteps;
        protected int AnimationIndex;
        public bool Animating { get; private set; }

        public ReactsToWeatherComponent(int glyph, int[] animationSteps)
        {
            OgGlyph = glyph;
            AnimationSteps = animationSteps;
        }

        public void Start()
        {
            Animating = true;
        }

        public void Stop()
        {
            Animating = false;
            Parent!.Appearance.Glyph = OgGlyph;
            AnimationIndex = 0;
        }

        public virtual void DoOneAnimationStep()
        {
            if (Animating)
            {
                if (AnimationIndex >= AnimationSteps.Length)
                    Stop();
                
                else
                {
                    SetAppearance();
                    AnimationIndex++;   
                }
            }
        }

        public virtual void SetAppearance()
        {
            Parent!.Appearance.Glyph = AnimationSteps[AnimationIndex];
        }
    }
}