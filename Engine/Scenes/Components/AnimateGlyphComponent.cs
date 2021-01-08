using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadConsole;

namespace Engine.Scenes.Components
{
    //Refactor this into a "blowing grass" entity that disposes of itself when finished
    public class AnimateGlyphComponent : IGameObjectComponent//ComponentBase
    {
        //AnimatedConsole animation;
        readonly int _ogGlyph;
        readonly int[] _animationSteps;
        int _animationIndex;
        public IGameObject Parent { get; set; }
        public bool Blowing { get; internal set; }

        public AnimateGlyphComponent(int glyph, int[] animationSteps)// : base(true, false, true, false)
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
            var parent = (BasicTerrain)Parent;
            parent.Glyph = _ogGlyph;
        }

        public void ProcessTimeUnit()
        {
            if (Blowing)
            {
                var parent = (BasicTerrain)Parent;
                parent.Glyph = _animationSteps[_animationIndex];
                _animationIndex++;
                if (_animationIndex >= _animationSteps.Length)
                {
                    _animationIndex = 0;
                    parent.Glyph = _ogGlyph;
                    Blowing = false;
                }

            }
        }
    }
}
