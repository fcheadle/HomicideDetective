using System;
using System.Collections.Generic;
using System.Linq;
using HomicideDetective.UserInterface;

namespace HomicideDetective.People.Speech
{
    public class ConversationContext : CommandContext
    {
        public IEnumerable<Personhood> PeopleInvolved { get; set; }
        public IEnumerable<string> SubjectsBroughtUp { get; set; }
        public IEnumerable<DateTime> TimesDiscussed { get; set; }
        public IEnumerable<Dialogue> WordsSpoken { get; set; }

        public ConversationContext(IEnumerable<KeyCommand> contextCommands, IEnumerable<Personhood> peopleInvolved, IEnumerable<string> subjectsBroughtUp, IEnumerable<DateTime> timesDiscussed) 
            : base(contextCommands)
        {
            PeopleInvolved = peopleInvolved;
            SubjectsBroughtUp = subjectsBroughtUp;
            TimesDiscussed = timesDiscussed;
            WordsSpoken = new List<Dialogue>();
        }

        public void AddDialogue(Dialogue dialogue) => ((List<Dialogue>)WordsSpoken).Add(dialogue);
    }
}