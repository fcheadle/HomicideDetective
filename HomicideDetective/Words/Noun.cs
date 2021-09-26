namespace HomicideDetective.Words
{
    public class Noun
    {
        public string Singular { get; set; }
        public string Plural { get; set; }
        
        public Noun(string singular, string plural)
        {
            Singular = singular;
            Plural = plural;
        }
    }
}