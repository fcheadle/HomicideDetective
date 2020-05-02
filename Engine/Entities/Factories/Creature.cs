using Engine.Components;
using Engine.Maps;
using Engine.UI;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Engine.Entities.Factories
{
    public class Creature : BasicEntity
    {
        internal new TownMap CurrentMap;
        internal int FOVRadius { get; private set; }
        internal Items.Inventory Inventory = new Items.Inventory();
        
        protected SadConsole.Maps.Region _currentRegion;


        public Creature(Coord position, Color foreground, int glyph, int fovRadius)
            : base(foreground, Color.Black, glyph, position, 1, isWalkable: false, isTransparent: true)
        {
            FOVRadius = fovRadius;
        }

        public static Creature Person(Coord position)
        {
            Color color;
            int chance = Calculate.Percent();
            if (chance < 30)
                color = ColorExtensions.MutateBy(Color.Brown, Color.Black);
            else if (chance < 60)
                color = Color.DarkKhaki;
            else if (chance < 80)
                color = Color.Khaki;
            else if (chance < 90)
                color = Color.Tan;
            else if (chance < 99)
                color = ColorExtensions.MutateBy(Color.Tan, Color.White);
            else
                color = Color.White;
            Creature critter = new Creature(position, color, 1, 15);

            //default healthy stats
            HealthComponent health = new HealthComponent();
            health.SystoleBloodPressure = 120;
            health.DiastoleBloodPressure = 80;
            health.Pulse = 85; //bpm
            health.BloodVolume = 5000; //milliliters
            health.BodyTemperature = 98.6;
            critter.AddGoRogueComponent(health);
            return critter;

        }
        public static Creature Animal(Coord position)
        {
            Creature critter = new Creature(position, Color.Gray, 224, 14);
            //default is dog i guess?
            HealthComponent health = new HealthComponent();
            health.SystoleBloodPressure = 120;
            health.DiastoleBloodPressure = 80;
            health.Pulse = 85; //bpm
            health.BloodVolume = 5000; //milliliters
            health.BodyTemperature = 102.5;
            critter.AddGoRogueComponent(health);
            return critter;
        }
    }
}
