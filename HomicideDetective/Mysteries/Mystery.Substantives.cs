using System.Collections.Generic;
using GoRogue;

namespace HomicideDetective.Mysteries
{
    public partial class Mystery
    {
        /// <summary>
        /// Generates the descriptive information about the scene of the crime.
        /// </summary>
        /// <returns></returns>
        public Substantive GenerateSceneOfMurderInfo()
        {
            var name = $"Brick Home";
            string noun = "location";
            string pronoun = "it";
            string pronounPossessive = "its";
            string pronounPassive = "it";

            
            string height = "long and low";
            string width = "slim";

            string description = $"{pronoun} is a {height}, {width}{noun}.";

            var substantive = new Substantive(Substantive.Types.Place, name, gender: "", article: "", pronoun: pronoun,
                pronounPossessive: pronounPossessive, description: description, mass: 0, volume: 0);

            return substantive;
        }

        /// <summary>
        /// Generate the descriptive info for any random person.
        /// </summary>
        /// <param name="surname">The Surname of the individual to generate.</param>
        /// <returns></returns>
        public Substantive GeneratePersonalInfo(string surname)
        {
            bool isMale = Random.Next(0, 2) == 0;
            bool isTall = Random.Next(0, 2) == 0;
            bool isFat = Random.Next(0, 2) == 0;
            bool isYoung = Random.Next(0, 3) <= 1; 

            string noun = isMale ? "man" : "woman";
            string pronoun = isMale ? "he" : "she";
            string pronounPossessive = isMale ? "his" : "her";
            string article = "";

            string height = isTall ? "tall" : "short";
            string heightDescription = isTall ? "slightly taller than average" : $"rather short for someone {pronounPossessive} age";
            string width = isFat ? "full-figured" : "slender";
            string widthDescription = isFat ? "which is slightly over-weight" : "rather slender compared to other adults";
            string age = isYoung ? "young" : "middle-aged";

            string description = $"{pronoun} is a {height}, {width} {age} {noun}";
            string givenName = isMale ? _maleGivenNames[ Random.Next(0, _maleGivenNames.Length)] : _femaleGivenNames[ Random.Next(0, _femaleGivenNames.Length)];

            var substantive = new Substantive(Substantive.Types.Person, $"{givenName} {surname}",
                gender: isMale ? "male" : "female", article, pronoun, pronounPossessive, description,
                37500, 24000, heightDescription, widthDescription);  
            
            return substantive;
        }

        /// <summary>
        /// Generate the descriptive info for the murder weapon.
        /// </summary>
        /// <returns></returns>
        public Substantive GenerateMurderWeaponInfo()
        {
            string name;
            string description;
            string detail;
            string size;
            string weight;
            int mass;
            int volume;
            switch (Random.Next(0,10))
            {
                default:
                case 0: 
                    name = "Hammer";
                    description = "A small tool, normally used for carpentry";
                    mass = 710;
                    volume = 490;
                    detail = "It is completely free of dust, and has a slight smell of bleach";
                    size = "is exactly average in size";
                    weight = "weighs a little less than it looks like";
                    break;
                case 1: 
                    name = "Switchblade"; 
                    description = "A small, concealable knife";
                    mass = 125;
                    volume = 325;
                    detail = "There is a small patch of red rust near the hinge";
                    size = "is tiny";
                    weight = "weighs almost nothing";
                    break;
                case 2: 
                    name = "Pistol"; 
                    description = "A small, concealable handgun";
                    mass = 6750;
                    volume = 465;
                    detail = "There are residue patterns on the muzzle";
                    size = "is easily concealable";
                    weight = "weighs more than it looks like";
                    break;
                case 3: 
                    name = "Poison"; 
                    description = "A lethal dose of hydrogen-cyanide";
                    mass = 15;
                    volume = 12;
                    detail = "There is a puncture hole in the cap";
                    size = "is fits within an insulin bottle";
                    weight = "weighs little more than water";
                    break;
                case 4: 
                    name = "Kitchen Knife"; 
                    description = "A small tool used for preparing food";
                    mass = 240;
                    volume = 390;
                    detail = "It is somewhat warped along its flat plane";
                    size = "is long and curved";
                    weight = "weighs very little";
                    break;
                case 5: 
                    name = "Shotgun";
                    description = "A large gun used for scaring off vermin";
                    mass = 13500;
                    volume = 8825;
                    detail = "there is one shell in the chamber";
                    size = "is exactly average in size";
                    weight = "is heavy and has powerful kickback";
                    break;
                case 6: 
                    name = "Rock"; 
                    description = "A stone from off the ground";
                    mass = 10000;
                    volume = 750;
                    detail = "It is covered in blood and haphazardly discarded nearby";
                    size = "is three times the size of your fist";
                    weight = "is very dense and hard";
                    break;
                case 7: 
                    name = "Screwdriver"; 
                    description = "A small tool, used for all manner of handiwork";
                    mass = 120;
                    volume = 200;
                    detail = "This flathead has been recently cleaned";
                    size = "is four and a half inches long";
                    weight = "has a clear plastic handle";
                    break;
                case 8: 
                    name = "Revolver"; 
                    description = "A handgun";
                    mass = 7000;
                    volume = 700;
                    detail = "it has five bullets, and one empty chamber";
                    size = "has finely-etched filligree";
                    weight = "looks like it costs a lot of money";
                    break;
                case 9: 
                    name = "Rifle"; 
                    description = "A large gun, used for hunting animals";
                    mass = 12000;
                    volume = 8200;
                    detail = "It is coated with gunpowder residue.";
                    size = "has scuff marks on the butt";
                    weight = "looks like it has taken a beating";
                    break;
            }

            var substantive = new Substantive(Substantive.Types.Thing, name, 
                description: description, mass: mass, volume: volume, sizeDescription: size, weightDescription: weight);
            substantive.AddDetail(detail);
            return substantive;
        }
    }
}