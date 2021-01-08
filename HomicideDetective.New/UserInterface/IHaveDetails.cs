namespace ExampleGame.UserInterface
{
    public interface IHaveDetails
    {
        string Name { get; }
        string Description { get; }
        string[] GetDetails();
    }
}