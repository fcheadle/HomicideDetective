using SadRogue.Primitives;

namespace HomicideDetective.Words
{
    public static class Constants
    {
        #region component tags
        //people
        public const string MemoryComponentTag = "Memories";
        public const string FingerprintComponentTag = "Memories";
        public const string SpeechComponentTag = "Memories";
        
        //places
        public const string RegionCollectionTag = "RegionCollection";
        public const string GridViewTag = "Map";
        public const string WindyPlainsTag = "WindyPlains";
        public const string BodyOfWaterTag = "BodyOfWater";
        public const string PlaceCollectionTag = "PlaceCollection";
        public const string SubstantiveTag = "Info";
        public const string WeatherTag = "Weather";

        #endregion

        #region people strings
        public static readonly Noun MaleNouns = new Noun("man", "men");
        public static readonly Noun FemaleNouns = new Noun("woman", "women");
        public static readonly Noun ChildNouns = new Noun("child", "children");
        public static readonly Noun GenderNeutralNouns = new Noun("person", "people");
        public static readonly Pronoun MalePronouns = new Pronoun("he", "him", "his", "himself");
        public static readonly Pronoun FemalePronouns = new Pronoun("she", "her", "her", "herself");
        public static readonly Pronoun GenderNeutralPronouns = new Pronoun("they", "them", "their", "themself");
        public static readonly Pronoun ItemPronouns = new Pronoun("it", "that", "that's", "itself");

        public static readonly string[] FemaleGivenNames = new[]
        {
            "Alice", "Angela", "Ashley", "Angelika", "Amber", "Autumn", "Amelia", "Annabelle", "Anna", "Anne",
            "Anastasia", "Alexa", "Alexandria", "Audrey", "Aria", "Artemis", "Amy", "Abigail", "Abby", "Adrian",
            "Ariel", "Alicia", "Alma", "Ada", "August", "Alissa", "Ali",
            "Betty", "Beatrice", "Bernadette", "Blair", "Bonnie", "Bianca", "Blanca", "Bridget", "Brianna", "Bella", 
            "Brook", "Bethany", "Bri", "Brittany", "Bailey", "Barbara",
            "Christine", "Clara", "Clementine", "Charlotte", "Celeste", "Celia", "Cecilia", "Claire", "Chloe", 
            "Caroline", "Catherine", "Cynthia", "Cassandra", "Cass", "Colette", "Camille", "Carmen", "Celine", 
            "Cleo", "Catalina", "Callie", "Clarissa", "Cheyenne",
            "Diane", "Diana", "Daphne", "Daisy", "Delilah",
            "Evangelina", "Elise", "Eleanor", "Elle", "Elaine", "Elena", 
            "Francine", "Freya", "Florence", "Flora", "Faye", "Fiona",
            "Georgina", "Gabriella", "Gwen", "Gianna", "Gwendolyn", "Gwenyvere",
            "Holly", "Henrietta", "Hazel", "Hannah", "Helen", "Helena", "Heidi", "Haley",
            "Imogene", "Isadora", "Iris", "Ivy", "Irene", "Ilana",
            "Jen", "Jesse", "Joan", "Jennifer", "Justine", 
            "Kayla", "Katherine", "Katie", "Karen", "Keira", "Kaytlyn", "Kylee", 
            "Liz", "Lonnie", "Laura", "Lily", "Liliana", "Lynne",
            "Mary", "Margeret", "Maggie", "Maria", "Melody", "Melissa",
            "Nancy", "Nanette", "Nina", "Nora", "Nova", "Natalie", "Natasha", "Nat", "Nicole", "Noelle", "Naomi",
            "Ophelia", "Opal", "Olivia", "Octavia",
            "Pheobe", "Penelope", "Persephone", "Piper", "Paige", "Priscilla", "Pearl", "Penny", "Paula",
            "Regina", "Rita", "Rey", "Rose", "Ruby", "Rebecca",
            "Sarah", "Simone", "Stella", "Sage", "Sadie", "Scarlet", "Sylvia",
            "Teresa", "Tabbie", "Thea", "Tessa", "Tiffany",
            "Veronica", "Violet", 
            "Wanda", "Wren", "Winter", "Whitney",
        };
        public static readonly string[] MaleGivenNames = new[]
        {
            "Alvin", "Aaron", "Adam", 
            "Bob", "Bill", "Boris",
            "Carl", "Clark", "Chris", "Christopher",
            "David", "Duke", "Dean", "Dick",
            "Evan", "Earl", "Enrique", "Eunice",
            "Foster", "Frank", "Felix",
            "Greg", "Geoffrey", "Gary",
            "Houston", "Howard", "Hank", "Harry",
            "Ivan", "Isaac", "Ichabod",
            "Jack", "Jeremiah", "Jerome", "Jerry", "John", "Josh", 
            "Kyle", "Kevin", "Kenneth", "Kenny",
            "Larry", "Lyle", "Lenny", "Luke", "Lloyd",
            "Martin", "Max", "Michael", "Mike", "Matthew", "Mark", 
            "Nate", "Nicholas", "Nick", "Nathan", 
            "Oscar", "Oswald", 
            "Peter", "Paul", 
            "Quincy", "Quinton",
            "Ralph", "Roger", 
            "Steve", "Stephen", "Saul", "Simon", 
            "Tom", "Thomas", "Timothy", 
            "Victor",
            "Walter", "Wally", "Winston", 
            "Xavier",
        };
        public static readonly string[] GenderNeutralGivenNames = new[]
        {
            "Pat", "Sam", "Ash", "Billy", "Blake", "Brooklyn", "Charlie", "Cameron", "Parker", "Paisley", "Paris", "Francis", "Ray", "Riley", "Taylor"
        };
        public static readonly string[] FamilyNames = new[]
        {
            "Anderson", "Ableton", "Abrahams", "Andrews",
            "Barrister", "Bannon", "Bilkington",
            "Clarkson", "Cavy", "Cor de Laine",
            "Douglas", "Davidson", "Daniels",
            "Ephegan", "Earlingmire", "Eagle",
            "Fox", "Flowers", "Fogle",
            "Gregori", "Garrison", "Gadson",
            "Henderson", "Howards", "Hamish",
            "Ivigny", "Imos", 
            "Jenkins", "Jamison", "Johnson", "Jove",
            "Klimpt", "Kennedy", "Kane", 
            "Lindenberg", "Lawton", "Larrimie", "Lorelai",
            "Michaels", "MacDonald", "Moor", "McMaster", "Manchester",
            "Niles", "Nantucket", "Norris",
            "O'Flanagan", "O'Niell", "Orchard",
            "Petersen", "Planter", "Prince",
            "Royal", "Rogerstein", "Rankin", "Rammstein",
            "Smith", "Summers", "Shumaker", "Schrodinger",
            "Tannenbaum", "Trow", "Tuck", "Talon", 
            "Vovchek", "Vorhees",
            "Williams", "Wojak", "Winston", "Winchester", "Walton", "White",
            "Yang", "Yellow", 
        };
        #endregion

        #region specific substantives
        public const string ParkName = "Butterfly Park";
        public const string ParkDescription = "a popular place among the locals";
        public static readonly Noun ParkNouns = new Noun("park", "parks");
        public static Substantive ParkSubstantive => new Substantive(SubstantiveTypes.Place, ParkName,
            ParkDescription, ParkNouns, ItemPronouns, new PhysicalProperties(0, 0));

        public const string PondName = "Clark's Pond";
        public const string PondDescription = "a popular spot for locals to come and watch the waves go by";
        public static readonly Noun PondNouns = new Noun("pond", "ponds");
        public static Substantive PondSubstantive => new(SubstantiveTypes.Place, PondName, PondDescription, PondNouns,
            ItemPronouns, new PhysicalProperties(0, 0));

        public const string BlockDescription = "a quiet residential block";
        public static readonly Noun BlockNouns = new Noun("block", "blocks");

        public const string StreetDescription = "a busy street";
        public static readonly Noun StreetNouns = new ("street", "streets");

        public const string HouseDescription = "a house in a neighborhood";
        public static readonly Noun HouseNouns = new("house", "houses");

        public static readonly Noun RoomNouns = new("room", "rooms");
        public static Substantive RoomSubstantive => new(SubstantiveTypes.Place, "room", "a room in a building",
            RoomNouns, ItemPronouns, new PhysicalProperties(0, 0));

        public static readonly Noun HallNouns = new("hall", "halls");

        public static Substantive HallSubstantive => new(SubstantiveTypes.Place, "hall", "a hall in a building",
            HallNouns, ItemPronouns, new PhysicalProperties(0, 0));

        public const string DownTownDescription = "A lively place where people come to shop";
        public static readonly Noun DownTownNouns = new Noun("downtown area", "downtown areas");

        public static readonly Noun ShopNouns = new("shop", "shops");

        #endregion

        #region speech strings

        #endregion

    }
}