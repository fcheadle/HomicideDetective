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
                37500, 24000);  
            
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
                    break;
                case 1: 
                    name = "Switchblade"; 
                    description = "A small, concealable knife";
                    mass = 125;
                    volume = 325;
                    detail = "There is a small patch of red rust near the hinge";
                    break;
                case 2: 
                    name = "Pistol"; 
                    description = "A small, concealable handgun";
                    mass = 6750;
                    volume = 465;
                    detail = "There are residue patterns on the muzzle";
                    break;
                case 3: 
                    name = "Poison"; 
                    description = "A lethal dose of hydrogen-cyanide";
                    mass = 15;
                    volume = 12;
                    detail = "There is a puncture hole in the cap";
                    break;
                case 4: 
                    name = "Kitchen Knife"; 
                    description = "A small tool used for preparing food";
                    mass = 240;
                    volume = 390;
                    detail = "It is somewhat warped along its flat plane";
                    break;
                case 5: 
                    name = "Shotgun";
                    description = "A large gun used for scaring off vermin";
                    mass = 13500;
                    volume = 8825;
                    detail = "there is one shell in the chamber";
                    break;
                case 6: 
                    name = "Rock"; 
                    description = "A stone from off the ground";
                    mass = 10000;
                    volume = 750;
                    detail = "It is covered in blood and haphazardly discarded nearby";
                    break;
                case 7: 
                    name = "Screwdriver"; 
                    description = "A small tool, used for all manner of handiwork";
                    mass = 120;
                    volume = 200;
                    detail = "This flathead has been recently cleaned";
                    break;
                case 8: 
                    name = "Revolver"; 
                    description = "A handgun";
                    mass = 7000;
                    volume = 700;
                    detail = "it has five bullets, and one empty chamber";
                    break;
                case 9: 
                    name = "Rifle"; 
                    description = "A large gun, used for hunting animals";
                    mass = 12000;
                    volume = 8200;
                    detail = "It is coated with gunpowder residue.";
                    break;
            }

            var substantive = new Substantive(Substantive.Types.Thing, name, 
                description: description, mass: mass, volume: volume);
            substantive.AddDetail(detail);
            return substantive;
        }

        /// <summary>
        /// Generates info for a random item.
        /// </summary>
        /// <returns>A substantive for a generic item that could exist anywhere.</returns>
        public Substantive GenerateMiscellaneousItemInfo()
        {
            string name, description, detail;
            int mass, volume;
            switch (Random.Next(1, 101) % 10)
            {
                default:
                case 0:
                    name = "coffee mug";
                    description = "a small ceramic cup used for hot liquids";
                    detail = "it has brown stains on the interior.";
                    mass = 101;
                    volume = 64;
                    break;
                case 1:
                    name = "typewriter";
                    description = "device used to write quickly and legibly";
                    detail = "it is worn from years of use.";
                    mass = 64000;
                    volume = 320;
                    break;
                case 2:
                    name = "photo album";
                    description = "a booklet for holding photographs";
                    detail = "there is a page in the middle with no photos.";
                    mass = 2050;
                    volume = 180;
                    break;
                case 3:
                    name = "handsaw";
                    description = "a small tool, normally used for carpentry";
                    detail = "it is rusted all over.";
                    mass = 881;
                    volume = 200;
                    break;
                case 4:
                    name = "record player";
                    description = "a device which plays music from records";
                    detail = "there is no record in it right now.";
                    mass = 1675;
                    volume = 850;
                    break;
                case 5:
                    name = "schoolbook";
                    description = "a book containing basic information about math";
                    detail = "the inside is covered in crayon drawings";
                    mass = 1750;
                    volume = 600;
                    break;
                case 6:
                    name = "toaster";
                    description = "a device used for toasting bread";
                    detail = "it is filled with crumbs.";
                    mass = 1200;
                    volume = 1200;
                    break;
                case 7:
                    name = "shirt";
                    description = "a shirt that has been tossed on the ground";
                    detail = "it is filthy beyond belief.";
                    mass = 101;
                    volume = 64;
                    break;
                case 8:
                    name = "vinyl record";
                    description = "a record that contains music and can be played by a record player";
                    detail = "it is a hard rock band with violent album art.";
                    mass = 0;
                    volume = 0;
                    break;
                case 9:
                    name = "hairbrush";
                    description = "a tool used for grooming one's hair";
                    detail = "it has red, brown, and grey hairs in its teeth.";
                    mass = 0;
                    volume = 0;
                    break;
            }   
            
            var substantive = new Substantive(Substantive.Types.Thing, name, 
                description: description, mass: mass, volume: volume);
            substantive.AddDetail(detail);
            return substantive;
        }
    }
}