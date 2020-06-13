using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Utilities
{
    /// <summary>
    /// A static class containing useful mathematical formulae.
    /// </summary>
    /// <remarks>
    /// All 'functions' returned from this class should be bound by -2 and 2. This means that no value ever returned should exceed 2 or be less than -2.
    /// There is no error checking around this, save by hand... for now
    /// </remarks>
    public static class Formulae
    {

        #region 2d functions
        public static float HeartBeat(float period) => 2f * (float)Math.Sin(Math.Sin(10 / period));
        public static double BoundedTan(double radians) => Math.Tan(radians % (Math.PI/4));
        #endregion

        #region Terrain
        public static List<Func<int, int, double>> TerrainGenerationFormulae = new List<Func<int, int, double>>()
        {
            //Terrain Generation formulae - starts at Color.Green and mutatesToZ of the returned value.
            //Currently, the color formula is counting down by 1, but we would like for it to count down by 0.01,
            //and then it should be bound between -2 and 2. Currently is not.
            //(x,y) => 0.00, //for testing purposes
            (x,y) => 07.00 * (Math.Sin(x + (x * 2.25)) + Math.Cos(y + (y/7))), //fast, personal fave
            (x,y) => 07.00 * Math.Cos(Math.Sqrt((x *x) + (y * y)) * 2), //fast, small concentric circles
            (x,y) => 04.65 * (Math.Sin(x/2) - (2*Math.Cos(4 * y / 14 + x / 4))), //moderate speed, odd vertical lines
            (x,y) => 04.65 * (Math.Sin(x/15 + y/15) - (2*Math.Cos(4 * y / 5))), //moderate speed, odd horizontal blocks
            (x,y) => 07.00 * Math.Cos(Math.Sqrt((x *x) + (y * y)) / 10), //moderate speed, large concentric circles
            (x,y) => 07.00 * Math.Sin(Math.Sqrt(x * x * Math.Cos(x) + y * y) / 16), //moderate speed, vertical grooves / horizontal sawtooth
            (x,y) => 07.00 * Math.Sin(Math.Sqrt(y * y * Math.Cos(y) + x * x) / 16), //moderate speed, horizontal grooves / vertical saw-tooth
            (x,y) => 07.00 * Math.Sin(x * Math.Sin(y) / 5) + Math.Sin(y * Math.Sin(x) / 5), //moderate speed, natural-ish, symetrical with respect to x,y
            (x,y) => 07.00 * Math.Sin(x * Math.Sin(y)) + Math.Sin(y * Math.Sin(x) / 8), //moderate speed, natural-ish
            (x,y) => 07.00 * Math.Sin(x * Math.Sin(y)/4) + Math.Sin(y * Math.Sin(x) * Math.PI), //moderate speed, very natural seeming
            (x,y) => 09.50 * Math.Sin(x * Math.Sin(y) * Math.PI) + Math.Sin(y * Math.Sin(x) / 8), //moderate speed, very natural seeming
            (x,y) => 12.00 * (Math.Sin(x / 25) / (2+Math.Cos(y / 8))), //moderate, peculiar gradients
            (x,y) => 12.00 * (Math.Sin((y + x % 32) / 18) / (2+Math.Cos(x + y % 13))), //moderate, unnatural rhombuses
            (x,y) => 12.00 * (Math.Sin(y / 4 + (x / 12)) / (2+Math.Cos(x / 16))), //moderate, curious waves
            (x,y) => 12.00 * Math.Cos((x + y / 15) / 8) / (2.5 + Math.Sin(y + x *2)), //moderate, knotted horizontal lines
            (x,y) => 19.19 * (Math.Sin((x + y) *0.66) + Math.Cos((y + x) * 1.33)), //moderate, Diagonal Ridges
            //(x,y) => 08.88 * (Math.Tan(x / 13 % 1.99) + Math.Tan(y % 1.91)), //slow, horizontal lines
            //(x,y) => 13.31 * (Math.Tan((-2 * x * y) % 1.99)), //slow, nice concentric circles
            //(x,y) => 19.19 * (Math.Cos(x * 0.33 + y * 0.25) - Math.Sin(y * 0.5 + x * 2.5)), //slowish, natural-looking
        };
        public static Func<int, int, double> RandomTerrainGenFormula()
        {
            return TerrainGenerationFormulae.RandomItem();
        }

        #endregion

        #region wind
        public static List<Func<int, int, TimeSpan, double>> EastBoundWindFormulae = new List<Func<int, int, TimeSpan, double>>()
        {
            //these are cleared - safe and pretty
            (x,y,t) => -Math.Cos(x * 3.45 - t.TotalMilliseconds / 777) - Math.Sin(y*0.77 - t.TotalMilliseconds / 77), //east waves - beautiful
            (x,y,t) => Math.Sin(x - t.TotalMilliseconds / 650) + Math.Cos(y - t.TotalMilliseconds / 325), //bubbles going south-southeast - beautiful
            (x,y,t) => 2 * Math.Cos(-t.TotalSeconds + Math.Sqrt((x+180)*(x+180)/444 + (y+90)*(y+90)/444)), // waves going southeast - beautiful
            (x,y,t) => 2 * Math.Cos(-t.TotalSeconds + Math.Sqrt(x*x/66 + (y+13)*(y+80)/222)), // waves going southeast - beautiful
            (x,y,t) => 2 * Math.Cos(-t.TotalSeconds + Math.Sqrt((x+222)*(x+222)/222 + (y+15)*(y+15)/66)), // waves going southeast - beautiful
            (x,y,t) => 2 * Math.Cos(t.TotalSeconds + Math.Sqrt((x-2222)*(x-2222)/222 + (y-1000)*(y-1000)/66)), // waves going southeast - beautiful
            (x,y,t) => 2 * Math.Cos(t.TotalSeconds + Math.Sqrt((x-1111)*(x-1111)/33 + (y-555)*(y-555)/99)), // waves going southeast - beautiful  
            (x,y,t) => 2 * Math.Cos(1.75*t.TotalSeconds + Math.Sqrt((x-2222)*(x-2222)/33 + (y-1000)*(y-1000)/99)), // waves going southeast - beautiful
            (x,y,t) => 2 * Math.Sin(x * (y / 8) + y * Math.Sin(y / (x+1)) + t.TotalSeconds),//bizarre and cool
        };
        public static List<Func<int, int, TimeSpan, double>> WestBoundWindFormulae = new List<Func<int, int, TimeSpan, double>>()
        {
            //cleared - safe and pretty
            (x,y,t) => 1.5 * Math.Sin(x / 3.33 + t.TotalSeconds) + Math.Cos(y * 3.33 + t.TotalSeconds), //bubbles northwest
            (x,y,t) => Math.Sin(x + t.TotalMilliseconds / 444) + Math.Cos(y + t.TotalMilliseconds / 333),//bubbles going northwest
            (x,y,t) => -Math.Cos(x - t.TotalSeconds) - Math.Sin(y + t.TotalSeconds), //NW bubbles - so so
            (x,y,t) => Math.Tan(y*.625 + x*1.75 - t.TotalSeconds / 100) % 2.01,//odd lines marching west, unbound
            (x,y,t) => 2* Math.Cos(t.TotalSeconds + Math.Sqrt(Math.Abs((x * y) + y * 180))), //badass west waves
            (x,y,t) => 2* Math.Cos(t.TotalSeconds / 2 + Math.Sqrt(Math.Abs((x * y) + y * 1400))), //badass west waves
            (x,y,t) => 2* Math.Cos(-t.TotalMilliseconds / 888 + Math.Sqrt(Math.Abs((x * y) - y * 1400))), //badass west waves

        };
        
        public static List<Func<int, int, TimeSpan, double>> NorthBoundWindFormulae = new List<Func<int, int, TimeSpan, double>>()
        {
            //(x,y,t) => Math.Cos(t.TotalMilliseconds / 666 + Math.Sqrt(Math.Abs((x * y) + y))) > 0.9, //waves going northwest
            //(x,y,t) => Math.Sin(x * 7.77 + t.TotalSeconds) + Math.Cos(y / 7.77 + t.TotalSeconds) > 1.1, //lines snaking north-northwest
            //(x,y,t) => Math.Sin(x / 3.33 + t.TotalMilliseconds / 666) + Math.Cos(y * 8.76 + t.TotalMilliseconds / 222), //fast north-northwest bubbles
            //(x,y,t) => Math.Cos(x * 1.125 - t.TotalMilliseconds / 111) + Math.Sin(y + t.TotalMilliseconds / 999) > 1.55, //wavy line crawling north
            //(x,y,t) => Math.Cos(y + t.TotalSeconds) + Math.Sin(x * y - t.TotalMilliseconds / 666), //marching breeze north
            //(x,y,t) => Math.Cos(x * y - t.TotalMilliseconds / 222) + Math.Sin(y * 4 - t.TotalMilliseconds / 222) > 1.55, //chaotic breeze northward with cross-breezes
            //(x,y,t) => (Math.Cos(x)* 3.75 + x + y * 4.15 + t.TotalSeconds * 25),//large, sloped sin waves marching north
            //(x,y,t) => Math.Sin(x - t.TotalMilliseconds / 111) + Math.Cos(y + t.TotalMilliseconds / 333) > 1.25, //lines zig-zagging north

        };
        public static List<Func<int, int, TimeSpan, double>> SouthBoundWindFormulae = new List<Func<int, int, TimeSpan, double>>()
        {
            //(x,y,t) => Math.Tan(y*.1 + x*1.75 - t.TotalSeconds) % 10,//south marching lines
            //(x,y,t) => Math.Sin(x + t.TotalMilliseconds / 111) + Math.Cos(y - t.TotalMilliseconds / 555) > 1.25, //waves zig-zagging south
            //(x,y,t) => Math.Sin(x - t.TotalMilliseconds / 650) - Math.Cos(y - t.TotalMilliseconds / 325), //bubbles snake south-southeast
            //(x,y,t) => Math.Cos(x * 3.45 - t.TotalMilliseconds / 500) - Math.Sin(y*0.77 - t.TotalMilliseconds / 250), //bubbles waving south
            //(x,y,t) => Math.Cos(x * y - t.TotalMilliseconds / 999) + Math.Sin(y - t.TotalMilliseconds / 111) > 1.55, //chaotic blowing south with cross-breezes 
            //(x,y,t) => (int)(y*4.4 + Math.Tan(x*0.666) - t.TotalMilliseconds / 188) % 69 == 1,//gentle south waves
            //(x,y,t) => Math.Cos(x * 0.88 + t.TotalMilliseconds / 333) + Math.Sin(y*1.125 - t.TotalMilliseconds / 777), //bubbles slowling going southwest

        };

        public static List<Func<int, int, TimeSpan, double>> ChaoticWindPatterns = new List<Func<int, int, TimeSpan, double>>()
        {            
            //(x,y,t) => 2 * Math.Cos(-t.TotalMilliseconds / 1111 + Math.Sqrt((x-70)*(x-70) / 111 + (y-90)*(y-90) / 111)), //concentric waves from (90,90)
            //(x,y,t) => Math.Cos(t.TotalMilliseconds / 222 + Math.Sqrt((x*x) + (y*y))) * 1.751, //concentric waves converging on northwest

            //(x,y,t) => 2 * Math.Cos(t.TotalMilliseconds / 777 + Math.Sqrt(Math.Abs(y * y * 0.75 - x * 3.5))), //odd waves
            //(x,y,t) => Math.Cos(t.TotalMilliseconds / 555 + Math.Sqrt(Math.Abs((x*x) + (y * 20)))), //spiraling towards northwest
            //(x,y,t) => Math.Cos(t.TotalMilliseconds / 777 + Math.Sqrt(y * x)) > 0.95, //waves going nothwest and curling as they go
            //(x,y,t) => (int)(Math.Cos(x) + x * 3.75 + y * 4.15 + t.TotalSeconds * t.TotalSeconds) % 64 <= 5,//gradually escalating northwest storm
            //(x,y,t) => Math.Cos(- t.TotalMilliseconds / 777 + Math.Sqrt(Math.Abs(y * y * 0.75 - x * 3.5))) > 0.9, //odd waves going west
            //(x,y,t) => Math.Cos(t.TotalMilliseconds / 777 - Math.Sqrt(y * y * x * x)) > 0.95, //omnidirectional chaos
            //(x,y,t) => Math.Sin(t.TotalMilliseconds / 777 + Math.Sqrt(y * y * x)) > 0.95, //omnidirectional waves with many nexi
            //(x,y,t) => Math.Sin(t.TotalMilliseconds / 777 - Math.Sqrt(y * x * x)) > 0.95, //omnidirectional waves with many nexi
            //(x,y,t) => Math.Sin(-t.TotalMilliseconds / 777 + Math.Sqrt(y * y * x)) > 0.95, //omnidirectional, spiraling chaos with many nexi
            //(x,y,t) => Math.Sin(-t.TotalMilliseconds / 777 - Math.Sqrt(y * x * x)) > 0.95, //omnidirectional wave form with many nexi
            //(x,y,t) => Math.Sin(Math.Sqrt(Math.Abs((y*y) - (x*x))) - t.TotalMilliseconds / 888) > 0.9, //beautiful lines come from an axis northwest-southeast and curl to the other corners
            //(x,y,t) => Math.Cos(-t.TotalMilliseconds / 999 + Math.Sqrt(Math.Abs((x*x) - (y*y)))) > 0.9, //beautiful lines come from northeast and southwest to the center
            //(x,y,t) => Math.Cos(x * y + t.TotalMilliseconds / 888) + Math.Sin(y * x - t.TotalMilliseconds / 555) > 1.75, //starts and stops, nondirectional 
            //(x,y,t) => Math.Cos(x * y + t.TotalMilliseconds / 666) - Math.Sin(-1.5 * y * x - t.TotalMilliseconds / 222) < -1.55, //omnidirectional chaos
            //(x,y,t) => Math.Cos(x * y + t.TotalMilliseconds / 666) + Math.Sin(y * x) < -1.55, //periodic, omnidirectional chaos
            //(x,y,t) => Math.Cos(y) + Math.Sin(x * y + t.TotalMilliseconds / 666) > 1.33, //east-west constant winds of varying intensities
            //(x,y,t) => (int)(Math.Tan(x*0.875) + Math.Tan(y*1.875) + t.TotalSeconds * 11) % 99 == 1,//periodic beautiful chaos
            //(x,y,t) => Math.Cos(Math.Sqrt(2 * t.TotalSeconds * t.TotalSeconds + x*y)) > 0.5,//counterclockwise spiral from a center of the southeast corner, speeds up over time
             
        };
        public static Func<int, int, TimeSpan, double> RandomWindPattern()
        {
            List<Func<int, int, TimeSpan, double>> Functions4d = EastBoundWindFormulae;
            Functions4d.AddRange(WestBoundWindFormulae);
            Functions4d.AddRange(SouthBoundWindFormulae);
            Functions4d.AddRange(NorthBoundWindFormulae);
            
            return Functions4d.RandomItem();
        }
        #endregion
    }
}
