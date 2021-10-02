using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Words;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using SadRogue.Integration.FieldOfView.Memory;

namespace HomicideDetective.Places.Generation
{
    public class StreetStep : GenerationStep
    {
        private bool _streetInMiddle;
        private int _horizontalStreetIndex;
        private int _verticalStreetIndex;

        public StreetStep()
        {
            _streetInMiddle = false;
            _horizontalStreetIndex = 0;
            _verticalStreetIndex = 0;
        }
        public StreetStep(bool streetInMiddle, int horizontalRoadIndex, int verticalRoadIndex)
        {
            _streetInMiddle = streetInMiddle;
            _horizontalStreetIndex = horizontalRoadIndex;
            _verticalStreetIndex = verticalRoadIndex;
        }

        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<MemoryAwareRogueLikeCell>>
                (() => new ArrayView<MemoryAwareRogueLikeCell>(context.Width, context.Height), Constants.GridViewTag);
            var blockArea = PolygonArea.Rectangle(new Rectangle((0, 0), (context.Width, context.Height)));
            var blockName = $"{_horizontalStreetIndex}00 block {(RoadNames)_verticalStreetIndex} street";
            var roads = context.GetFirstOrNew(() => new Place(blockArea, blockName, Constants.BlockDescription, Constants.BlockNouns, Constants.ItemPronouns, new PhysicalProperties(0,0)), Constants.RegionCollectionTag);
            var random = new Random();
            
            if(_streetInMiddle)
                foreach(var region in CreateStreet(map.Width, map.Height, _horizontalStreetIndex, _verticalStreetIndex))
                    roads.AddSubRegion(region);
            else
                foreach(var road in CreateBlock(map.Width, map.Height, _horizontalStreetIndex, _verticalStreetIndex)) 
                    roads.AddSubRegion(road);
            
            foreach(var road in roads.SubAreas)
                foreach (var point in road.Area.Where(p => map.Contains(p)))
                    map[point] = new MemoryAwareRogueLikeCell(point, Color.DarkGray, Color.Black, '.', 0);


            yield return null;
        }

        //creates a map where the street is in the center
        private IEnumerable<Place> CreateStreet(int width, int height, int horizontalNameIndex, int verticalNameIndex)
        {
            var equator = height / 2;
            Point nw = (-3, equator - 3);
            Point sw = (-3, equator + 3);
            Point ne = (width + 3, equator - 3);
            Point se = (width + 3, equator + 3);
            var polygon = new PolygonArea(nw, ne, se, sw);
            yield return new Place(polygon, $"{(RoadNames)horizontalNameIndex} street", Constants.StreetDescription,
                Constants.StreetNouns, Constants.ItemPronouns, new PhysicalProperties(0, 0));

            var sqDiff = (int)(height / Math.Sqrt(2));
            nw = (13, -sqDiff);
            sw = (13, height + sqDiff);
            ne = (19, -sqDiff);
            se = (19, height + sqDiff);
            polygon = new PolygonArea(nw, ne, se, sw).Rotate(45);
            yield return new Place(polygon, $"{(RoadNumbers)verticalNameIndex} street", Constants.StreetDescription,
                Constants.StreetNouns, Constants.ItemPronouns, new PhysicalProperties(0, 0));
            
            nw = (width - 10, -sqDiff);
            sw = (width - 10, height + sqDiff);
            ne = (width - 4 , -sqDiff);
            se = (width - 4 , height + sqDiff);
            polygon = new PolygonArea(nw, ne, se, sw).Rotate(45);
            yield return new Place(polygon, $"{(RoadNumbers)(++verticalNameIndex)} street", Constants.StreetDescription,
                Constants.StreetNouns, Constants.ItemPronouns, new PhysicalProperties(0, 0));
        }

        private IEnumerable<Place> CreateBlock(int width, int height, int horizontalNameIndex, int verticalNameIndex)
        {
            //generate roads
            Point nw = (-3, -3);
            Point sw = (-3, 3);
            Point ne = (width + 3, -3);
            Point se = (width + 3, 3);
            var polygon = new PolygonArea(nw, ne, se, sw);
            yield return new Place(polygon, $"{(RoadNames)horizontalNameIndex} street", Constants.StreetDescription,
                Constants.StreetNouns, Constants.ItemPronouns, new PhysicalProperties(0, 0));
            
            
            nw = (-3, height - 3);
            sw = (-3, height + 3);
            ne = (width + 3, height - 3);
            se = (width + 3, height + 3);
            polygon = new PolygonArea(nw, ne, se, sw);
            yield return new Place(polygon, $"{(RoadNames)(++horizontalNameIndex)} street", Constants.StreetDescription,
                Constants.StreetNouns, Constants.ItemPronouns, new PhysicalProperties(0, 0));
            
            var sqDiff = (int)(height / Math.Sqrt(2));
            nw = (13, -sqDiff);
            sw = (13, height + sqDiff);
            ne = (19, -sqDiff);
            se = (19, height + sqDiff);
            polygon = new PolygonArea(nw, ne, se, sw).Rotate(45);
            yield return new Place(polygon, $"{(RoadNumbers)verticalNameIndex} street", Constants.StreetDescription,
                Constants.StreetNouns, Constants.ItemPronouns, new PhysicalProperties(0, 0));
            
            nw = (width - 10, -sqDiff);
            sw = (width - 10, height + sqDiff);
            ne = (width - 4 , -sqDiff);
            se = (width - 4 , height + sqDiff);
            polygon = new PolygonArea(nw, ne, se, sw).Rotate(45);
            yield return new Place(polygon, $"{(RoadNumbers)(++verticalNameIndex)} street", Constants.StreetDescription,
                Constants.StreetNouns, Constants.ItemPronouns, new PhysicalProperties(0, 0));
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