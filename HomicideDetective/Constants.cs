namespace HomicideDetective
{
    public class Constants
    {
        #region generation strings
        public const string RegionCollectionTag = "regions";
        public const string GridViewTag = "map";
        public const string WindyPlainsTag = "plains";
        public const string BodyOfWaterTag = "pond";
        #endregion
        
        #region substantive strings
        public const string MaleGenderName = "male";
        public const string MalePronoun = "he";
        public const string MalePronounPossessive = "his";
        public const string MalePronounPassive = "him";
        public const string MaleAdultNoun = "man";
        public const string MaleChildNoun = "boy";

        public const string FemaleGenderName = "female";
        public const string FemalePronoun = "she";
        public const string FemalePronounPossessive = "her";
        public const string FemalePronounPassive = "her";
        public const string FemaleAdultNoun = "woman";
        public const string FemaleChildNoun = "girl";

        public const string NonBinaryGenderName = "intersex";
        public const string NonBinaryPronoun = "they";
        public const string NonBinaryPronounPossessive = "their";
        public const string NonBinaryPronounPassive = "them";
        public const string NonBinaryAdultNoun = "person";
        public const string NonBinaryChildNoun = "child";  
        
        public const string NonGenderedPronoun = "it";
        public const string NonGenderedPronounPossessive = "it's";
        public const string NonGenderedPronounPassive = "that";

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
            "Evangelina", "Elise", "Eleanor", "Elle", 
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
            "Regina", "Rita", "Renita", "Rey", "Rose", "Ruby", "Rebecca",
            "Sarah", "Simone", "Stella", "Sage", "Sadie", "Scarlet", "Sylvia",
            "Teresa", "Tabbie", "Thea", "Tessa", "Tiffany",
            "Veronica", "Violet", 
            "Wanda", "Wilhelmetta", "Wren", "Winter", "Whitney",
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
            "Kyle", "Kevin",
            "Larry", "Lyle", "Lenny", "Luke", 
            "Martin", "Max", "Michael", "Mike", "Matthew", "Mark", 
            "Nate", "Nicholas", "Nick", "Nathan", 
            "Oscar", "Oswald", 
            "Peter", "Paul", 
            "Quincy", 
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

        #region speech strings

        #endregion
    }
}