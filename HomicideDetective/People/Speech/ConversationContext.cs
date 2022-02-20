using System;
using System.Collections.Generic;
using System.Text;
using HomicideDetective.UserInterface;
using SadConsole.Input;

namespace HomicideDetective.People.Speech
{
    public class ConversationContext : CommandContext
    {
        private readonly Keys[] _buttons = new[]
        {
            Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M,
            Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z,
        };
        public Personhood WithPerson { get; set; }
        private Personhood Detective { get; }
        public IEnumerable<string> SubjectsBroughtUp { get; set; }
        public IEnumerable<DateTime> TimesDiscussed { get; set; }
        public IEnumerable<Dialogue> WordsSpoken { get; set; }
        
        private DateTime _timeBeingDiscussed = Program.CurrentGame.CurrentTime;
        
        public ConversationContext(IEnumerable<KeyCommand> contextCommands, Personhood withPerson, IEnumerable<string> subjectsBroughtUp, IEnumerable<DateTime> timesDiscussed) 
            : base(contextCommands)
        {
            WithPerson = withPerson;
            SubjectsBroughtUp = subjectsBroughtUp;
            TimesDiscussed = timesDiscussed;
            WordsSpoken = new List<Dialogue>();

            Detective = PersonFactory.GeneratePersonalInfo(0, "Jones");
            Detective.Name = "Detective";
        }

        public void AddDialogue(Dialogue dialogue) 
            => ((List<Dialogue>)WordsSpoken).Add(dialogue);

        public static ConversationContext GreetContext(IEnumerable<KeyCommand> contextCommands, Personhood withPerson, IEnumerable<string> subjectsBroughtUp, IEnumerable<DateTime> timesDiscussed)
        {
            var context = new ConversationContext(contextCommands, withPerson, subjectsBroughtUp, timesDiscussed);
            context.AddGreetings();
            return context;
        }
        public static ConversationContext GeneralConversationContext(IEnumerable<KeyCommand> contextCommands, Personhood withPerson, IEnumerable<string> subjectsBroughtUp, IEnumerable<DateTime> timesDiscussed)
        {
            var context = new ConversationContext(contextCommands, withPerson, subjectsBroughtUp, timesDiscussed);
            context.AddGreetings();
            return context;
        }
        private int CreateCommand(string name, string description, Action action, int index)
        {
            AddCommand(new KeyCommand(name, description, _buttons[index], false, false, false, action));
            return ++index;
        }
        private void AddCommand(KeyCommand command) => ((List<KeyCommand>)Commands).Add(command);
        private void ClearCommands() => ((List<KeyCommand>)Commands).Clear();
        private void AddGreetings()
        {
            ClearCommands();
            var index = 0;
            
            // Greeting
            Action greet = () =>
            {
                var call = new Dialogue(Detective, "Hello.");
                AddDialogue(call);
                string response = WithPerson.Greet();
                var dialogue = new Dialogue(WithPerson, response);
                AddDialogue(dialogue);
                AddGreetings();
            };
            index = CreateCommand("Greet", "Greet listener", greet, index);
            
            // Introduction
            if (!WithPerson.HasIntroduced)
            {
                Action introduce = () => 
                {
                    var call = new Dialogue(Detective, "I'm the Detective working on the case.");
                    AddDialogue(call);
                    string response = WithPerson.Introduce();
                    var dialogue = new Dialogue(WithPerson, response);
                    AddDialogue(dialogue);
                    AddGreetings();
                };
                index = CreateCommand("Introduce", "Introduce yourself to listener", introduce, index);
            }
            
            // Describe yourself
            if (!WithPerson.HasToldAboutSelf)
            {
                Action inquire = () => 
                {
                    var call = new Dialogue(Detective, "Tell me about yourself?");
                    AddDialogue(call);
                    string response = WithPerson.InquireAboutSelf();
                    var dialogue = new Dialogue(WithPerson, response);
                    AddDialogue(dialogue);
                    AddGeneralDialogueOptions();
                };
                index = CreateCommand("Inquire about them", "Inquire about the listener", inquire, index);
            }
            
            // Next Menu
            Action bypass = () => AddGeneralDialogueOptions();
            index = CreateCommand("Skip Greeting", "Skip Greeting (brings up new menu)", bypass, index);
            
            UserInterface.Commands.PrintCommands();
        }
        private void AddGeneralDialogueOptions()
        {
            ClearCommands();
            var index = 0;
            
            Action inquireAboutPerson = () => AddPersonInquiryDialogue();
            index = CreateCommand("Inquire about someone", "Ask the listener about someone else (brings up new menu)", inquireAboutPerson, index);

            Action inquireAboutPlace = () => AddPlaceInquiryDialogue();
            index = CreateCommand("Inquire about a place", "Ask the listener about a place (brings up new menu)", inquireAboutPlace, index);
            
            Action inquireAboutThing = () => AddThingInquiryDialogue();
            index = CreateCommand("Inquire about a thing", "Ask the listener about some object (brings up new menu)", inquireAboutThing, index);

            Action inquireAboutMemory = () => AddDaySelection();
            index = CreateCommand("Ask about a time", "Ask the listener what they were doing at a certain time (brings up new menu)", inquireAboutMemory, index);

            UserInterface.Commands.PrintCommands();
        }
        private void AddDaySelection()
        {
            ClearCommands();

            for (int index = 0; index < 15; index++)
            {
                var date = Program.CurrentGame.CurrentTime - TimeSpan.FromDays(index);
                Action today = () =>
                {
                    SelectDay(date);
                    AddHourSelection();
                };
                string dayPrefix = "";
                if (index == 0) 
                    dayPrefix = "Today, ";
                else if (index == 1)
                    dayPrefix = "Yesterday, ";

                CreateCommand($"{dayPrefix}{date.Month}/{date.Day}/{date.Year}", "", today, index);
            }
            
            // Action today = () => SelectDay(Program.CurrentGame.CurrentTime);
            // index = CreateCommand($"Today, {Program.CurrentGame.CurrentTime}", "Ask about today", today, index);
            //
            // Action yesterday = () => SelectDay(Program.CurrentGame.CurrentTime - TimeSpan.FromDays(1));
            // index = CreateCommand($"Today, {Program.CurrentGame.CurrentTime - TimeSpan.FromDays(1)}", "Ask about yesterday", today, index);
            
            UserInterface.Commands.PrintCommands();
            //todo
        }

        private void AddHourSelection()
        {
            ClearCommands();
            for (int index = 0; index < 24; index++)
            {
                var time = _timeBeingDiscussed.Date + TimeSpan.FromHours(index);
                Action today = () =>
                {
                    SelectTime(time.TimeOfDay);
                    AddMinuteSelection();
                };
                CreateCommand($"{time}","", today, index);
            }
            UserInterface.Commands.PrintCommands();
        }

        private void AddMinuteSelection()
        {
            ClearCommands();
            for (int index = 0; index < 12; index++)
            {
                var time = _timeBeingDiscussed + TimeSpan.FromMinutes(index * 5);
                Action today = () =>
                {
                    SelectTime(time.TimeOfDay);
                    AddInquiryOfSelectedTime();
                };
                CreateCommand($"{time}","", today, index);
            }
            UserInterface.Commands.PrintCommands();
        }

        private void SelectDay(DateTime date)
        {
            var day = date.Date;
            var time = _timeBeingDiscussed.TimeOfDay;
            _timeBeingDiscussed = new DateTime(day.Year, day.Month, day.Day, time.Hours, time.Minutes, time.Seconds);
        }

        private void SelectTime(TimeSpan timeOfDay)
        {
            var day = _timeBeingDiscussed.Date;
            var time = timeOfDay;
            _timeBeingDiscussed = new DateTime(day.Year, day.Month, day.Day, time.Hours, time.Minutes, time.Seconds);
        }
        private void AddInquiryOfSelectedTime()
        {
            ClearCommands();
            var index = 0;
            Action inquireAboutWhereabouts = () => AddInquiryOfWhereabouts();
            index = CreateCommand("Ask where listener was", "Ask the listener where they were at a certain time",inquireAboutWhereabouts, index);

            Action inquireAboutCompany = () => AddInquiryOfCompany();
            index = CreateCommand("Ask listener who they were with", "Ask the listener who they were with at a certain time", inquireAboutCompany, index);

            Action inquireAboutMemory = () => AddInquiryOfSpecificMemory();
            index = CreateCommand("Ask what listener was doing", "Ask the listener who they were with at a certain time", inquireAboutMemory, index);
            
            UserInterface.Commands.PrintCommands();
        }
        private void AddThingInquiryDialogue()
        {
            // todo - all relevant items heard about by the detective
            throw new NotImplementedException();
        }
        private void AddInquiryOfSpecificMemory()
        {
            AddDialogue(new Dialogue(Detective, $"What were you doing on {_timeBeingDiscussed}?"));
            AddDialogue(new Dialogue(WithPerson, WithPerson.InquireAboutMemory(_timeBeingDiscussed)));
        }

        private void AddInquiryOfCompany()
        {
            AddDialogue(new Dialogue(Detective, $"Who were you with on {_timeBeingDiscussed}?"));
            AddDialogue(new Dialogue(WithPerson, WithPerson.InquireAboutCompany(_timeBeingDiscussed)));
        }

        private void AddInquiryOfWhereabouts()
        {
            AddDialogue(new Dialogue(Detective, $"Where were you on {_timeBeingDiscussed}?"));
            AddDialogue(new Dialogue(WithPerson, WithPerson.InquireWhereabouts(_timeBeingDiscussed)));
        }

        private void AddPlaceInquiryDialogue()
        {
            // todo - all locations heard about by the detective
            throw new NotImplementedException();
        }

        private void AddPersonInquiryDialogue()
        {
            // todo - all persons of interest
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var dialogue in WordsSpoken)
                sb.Append($"{dialogue.Speaker.Name}: {dialogue.Spoken}\r\n");

            sb.Append(base.ToString());
            return sb.ToString();
        }
    }
}