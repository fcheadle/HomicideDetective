using HomicideDetective.UserInterface;

namespace HomicideDetective.Places
{
    public class MovesInWaves : AnimatingGlyphComponent
    {
        public enum States {On, Dying, Off}
        public States State;
        public States NextState;
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
            switch (State)
            {
                case States.Dying: _animationIndex = 0; break;
                case States.Off: _animationIndex = 1; break;
                case States.On: _animationIndex = 2; break;
            }
            
            base.SetAppearance();
        }
    }
}