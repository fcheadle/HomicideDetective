using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using SadRogue.Integration;

namespace HomicideDetective.Places.Generation
{
    public class StreetStep : GenerationStep
    {
        private bool _streetInMiddle;

        public StreetStep(bool streetInMiddle = false)
        {
            _streetInMiddle = streetInMiddle;
        }

        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<RogueLikeCell>>
                (() => new ArrayView<RogueLikeCell>(context.Width, context.Height), "street");
            var roads = context.GetFirstOrNew(() => new List<Region>(), "regions");
            var random = new Random();
            int horizontalNameIndex = random.Next(Enum.GetNames(typeof(RoadNames)).Length - 2);
            int verticalNameIndex = random.Next(Enum.GetNames(typeof(RoadNumbers)).Length - 2);
            
            if(_streetInMiddle)
                roads.AddRange(CreateStreet(map.Width, map.Height, horizontalNameIndex, verticalNameIndex));
            else
                roads.AddRange(CreateBlock(map.Width, map.Height, horizontalNameIndex, verticalNameIndex));
            
            foreach (var road in roads)
            {
                foreach (var point in road.Points.Where(p => map.Contains(p)))
                {
                    map[point] = new RogueLikeCell(point, Color.DarkGray, Color.Black, '.', 0);
                }

                yield return null;
            }
            
            roads.Add(new Region($"{horizontalNameIndex}00 block {verticalNameIndex} street", 
                (0, 0), (map.Width, 0), (map.Width, map.Height), (0, map.Height)));
        }

        private IEnumerable<Region> CreateStreet(int width, int height, int horizontalNameIndex, int verticalNameIndex)
        {
            var equator = height / 2;
            Point nw = (-3, equator - 3);
            Point sw = (-3, equator + 3);
            Point ne = (width + 3, equator - 3);
            Point se = (width + 3, equator + 3);
            yield return new Region($"{Enum.GetNames(typeof(RoadNames))[horizontalNameIndex]} street", nw, ne, se, sw);
            
            nw = (13, -10);
            sw = (13, height + 10);
            ne = (19, -10);
            se = (19, height + 10);

            yield return new Region($"{Enum.GetNames(typeof(RoadNumbers))[verticalNameIndex]} street", nw, ne, se, sw).Rotate(45);
            
            nw = (width - 10, -10);
            sw = (width - 10, height + 10);
            ne = (width - 4 , -10);
            se = (width - 4 , height + 10);

            yield return new Region($"{Enum.GetNames(typeof(RoadNumbers))[verticalNameIndex + 1]} street", nw, ne, se, sw).Rotate(45);
        }

        private IEnumerable<Region> CreateBlock(int width, int height, int horizontalNameIndex, int verticalNameIndex)
        {
            //generate roads
            Point nw = (-3, -3);
            Point sw = (-3, 3);
            Point ne = (width + 3, -3);
            Point se = (width + 3, 3);
            yield return new Region($"{Enum.GetNames(typeof(RoadNames))[horizontalNameIndex]} street", nw, ne, se, sw);
            
            nw = (-3, height - 3);
            sw = (-3, height + 3);
            ne = (width + 3, height - 3);
            se = (width + 3, height + 3);

            yield return new Region($"{Enum.GetNames(typeof(RoadNames))[horizontalNameIndex + 1]} street", nw, ne, se, sw);
            
            nw = (13, -10);
            sw = (13, height + 10);
            ne = (19, -10);
            se = (19, height + 10);

            yield return new Region($"{Enum.GetNames(typeof(RoadNumbers))[verticalNameIndex]} street", nw, ne, se, sw).Rotate(45);
            
            nw = (width - 10, -10);
            sw = (width - 10, height + 10);
            ne = (width - 4 , -10);
            se = (width - 4 , height + 10);

            yield return new Region($"{Enum.GetNames(typeof(RoadNumbers))[verticalNameIndex + 1]} street", nw, ne, se, sw).Rotate(45);
        }
    }
    public enum RoadNames
    {
        Alder,
        Baskins,
        Cottonwood,
        Danforth,
        England,
        Franklin,
        Guittierez,
        Holly,
        Imbrogno,
        Juniper,
        Klatski,
        Lark,
        Martin,
        Norwood,
        Oak,
        Pear,
        Quenton,
        Raspberry,
        Smith,
        Thompson,
        Underbridge,
        Vanguard,
        Wapanaugh,
        Xavier,
        Yew,
        Zedd,

        Alm,
        BellFlower,
        Cypress,
        DuskView,
        Elders,
        Fey,
        Gabriel,
        Harris,
        Idlewine,
        Jackal,
        Kaczkowski,
        LaPierre,
        MatrinLuthorKingJr,
        Nathaniel,
        Oscar,
        Princeton,
        Quincy,
        Rodriguez,
        Spruce,
        Terweiliger,
        Utah,
        VillaNova,
        Waterfront,
        Xylophone,
        YellowLine,
        Zephraim,

        Apple,
        Beligne,
        Chestnut,
        Delaney,
        Eagle,
        Fadler,
        Gomez,
        Hendersen,
        Inhoff,
        Jet,
        Klein,
        Loop,
        Mayor,
        Nil,
        Opal,
        Pickers,
        Rowdy,
        Sunset,
        TurnMeadow,
        Umberday,
        VonStukk,
        Yslich,

        Ash,
        Birch,
        Cedar,
        Dogwood,
        Elm,
        Fir,
        Guthrow,
        Hanson,
        Idriss,
        Jamal,
        Kodak,
        Lopez,
        McMonohan,
        Nigel,
        Oberon,
        Pearl,
        Quail,
        RodStirling,
        Spear,
        Thurston,
        Utica,
        VanNuys,
        Westerly,
        XTransfer,
        Yarborough,
        Zale,

        Aspen,
        Burk,
        Chambers,
        Denmark,
        Elton,
        Fredricks,
        Greensburg,
        Hamburg,
        Ibex,
        Justice,
        Kasimir,
        Loxley,
        Montoya,
        Neumann,
        Olive,
        Peach,
        Quebec,
        Redman,
        Shakespear,
        Taupe,
        Ursula,
        Vanilla,
        Williams,
        Xanadu,
        Yates,
        ZooFront,

        Anaheim,
        Boston,
        Cherry,
        Dunn,
        Edinburough,
        Fabian,
        Gannon,
        Hogan,
        Ibanez,
        Jaillet,
        Katz,
        Lindbergh,
        Menendez,
        Neheigh,
        Olivia,
        Parsley,
        Quaker,
        Rojo,
        Sanders,
        Topaz,
        Urion,
        Vassal,
        Wadworth,
        Zilch,

        AntiqueRow,
        Beaufort,
        Celeste,
        Debutante,
        Esperoza,
        Farnsworth,
        Gorilla,
        Hinkley,
        Ipanema,
        Jasper,
        Kuiper,
        Larimer,
        Miracle,
        Noddingham,
        Olivine,
        Pyrite,
        Quartz,
        Rochester,
        Samuels,
        Tourmaline,
        Ultraline,
        VeraCruz,
        Watley,
        Yuri,
        Zacksen
    }
    public enum RoadNumbers
    {
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        Sixth,
        Seventh,
        Eighth,
        Ninth,
        Tenth,
        Eleventh,
        Twelfth,
        Thirteenth,
        Fourteenth,
        Fifteenth,
        Sixteenth,
        Seventeenth,
        Eighteenth,
        Nineteenth,
        Twentieth,
        TwentyFirst,
        TwentySecond,
        TwentyThird,
        TwentyFourth,
        TwentyFifth,
        TwentySixth,
        TwentySeventh,
        TwentyEighth,
        TwentyNinth,
        Thirtieth,
        ThirtyFirst,
        ThirtySecond,
        ThirtyThird,
        ThirtyFourth,
        ThirtyFifth,
        ThirtySixth,
        ThirtySeventh,
        ThirtyEighth,
        ThirtyNinth,
        Fortieth,
        FortyFirst,
        FortySecond,
        FortyThird,
        FortyFourth,
        FortyFifth,
        FortySixth,
        FortySeventh,
        FortyEighth,
        FortyNinth,
        Fiftieth,
        FiftyFirst,
        FiftySecond,
        FiftyThird,
        FiftyFourth,
        FiftyFifth,
        FiftySixth,
        FiftySeventh,
        FiftyEighth,
        FiftyNinth,
        Sixtieth,
        SixtyFirst,
        SixtySecond,
        SixtyThird,
        SixtyFourth,
        SixtyFifth,
        SixtySixth,
        SixtySeventh,
        SixtyEighth,
        SixtyNinth,
        Seventieth,
        SeventyFirst,
        SeventySecond,
        SeventyThird,
        SeventyFourth,
        SeventyFifth,
        SeventySixth,
        SeventySeventh,
        SeventyEighth,
        SeventyNinth,
        Eightieth,
        EightyFirst,
        EightySecond,
        EightyThird,
        EightyFourth,
        EightyFifth,
        EightySixth,
        EightySeventh,
        EightyEighth,
        EightyNinth,
        Ninetieth,
        NinetyFirst,
        NinetySecond,
        NinetyThird,
        NinetyFourth,
        NinetyFifth,
        NinetySixth,
        NinetySeventh,
        NinetyEighth,
        NinetyNinth,
        OneHundred,
        OneHundredFirst,
    }
}