using HomicideDetective.Places.Weather;
using HomicideDetective.UserInterface;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace HomicideDetective.Places
{
    public class MovesInWaves : ReactsToWeatherComponent
    {
        public enum States {On, Dying, Off}
        public States State;
        public MovesInWaves() : base('-', new[] {126, '-', 247,})
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