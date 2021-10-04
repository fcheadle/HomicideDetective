namespace HomicideDetective.Places.Weather
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
                case States.Dying: AnimationIndex = 0; break;
                case States.Off: AnimationIndex = 1; break;
                case States.On: AnimationIndex = 2; break;
            }
            
            base.SetAppearance();
        }
    }
}