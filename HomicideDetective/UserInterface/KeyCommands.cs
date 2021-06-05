using HomicideDetective.People;
using HomicideDetective.Things;
using SadRogue.Integration;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.UserInterface
{
    /// <summary>
    /// The commands that map to key presses, that players can perform.
    /// </summary>
    public static class KeyCommands
    {
        /// <summary>
        /// Talks to each entity in the squares surrounding the player
        /// </summary>
        public static void Talk()
        {
            //Program.CurrentTime.Minutes++;
            var thoughts = Program.PlayerCharacter.AllComponents.GetFirst<Thoughts>();
            for (int i = Program.PlayerCharacter.Position.X - 1; i < Program.PlayerCharacter.Position.X + 2; i++)
            {
                for (int j = Program.PlayerCharacter.Position.Y - 1; j < Program.PlayerCharacter.Position.Y + 2; j++)
                {
                    if (Program.Map.Contains((i, j)) && (i,j) != Program.PlayerCharacter.Position)
                    {
                        foreach (var entity in Program.Map.GetEntitiesAt<RogueLikeEntity>((i,j)))
                        {
                            var speech = entity.AllComponents.GetFirstOrDefault<Speech>();
                            if (speech is not null)
                            {
                                thoughts.Think(speech.SpeakTo());
                            }
                        }
                    }
                }
            }

            Program.Page.Write(thoughts.CurrentThought.What);
        }

        /// <summary>
        /// looks at each entity in the squares surrounding the player
        /// </summary>
        public static void Look()
        {
            //Program.CurrentTime.Minutes++;
            var thoughts = Program.PlayerCharacter.AllComponents.GetFirst<Thoughts>();
            for (int i = Program.PlayerCharacter.Position.X - 1; i < Program.PlayerCharacter.Position.X + 2; i++)
            {
                for (int j = Program.PlayerCharacter.Position.Y - 1; j < Program.PlayerCharacter.Position.Y + 2; j++)
                {
                    if (Program.Map.Contains((i, j)) && Program.PlayerCharacter.Position != (i,j))
                    {
                        foreach (var entity in Program.Map.GetEntitiesAt<RogueLikeEntity>((i,j)))
                        {
                            var substantive = entity.AllComponents.GetFirstOrDefault<Substantive>();
                            if(substantive is not null)
                                thoughts.Think(substantive.GenerateDetailedDescription());
                        }
                    }
                }
            }

            Program.Page.Write(thoughts.CurrentThought.What);
        }
        
        /// <summary>
        /// looks at each entity in the squares surrounding the player
        /// </summary>
        public static void Inspect()
        {
            //Program.CurrentTime.Minutes++;
            var thoughts = Program.PlayerCharacter.AllComponents.GetFirst<Thoughts>();
            for (int i = Program.PlayerCharacter.Position.X - 1; i < Program.PlayerCharacter.Position.X + 2; i++)
            {
                for (int j = Program.PlayerCharacter.Position.Y - 1; j < Program.PlayerCharacter.Position.Y + 2; j++)
                {
                    if (Program.Map.Contains((i, j)) && Program.PlayerCharacter.Position != (i,j))
                    {
                        foreach (var entity in Program.Map.GetEntitiesAt<RogueLikeEntity>((i,j)))
                        {
                            var markings = entity.AllComponents.GetFirstOrDefault<MarkingCollection>();
                            if (markings is not null)
                            {
                                string thought = "";
                                thought += "On this entity is ";
                                foreach (var marking in markings.MarkingsOn)
                                    thought += $"{marking.Name}, ";
                                thoughts.Think(thought);
                            }
                        }
                    }
                }
            }

            Program.Page.Write(thoughts.CurrentThought.What);
        }
    }
}