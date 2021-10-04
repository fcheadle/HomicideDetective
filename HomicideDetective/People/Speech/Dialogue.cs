using System;

namespace HomicideDetective.People.Speech
{
    public class Dialogue
    {
        public ISubstantive Speaker { get; }
        public string Spoken { get; }
        public DateTime AtTime { get; }
        
        public Dialogue(ISubstantive speaker, string spoken, DateTime atTime)
        {
            Speaker = speaker;
            Spoken = spoken;
            AtTime = atTime;
        }

        public Dialogue(ISubstantive speaker, string spoken)
        {
            Speaker = speaker;
            Spoken = spoken;
            AtTime = Program.CurrentGame.CurrentTime;
        }
    }
}