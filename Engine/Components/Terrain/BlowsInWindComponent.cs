using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadConsole;
using System;

namespace Engine.Components.Terrain
{
    //Can't add IGameFramProcessor to terrain objects
    public class BlowsInWindComponent : IGameObjectComponent//ComponentBase
    {
        //AnimatedConsole animation;
        int _ogGlyph;
        int[] _animationSteps = { '/', '|', '(', '\\', '|', ')' };
        int _animationIndex = 0;
        TimeSpan _interval = TimeSpan.FromMilliseconds(100);
        TimeSpan _counter = TimeSpan.FromSeconds(0);

        public IGameObject Parent { get; set; }
        public bool Blowing { get; internal set; }

        public BlowsInWindComponent(int glyph)// : base(true, false, true, false)
        {
            _ogGlyph = glyph;
        }

        public void Update(TimeSpan delta)
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

        public string[] GetDetails()
        {
            string[] answer =
            {
                "This entity blows in the wind."
            };
            return answer;
        }

        public void Blow()
        {
            Blowing = true;
        }
    }
}
