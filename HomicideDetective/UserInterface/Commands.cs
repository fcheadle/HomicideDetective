using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.UserInterface
{
    /// <summary>
    /// The commands that map to key presses, that players can perform.
    /// </summary>
    public static class Commands
    {
        private static RogueLikeEntity Player => Program.CurrentGame.PlayerCharacter;
        private static RogueLikeMap Map => Program.CurrentGame.Map;
        private static PageWindow MessageWindow => Program.CurrentGame.MessageWindow;
        private static Mystery Mystery => Program.CurrentGame.Mystery;

        #region Speech
        public static void Talk()
        {
            for (int i = Player.Position.X - 1; i < Player.Position.X + 2; i++)
            {
                for (int j = Player.Position.Y - 1; j < Player.Position.Y + 2; j++)
                {
                    if (Map.Contains((i, j)) && (i,j) != Player.Position)
                    {
                        foreach (var entity in Map.GetEntitiesAt<RogueLikeEntity>((i,j)))
                        {
                            var component = entity.AllComponents.GetFirstOrDefault<Personhood>();
                            if (component is not null)
                            {
                                var contents = component.SpeakTo();
                                MessageWindow.Write(contents);
                            }
                        }
                    }
                }
            }
        }

        public static void TalkTo(Point position)
        {
            if (Map.Contains(position) && position != Player.Position)
            {
                foreach (var entity in Map.GetEntitiesAt<RogueLikeEntity>(position))
                {
                    var component = entity.AllComponents.GetFirstOrDefault<Personhood>();
                    if (component is not null)
                    {
                        var contents = component.SpeakTo();
                        MessageWindow.Write(contents);
                    }
                }
            }
        }

        public static void Greet()
        {
            if(Program.CurrentGame.Context.Subject is Personhood person)
                Greet(person);
        }
        public static void Greet(Personhood partner)
            => MessageWindow.Write(partner.Greet());

        public static void InquireAboutSelf()
        {
            if(Program.CurrentGame.Context.Subject is Personhood person)
                InquireAboutSelf(person);
        }
        public static void InquireAboutSelf(Personhood partner)
            => MessageWindow.Write(partner.InquireAboutSelf());
        
        // public static void InquireWhereabouts()
        // {
        //     if(Program.CurrentGame.Context.Subject is Personhood person)
        //         InquireWhereabouts(person);
        // }
        // public static void InquireWhereabouts(Personhood partner, DateTime at)
        //     => MessageWindow.Write(partner.InquireWhereabouts(at));
        // public static void InquireWhoWithAtTime()
        // {
        //     if(Program.CurrentGame.Context.Subject is Personhood person)
        //         InquireWhoWithAtTime(person);
        // }
        // public static void InquireWhoWithAtTime(Personhood partner, DateTime at)
        //     => MessageWindow.Write(partner.InquireAboutCompany(at));
        // public static void InquireAboutMemory()
        // {
        //     if(Program.CurrentGame.Context.Subject is Personhood person)
        //         InquireAboutSelf(person);
        // }
        // public static void InquireAboutMemory(Personhood partner, DateTime at)
        //     => MessageWindow.Write(partner.InquireAboutMemory(at));
        // public static void InquireAboutPlace()
        // {
        //     if(Program.CurrentGame.Context.Subject is Personhood person)
        //         InquireAboutPlace(person);
        // }
        // public static void InquireAboutPlace(Personhood partner, string place)
        //     => MessageWindow.Write(partner.InquireAboutPlace(place));
        // public static void InquireAboutPerson()
        // {
        //     if(Program.CurrentGame.Context.Subject is Personhood person)
        //         InquireAboutPerson(person);
        // }
        // public static void InquireAboutPerson(Personhood partner, string name)
        //     => MessageWindow.Write(partner.InquireAboutPerson(name));
        // public static void InquireAboutThing()
        // {
        //     if(Program.CurrentGame.Context.Subject is Personhood person)
        //         InquireAboutThing(person);
        // }
        // public static void InquireAboutThing(Personhood partner, string thing)
        //     => MessageWindow.Write(partner.InquireAboutThing(thing));
        
        #endregion
        public static void LookAround()
        {
            var place = Mystery.CurrentPlaceInfo(Player.Position);
            var contents = place.Name;
            contents += "\r\n";
            contents += place.Description;
            contents += "\r\n";
            var entitiesVisible = new List<RogueLikeEntity>();
            foreach (var point in Mystery.CurrentLocation.PlayerFOV.CurrentFOV)
            {
                if (point != Player.Position)
                {
                    entitiesVisible.AddRange(Map.GetEntitiesAt<RogueLikeEntity>(point));
                }
            }
            
            if(entitiesVisible.Any())
            {
                foreach(var entity in entitiesVisible)
                {
                    contents += " I see";
                    var subs = entity.AllComponents.GetFirstOrDefault<ISubstantive>(); 
                    if (subs != null)
                        contents += $" {subs.Name}\r\n {subs.Description}.\r\n";
                }
            }

            else
            {
                contents += " There are no people nor items in sight.";
            }
            
            MessageWindow.Write(contents);
        }

        public static void LookAt(Point position)
        {
            var entities = Map.GetEntitiesAt<RogueLikeEntity>(position);
            var sb = new StringBuilder();
            if (entities.Any())
            {
                sb.Append("Before me is ");
                foreach (var entity in entities)
                {
                    var subs = entity.GoRogueComponents.GetFirstOrDefault<ISubstantive>();
                    if (subs != null)
                        sb.Append($"{subs.Name}, ");
                }
            }
        }
        
        public static void Inspect()
        {
            for (int i = Program.CurrentGame.PlayerCharacter.Position.X - 1; i < Program.CurrentGame.PlayerCharacter.Position.X + 2; i++)
            {
                for (int j = Program.CurrentGame.PlayerCharacter.Position.Y - 1; j < Program.CurrentGame.PlayerCharacter.Position.Y + 2; j++)
                {
                    if (Program.CurrentGame.Map.Contains((i, j)) && Program.CurrentGame.PlayerCharacter.Position != (i,j))
                    {
                        foreach (var entity in Program.CurrentGame.Map.GetEntitiesAt<RogueLikeEntity>((i,j)))
                        {
                            var component = entity.AllComponents.GetFirstOrDefault<Personhood>();
                            if(component is not null)
                            {
                                string thought = "";
                                thought += "On this entity is ";
                                
                                foreach (var marking in component.Markings.MarkingsOn)
                                    thought += $"{marking}, ";
                                
                                MessageWindow.Write(thought);
                            }
                        }
                    }
                }
            }
        }

        public static void NextMap()
        {
            Program.CurrentGame.NextMap();
        }

        public static void PrintCommands()
        {
            var game = Program.CurrentGame;
            var keybindings = game.Commands;
            
            string help = "";
            foreach (var command in keybindings)
            {
                var keyString = command.Key.ToString();
                if (keyString.Contains("OemQuestion"))
                    keyString = "?";
                help += $"{keyString}: {command.Name}\r\n";
            }

            MessageWindow.Write(help);
        }
        
        
        public static void BackPage()
        {
            var page = Program.CurrentGame.MessageWindow;
            if (page.PageNumber > 0)
                page.PrintContents(--page.PageNumber);
        }
        
        public static void ForwardPage()
        {
            var page = Program.CurrentGame.MessageWindow;
            if (page.PageNumber < page.Contents.Count - 1)
                page.PrintContents(++page.PageNumber);
        }

        public static void UpOneLine()
        {
            var page = Program.CurrentGame.MessageWindow;
            page.TextSurface.Surface.ViewPosition -= (0, 1);
            page.TextSurface.IsDirty = true;
        }

        public static void DownOneLine()
        {
            var page = Program.CurrentGame.MessageWindow;
            page.TextSurface.Surface.ViewPosition += (0, 1);
            page.TextSurface.IsDirty = true;
        }

        //temporary - for testing
        public static void WriteGarbage()
        {
            var page = Program.CurrentGame.MessageWindow;
            page.Clear();
            string contents = "home ";
            
            for (int i = 0; i < 1000; i++)
                contents += $"{(char)(i % 256)} ";
            
            page.Write(contents);
        }

    }
}