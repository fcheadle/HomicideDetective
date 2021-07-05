using System.Collections.Generic;

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
        public ISubstantive.Types LeftOn { get; set; }
        public IEnumerable<Marking>? LeavesFurtherMarkings { get; set; }

        public Marking(string name = null!, string color = null!, string description = null!, string adjective = null!,
            ISubstantive.Types leftOn = ISubstantive.Types.Person, IEnumerable<Marking> leavesFurtherMarkings = null!)
        {
            Name = name;
            Color = color;
            Description = description;
            Adjective = adjective;
            LeftOn = leftOn;
            LeavesFurtherMarkings = leavesFurtherMarkings ?? new List<Marking>();
        }

        public string GetPrintableString() => $"{Description} {Adjective} {Color} {Name}";
    }
}
