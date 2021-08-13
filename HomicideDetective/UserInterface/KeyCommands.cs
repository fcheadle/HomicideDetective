using System.Collections.Generic;
using System.Linq;
using HomicideDetective.People;
using HomicideDetective.Places;
using HomicideDetective.Things;
using SadRogue.Integration;
using SadRogue.Integration.Maps;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.UserInterface
{
    /// <summary>
    /// The commands that map to key presses, that players can perform.
    /// </summary>
    public static class KeyCommands
    {
        private static RogueLikeEntity Player => Program.CurrentGame.PlayerCharacter;
        private static RogueLikeMap Map => Program.CurrentGame.Map;
        private static PageWindow MessageWindow => Program.CurrentGame.MessageWindow;
        /// <summary>
        /// Talks to each entity in the squares surrounding the player
        /// </summary>
        public static void Talk()
        {
            var title = $"Conversation with ";
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
                                title += entity.Name;
                                var contents = component.SpeakTo();
                                MessageWindow.Write(title, contents);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// looks at each entity in the squares surrounding the player
        /// </summary>
        public static void Look()
        {
            var mystery = Program.CurrentGame.Mystery;
            var place = mystery.CurrentPlaceInfo(Player.Position);
            var title = place.Name;
            var contents = place.Description;
            var entitiesVisible = new List<RogueLikeEntity>();
            foreach (var point in mystery.CurrentLocation.PlayerFOV.CurrentFOV)
            {
                if (point != Player.Position)
                {
                    entitiesVisible.AddRange(Map.GetEntitiesAt<RogueLikeEntity>(point));
                }
            }
            
            if(entitiesVisible.Any())
            {
                contents += " I see";
                foreach(var entity in entitiesVisible)
                {
                    var subs = entity.AllComponents.GetFirstOrDefault<ISubstantive>(); 
                    if (subs != null)
                        contents += " " + subs.Name + ",";
                    
                }
            }

            else
            {
                contents += " There are no people nor items in sight.";
            }
            // for (int i = Player.Position.X - 1; i < Player.Position.X + 2; i++)
            // {
            //     for (int j = Player.Position.Y - 1; j < Player.Position.Y + 2; j++)
            //     {
            //         if (Map.Contains((i, j)) && Player.Position != (i,j))
            //         {
            //             foreach (var entity in Map.GetEntitiesAt<RogueLikeEntity>((i,j)))
            //             {
            //                 var component = entity.AllComponents.GetFirstOrDefault<Personhood>();
            //                 contents += $"{component.GetPrintableString()}, ";
            //             }
            //         }
            //     }
            // }
            
            MessageWindow.Write(title, contents);
        }
        
        /// <summary>
        /// looks at each entity in the squares surrounding the player
        /// </summary>
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
                                
                                var pageContents = new PageContentSource($"Markings on {entity.Name}", thought);
                                MessageWindow.Write(pageContents);
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
    }
}