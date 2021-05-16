using System.Collections.Generic;

namespace HomicideDetective.Mysteries
{
    //todo - refactor out?
    public interface IDetailed
    {
        List<string> Details { get; }
    }
}