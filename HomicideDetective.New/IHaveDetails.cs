namespace HomicideDetective.New
{
    public interface IHaveDetails
    {
        string Name { get; }
        string Description { get; }
        string[] GetDetails();
    }
}