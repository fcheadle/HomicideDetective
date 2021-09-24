namespace HomicideDetective.Words
{
    public class Pronoun
    {
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Possessive { get; set; }
        public string Reflexive { get; set; }
        
        public Pronoun(string subjective, string objective, string possessive, string reflexive)
        {
            Subjective = subjective;
            Objective = objective;
            Possessive = possessive;
            Reflexive = reflexive;
        }
    }
}