namespace HomicideDetective.Things
{
    /// <summary>
    /// A "Marking" is left by an interaction. Includes fingerprints, scuff marks, blood stains, bruises, so on.
    /// </summary>
    public class Marking
    {
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
        public string? Adjective { get; set; }
    }
}
