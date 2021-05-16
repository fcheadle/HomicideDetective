using HomicideDetective.Mysteries;
using HomicideDetective.People;
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
                                thoughts.Think(speech.Details.ToArray());
                            }
                        }
                    }
                }
            }

            Program.Page.Print();
        }

        /// <summary>
        /// looks at each entity in the squares surrounding the player
        /// </summary>
        public static void Look()
        {
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
                                thoughts.Think(substantive.Details);
                        }
                    }
                }
            }

            Program.Page.Print();
        }
        
    }
}