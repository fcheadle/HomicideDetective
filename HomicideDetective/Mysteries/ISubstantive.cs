namespace HomicideDetective.Mysteries
{
    /// <summary>
    /// A Substantive is a Person, Place, or Thing.
    /// </summary>
    public interface ISubstantive
    {
        public enum Types { Person,Place,Thing }
        public Types? Type { get; } 
        public string Name { get; } 
        public string Description { get; }
        public int Mass { get; }//in grams
        public int Volume { get; }//in cm^3
        public string SizeDescription { get; }
        public string WeightDescription { get; }
        public string[] Details { get; }
        public string GetDetailedDescription();
        public void AddDetail(string detail);
    }
}