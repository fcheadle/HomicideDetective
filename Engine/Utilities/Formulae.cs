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
            (x,y) => 07.00 * (Math.Sin(x + (x * 2.25)) + Math.Cos(y + (y/7))), //personal fave
            (x,y) => 04.65 * (Math.Sin(x) - (2*Math.Cos(4* y))), //freckles
            (x,y) => 07.77 * (Math.Cos((x *x) + (y * y))), //static
            //(x,y) => 04.44 * (Math.Cos(x + y * Math.PI / 180) + Math.Sin(x + y * Math.PI / 180)), //smooth vertical lines
            //(x,y) => 08.88 * (Math.Tan(x / 13 % 1.99) + Math.Tan(y % 1.91)), //horizontal lines
            //(x,y) => 05.11 * (Math.Cos(Math.Sqrt(Math.Abs((x*x) + (y*y))))), //concentric circles origination from the top left
            //(x,y) => 08.75 * (Math.Cos(Math.Sqrt(Math.Abs((-x*x) + (y*y))))), //pretty, curved lines
            (x,y) => 13.31 * (Math.Tan((-2 * x * y) % 1.99)), //nice, simple pattern
            (x,y) => 11.11 * (Math.Tan(Math.Sqrt(x*y))), //nice, indiscernible pattern
            (x,y) => 12.00 * (Math.Cos(y) * Math.Tan(x)), //looks natural
            (x,y) => 15.00 * (Math.Sin(x) * Math.Tan(y)), //subtle
            (x,y) => 08.88 * (Math.Sin(x / (y+1)) * Math.Tan(y * x)), //subtle
            (x,y) => 19.19 * (Math.Sin(x + y) * Math.Cos(y * x)), //subtle
        };
        public static Func<int, int, double> RandomTerrainGenFormula()
        {
            return TerrainGenerationFormulae.RandomItem();
        }

        #endregion

        #region wind
        public static List<Func<int, int, TimeSpan, double>> EastBoundWindFormulae = new List<Func<int, int, TimeSpan, double>>()
        {
            (x,y,t) => Math.Sin(x - t.TotalMilliseconds / 650) + Math.Cos(y - t.TotalMilliseconds / 325), //bubbles going southeast
            //(x,y,t) => Math.Cos(t.TotalMilliseconds / 777 + Math.Sqrt(Math.Abs(y * y * 0.75 - x * 3.5))) > 0.9, //odd waves going east
            (x,y,t) => -Math.Cos(x * 3.45 - t.TotalMilliseconds / 777) - Math.Sin(y*0.77 - t.TotalMilliseconds / 77), //bubbles snaking east
            //(x,y,t) => Math.Cos(-t.TotalMilliseconds / 333 - Math.Sqrt(Math.Abs((-x*x) + (y*y)))) > 0.9, //spiraling waves going northeast
            //(x,y,t) => Math.Cos(-t.TotalMilliseconds / 444 + Math.Sqrt((x*x) + (y*y))) > 0.9, //concentric waves going southeast
            
        };
        public static List<Func<int, int, TimeSpan, double>> WestBoundWindFormulae = new List<Func<int, int, TimeSpan, double>>()
        {
            (x,y,t) => Math.Sin(x / 3.33 + t.TotalSeconds) + Math.Cos(y * 3.33 + t.TotalSeconds), //slow lines west-northwest
            (x,y,t) => Math.Sin(x + t.TotalMilliseconds / 444) + Math.Cos(y + t.TotalMilliseconds / 333),//bubbles going northwest
            (x,y,t) => x*1.111 + Math.Tan(y*.875) - t.TotalMilliseconds / 250,//odd lines marching west, unbound
            (x,y,t) => (int)(x * (x / 8) + Math.Sin(y)*4 + t.TotalSeconds * 25),//periodic gentle west breeze
            //(x,y,t) => Math.Cos(t.TotalMilliseconds / 222 + Math.Sqrt((x*x) + (y*y))) > 0.95, //concentric waves converging on northwest
            //(x,y,t) => Math.Cos(t.TotalMilliseconds / 666 + Math.Sqrt(Math.Abs((x * y) + y))) > 0.9, //waves going northwest

        };
        
        public static List<Func<int, int, TimeSpan, double>> NorthBoundWindFormulae = new List<Func<int, int, TimeSpan, double>>()
        {
            //(x,y,t) => Math.Cos(t.TotalMilliseconds / 666 + Math.Sqrt(Math.Abs((x * y) + y))) > 0.9, //waves going northwest
            //(x,y,t) => Math.Sin(x * 7.77 + t.TotalSeconds) + Math.Cos(y / 7.77 + t.TotalSeconds) > 1.1, //lines snaking north-northwest
            (x,y,t) => Math.Sin(x / 3.33 + t.TotalMilliseconds / 666) + Math.Cos(y * 8.76 + t.TotalMilliseconds / 222), //fast north-northwest bubbles
            //(x,y,t) => Math.Cos(x * 1.125 - t.TotalMilliseconds / 111) + Math.Sin(y + t.TotalMilliseconds / 999) > 1.55, //wavy line crawling north
            (x,y,t) => Math.Cos(y + t.TotalSeconds) + Math.Sin(x * y - t.TotalMilliseconds / 666), //marching breeze north
            //(x,y,t) => Math.Cos(x * y - t.TotalMilliseconds / 222) + Math.Sin(y * 4 - t.TotalMilliseconds / 222) > 1.55, //chaotic breeze northward with cross-breezes
            (x,y,t) => (Math.Cos(x)* 3.75 + x + y * 4.15 + t.TotalSeconds * 25),//large, sloped sin waves marching north
            //(x,y,t) => Math.Sin(x - t.TotalMilliseconds / 111) + Math.Cos(y + t.TotalMilliseconds / 333) > 1.25, //lines zig-zagging north

        };
        public static List<Func<int, int, TimeSpan, double>> SouthBoundWindFormulae = new List<Func<int, int, TimeSpan, double>>()
        {
            //(x,y,t) => Math.Sin(x + t.TotalMilliseconds / 111) + Math.Cos(y - t.TotalMilliseconds / 555) > 1.25, //waves zig-zagging south
            (x,y,t) => Math.Sin(x - t.TotalMilliseconds / 650) - Math.Cos(y - t.TotalMilliseconds / 325), //bubbles snake south-southeast
            (x,y,t) => Math.Cos(x * 3.45 - t.TotalMilliseconds / 500) - Math.Sin(y*0.77 - t.TotalMilliseconds / 250), //bubbles waving south
            //(x,y,t) => Math.Cos(x * y - t.TotalMilliseconds / 999) + Math.Sin(y - t.TotalMilliseconds / 111) > 1.55, //chaotic blowing south with cross-breezes 
            //(x,y,t) => (int)(y*4.4 + Math.Tan(x*0.666) - t.TotalMilliseconds / 188) % 69 == 1,//gentle south waves
            (x,y,t) => Math.Cos(x * 0.88 + t.TotalMilliseconds / 333) + Math.Sin(y*1.125 - t.TotalMilliseconds / 777), //bubbles slowling going southwest

        };

        public static List<Func<int, int, TimeSpan, double>> ChaoticWindPatterns = new List<Func<int, int, TimeSpan, double>>()
        {
            (x,y,t) => Math.Cos(t.TotalMilliseconds / 555 + Math.Sqrt(Math.Abs((x*x) + (y * 20)))), //spiraling towards northwest
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
