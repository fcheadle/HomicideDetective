using Engine.Creatures;
using Engine.Maps;
using Microsoft.Xna.Framework;
using System;

namespace Engine
{
    internal class Program
    {
        internal static UI.GameScreen MapScreen { get; private set; }
        internal static TimeSpan UpdateCounter { get; private set; } = TimeSpan.FromSeconds(0);

        internal static void Main()
        {
            SadConsole.Game.Create(Settings.GameWidth, Settings.GameHeight);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Update(GameTime obj)
        {
            // jk... unless...?
            UpdateCounter += obj.ElapsedGameTime;
            //every tenth of a second
            if(UpdateCounter > TimeSpan.FromMilliseconds(250))
            {
                foreach(IActor actor in MapScreen.Map.GetCreatures())
                {
                    actor.Act();
                }
                UpdateCounter = TimeSpan.FromMilliseconds(0);
                MapScreen.IsDirty = true;
            }
        }

        private static void Init()
        {
            MapScreen = new UI.GameScreen(Settings.MapWidth, Settings.MapHeight, Settings.GameWidth, Settings.GameHeight, Settings.FOVRadius);
            SadConsole.Global.CurrentScreen = MapScreen;
        }
    }
}
