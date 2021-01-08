using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;

namespace ExampleGame.Scenes.GenerationSteps
{
    public class BlockGenerationStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            var map = context.GetFirstOrNew<ISettableGridView<RogueLikeCell>>
                (() => new ArrayView<RogueLikeCell>(context.Width, context.Height), "block");

            var roads = new List<Region>();
            var random = new Random();
            double rotation = random.NextDouble() * 20;
            int horizontalNameIndex = random.Next(Enum.GetNames(typeof(RoadNames)).Length - 1);
            int verticalNameIndex = random.Next(Enum.GetNames(typeof(RoadNumbers)).Length - 1);

            foreach (var point in map.Positions())
            {
                map[point] = new RogueLikeCell(point, Color.Green, Color.Black, '"', 0);
            }

            yield return null;
            
            //generate roads
            Point nw = (-3, -3);
            Point sw = (-3, 3);
            Point ne = (map.Width + 3, -3);
            Point se = (map.Width + 3, 3);

            roads.Add(new Region(((RoadNames)horizontalNameIndex).ToString(), nw, ne, se, sw).Rotate(rotation));
            
            nw = (-3, map.Height - 3);
            sw = (-3, map.Height + 3);
            ne = (map.Width + 3, map.Height - 3);
            se = (map.Width + 3, map.Height + 3);

            roads.Add(new Region(((RoadNames)horizontalNameIndex + 1).ToString(), nw, ne, se, sw).Rotate(rotation));
            
            nw = (-3, -3);
            sw = (-3, map.Height + 3);
            ne = (3, -3);
            se = (3, map.Height + 3);

            roads.Add(new Region(((RoadNumbers)horizontalNameIndex).ToString(), nw, ne, se, sw).Rotate(rotation));
            
            nw = (map.Width - 3, -3);
            sw = (map.Width - 3, map.Height + 3);
            ne = (map.Width + 3, -3);
            se = (map.Width + 3, map.Height + 3);

            roads.Add(new Region(((RoadNumbers)verticalNameIndex + 1).ToString(), nw, ne, se, sw).Rotate(rotation));

            foreach (var road in roads)
            {
                foreach (var point in road.Points.Where(p => map.Contains(p)))
                {
                    map[point] = new RogueLikeCell(point, Color.DarkGray, Color.Black, '.', 0);
                }

                yield return null;
            }

            Point center = (map.Width / 2, map.Height / 2);
            var floorSpace = new Rectangle(8, 8, map.Width - 15, map.Height - 15);
            foreach (var room in floorSpace.BisectRecursive(8))
            {
                var region = Region.FromRectangle("room", room).Rotate(rotation, center);
                foreach (var point in region.InnerPoints.Positions.Where(p=> map.Contains(p)))
                {
                    bool alt = (point.X + point.Y) % 2 == 0;
                    int glyph = alt ? 9 : 10;
                    Color fore = Color.Yellow;
                    Color back = Color.DarkGoldenrod;
                    map[point] = new RogueLikeCell(point, fore, back, glyph, 0);
                }

                
                foreach (var point in region.OuterPoints.Positions.Where(p=> map.Contains(p)))
                {
                    map[point] = new RogueLikeCell(point, Color.Red, Color.White, 178, 0, false, false);
                }

                yield return null;
            }
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