using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadConsole;
using System;

namespace Engine.Components.Terrain
{
    //Can't add ComponentBase to terrain objects, and we need this to be able to do that
    public class AnimateGlyphComponent : IGameObjectComponent//ComponentBase
    {
        //AnimatedConsole animation;
        int _ogGlyph;
        int[] _animationSteps;
        int _animationIndex = 0;
        TimeSpan _interval = TimeSpan.FromMilliseconds(100);
        TimeSpan _counter = TimeSpan.FromSeconds(0);

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
