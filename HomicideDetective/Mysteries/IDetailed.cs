namespace HomicideDetective.Mysteries
{
    public interface IDetailed
    {
        string Name { get; }
        string Description { get; }
        string[] GetDetails();
        string[] AllDetails();
    }
}