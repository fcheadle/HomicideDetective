using HomicideDetective.UserInterface;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace HomicideDetective.Places
{
    public class MovesInWaves : AnimatingGlyphComponent
    {
        public enum States {On, Dying, Off}
        public States State;
        public States NextState;
        private Color _ogColor = Color.Transparent;
        public MovesInWaves() : base('-', new[]
        {
            126, //126, 126, 126, 126, 126, 126, 126, 126, 126, 
            '-', //'-', '-', '-', '-', '-', '-', '-', '-', '-', 
            247, //247, 247, 247, 247, 247, 247, 247, 247, 247, 
        })
        {
        }
        
        

        public override void SetAppearance()
        {
            if (_ogColor == Color.Transparent)
                _ogColor = ((RogueLikeCell) Parent).Appearance.Foreground;
            
            switch (State)
            {
                case States.Dying: _animationIndex = 0; break;
                case States.Off: _animationIndex = 1; break;
                case States.On: _animationIndex = 2; break;
            }

            //SetColor();
            base.SetAppearance();
        }

        private void SetColor()
        {
            switch (State)
            {
                case States.Dying: ((RogueLikeCell) Parent).Appearance.Foreground = Color.Blue; break;
                case States.Off: ((RogueLikeCell) Parent).Appearance.Foreground = _ogColor; break;
                case States.On: ((RogueLikeCell) Parent).Appearance.Foreground = Color.DarkBlue; break;
            }
        }
    }
}