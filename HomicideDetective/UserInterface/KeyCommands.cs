using System.Collections.Generic;
using System.Linq;
using HomicideDetective.People;
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

        /// <summary>
        /// looks at each entity in the squares surrounding the player
        /// </summary>
        public static void LookAround()
        {
            var mystery = Program.CurrentGame.Mystery;
            var place = mystery.CurrentPlaceInfo(Player.Position);
            var contents = place.Name;
            contents += "\r\n";
            contents += place.Description;
            contents += "\r\n";
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
            
            MessageWindow.Write(contents);
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
                                
                                //var pageContents = new PageContentSource($"Markings on {entity.Name}", thought);
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
            var surface = game.MessageWindow;
            var keybindings = game.ActionNames;
            surface.Clear();
            
            string help = "";
            foreach (var command in keybindings)
            {
                var keyString = command.Key.ToString();
                if (keyString.Contains("OemQuestion"))
                    keyString = "?";
                help += $"{keyString}: {command.Value}\r\n";
            }

            surface.Write(help);
        }
    }
}