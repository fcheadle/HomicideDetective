using System;
using System.Collections.Generic;
using HomicideDetective.Words;

namespace HomicideDetective.Things
{
    public class ThingFactory
    {
        public static Substantive MiscellaneousItem(int seed)
        {
            var random = new Random(seed);
            string name, description, detail;
            Noun nouns;
            Pronoun pronouns = Constants.ItemPronouns;
            PhysicalProperties properties;
            Verb? verbs;
            Verb.Tense past;
            Verb.Tense present;
            Verb.Tense future;
            
            switch (random.Next(1, 10) % 2)
            {
                default:
                    name = "coffee mug";
                    nouns = new Noun("coffee mug", "coffee mugs");
                    past = new Verb.Tense("sipped from");
                    present = new Verb.Tense("sip from", "sip from", "sip from", "sip from", "sips from", "sip from");
                    future = new Verb.Tense("will sip from");
                    description = "a small ceramic cup used for hot liquids";
                    detail = "it has brown stains on the interior.";
                    verbs = new Verb("to sip from", past, present, future);
                    properties = new PhysicalProperties(101, 64);
                    break;
                case 1:
                    name = "typewriter";
                    nouns = new Noun("typewriter", "typewriters");
                    past = new Verb.Tense("typed on");
                    present = new Verb.Tense("am typing", "are typing", "are typing", "are typing", "is typing",
                        "are typing");
                    future = new Verb.Tense("will type");
                    description = "device used to write quickly and legibly";
                    detail = "it is worn from years of use.";
                    verbs = new Verb("to type", past, present, future);
                    properties = new PhysicalProperties(64000, 320);
                    break;
            }   
            var substantive = new Substantive(SubstantiveTypes.Thing, name, description, nouns, pronouns, properties, verbs);
            substantive.AddDetail(detail);
            return substantive;
        }

        public static Substantive Weapon(int seed)
        {
            var random = new Random(seed);
            string name, description, detail, article;
            PhysicalProperties properties;
            Noun nouns;
            Verb.Tense pastTense;
            Verb.Tense presentTense;
            Verb.Tense futureTense;
            Verb? usageVerbs;
            Pronoun pronouns = Constants.ItemPronouns;
            
            switch (random.Next(0,4))
            {
                default: 
                    name = "hammer";
                    article = "a";
                    description = "A small tool, normally used for carpentry";
                    properties = new PhysicalProperties(710, 490);
                    nouns = new Noun("hammer", "hammers");
                    pastTense = new Verb.Tense("swung");
                    presentTense = new Verb.Tense("am swinging", "are swinging", 
                        "is swinging", "are swinging", "is swinging", "are swinging");
                    futureTense = new Verb.Tense("will swing");
                    usageVerbs = new Verb("to swing", pastTense, presentTense, futureTense);
                    detail = "It is completely free of dust, and has a slight smell of bleach";
                    break;
                case 1: 
                    name = "switchblade"; 
                    description = "A small, concealable knife";
                    article = "a";
                    properties = new PhysicalProperties(125, 325);
                    nouns = new Noun("switchblade", "switchblades");
                    pastTense = new Verb.Tense("stabbed");
                    presentTense = new Verb.Tense("stab", "stab", 
                        "stab", "stab", "stabs", "stab");
                    futureTense = new Verb.Tense("will stab");
                    usageVerbs = new Verb("to stab", pastTense, presentTense, futureTense);
                    detail = "There is a small patch of red rust near the hinge";
                    break;
                case 2: 
                    name = "pistol"; 
                    description = "A small, concealable handgun";
                    article = "a";
                    properties = new PhysicalProperties(6750, 465);
                    nouns = new Noun("pistol", "pistols");
                    pastTense = new Verb.Tense("shot");
                    presentTense = new Verb.Tense("shoot");
                    futureTense = new Verb.Tense("will shoot");
                    usageVerbs = new Verb("to shoot", pastTense, presentTense, futureTense);
                    detail = "There are residue patterns on the muzzle";
                    break;
                case 3: 
                    name = "poison"; 
                    description = "A lethal dose of hydrogen-cyanide";
                    article = "a";
                    properties = new PhysicalProperties(15, 12);
                    nouns = new Noun("vial of poison", "vials of poison");
                    pastTense = new Verb.Tense("poisoned");
                    presentTense = new Verb.Tense("apply poison to", "apply poison to", "apply poison to",
                        "apply poison to", "applies poison to", "apply poison to");
                    futureTense = new Verb.Tense("will poison");
                    usageVerbs = new Verb("to use poison", pastTense, presentTense, futureTense);
                    detail = "There is a puncture hole in the cap";
                    break;
            }

            var substantive = new Substantive(SubstantiveTypes.Thing, name, description, nouns, pronouns, properties, usageVerbs, article);
            substantive.AddDetail(detail);
            return substantive;
        }
    }
}