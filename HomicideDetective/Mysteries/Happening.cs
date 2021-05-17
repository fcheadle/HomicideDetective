namespace HomicideDetective.Mysteries
{
    /// <summary>
    /// A happening is a thing that happened at a time.
    /// </summary>
    /// <remarks>Use pretty sentences, please.</remarks>
    public class Happening
    {
        public Time OccurredAt { get; set; }
        public string Occurrence { get; set; }
        
        public Happening(Time occurredAt, string occurrence)
        {
            Occurrence = occurrence;
            OccurredAt = occurredAt;
        }

        public override string ToString() => $"At {OccurredAt.ToString()}, {Occurrence}";
        
    }
}