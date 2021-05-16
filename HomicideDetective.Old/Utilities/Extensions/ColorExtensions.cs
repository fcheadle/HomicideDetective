using System;
using HomicideDetective.Old.Utilities.Mathematics;
using Color = Microsoft.Xna.Framework.Color;
// ReSharper disable PossibleLossOfFraction

namespace HomicideDetective.Old.Utilities.Extensions
{
    public static class ColorExtensions
    {
        public static Color FadeOut(this Color color)
        {
            color.A -= Convert.ToByte(1);
            return color;
        }

        public static Color FadeIn(this Color color)
        {
            color.A += Convert.ToByte(1);
            return color;
        }
        public static Color Darken(this Color color)
        {
            int r = Convert.ToInt32(color.R);
            int g = Convert.ToInt32(color.G);
            int b = Convert.ToInt32(color.B);
            int total = 1 + r + g + b;
            double rPercentValue = r / total;
            double bPercentValue = b / total;

            double chance;

            for (int i = 0; i < 10; i++)
            {
                chance = Calculate.PercentValue();
                if (chance <= rPercentValue)
                {
                    color.R -= Convert.ToByte(1);
                }
                else if (chance <= bPercentValue + rPercentValue)
                {
                    color.B -= Convert.ToByte(1);
                }
                else
                    color.G -= Convert.ToByte(1);
            }
            return color;
        }

        public static Color Brighten(this Color color)
        {
            int r = Convert.ToInt32(color.R);
            int g = Convert.ToInt32(color.G);
            int b = Convert.ToInt32(color.B);
            int total = 1 + r + g + b;
            double rPercentValue = r / total;
            double bPercentValue = b / total;
            double gPercentValue = g / total;

            double chance;

            for (int i = 0; i < 10; i++)
            {
                chance = Calculate.PercentValue();
                if (chance <= rPercentValue)
                {
                    //if (Convert.ToInt32(color.R) < 255)
                    color.R += Convert.ToByte(1);
                }
                else if (chance <= bPercentValue + rPercentValue)
                {
                    //if (Convert.ToInt32(color.B) < 255)
                    color.G += Convert.ToByte(1);
                }
                else if (chance <= bPercentValue + rPercentValue + gPercentValue)
                    //if (Convert.ToInt32(color.G) < 255)
                    color.B += Convert.ToByte(1);
            }
            return color;
        }
        public static Color MutateToIndex(this Color color, double z)
        {
            for (double k = 0; k < z; k++)
                color = color.Brighten();
            for (double k = z; k < 0; k++)
                color = color.Darken();

            return color;
        }
        public static Color MutateBy(this Color baseColor, Color target)
        {
            Color newC = Color.Black;
            newC.R = Convert.ToByte((baseColor.R + target.R) / 4);
            newC.G = Convert.ToByte((baseColor.G + target.G) / 4);
            newC.B = Convert.ToByte((baseColor.B + target.B) / 4);
            return newC;
        }

        public static Color Half(this Color color)
        {
            return color.MutateBy(Color.Black);
        }
        public static Color Double(this Color color)
        {
            Color c = color;
            int b = c.B * 2 > 255 ? 255 : c.B * 2;
            c.B = Convert.ToByte(b);
            int r = c.R * 2 > 255 ? 255 : c.R * 2;
            c.R = Convert.ToByte(r);
            int g = c.G * 2 > 255 ? 255 : c.G * 2;
            c.G = Convert.ToByte(g);
            return c;
        }

        public static Color Greenify(this Color color)
        {
            if (color.R > color.G || color.B > color.G)
            {
                Color target;
                int chance = Calculate.PercentValue();
                if (chance < 10)
                    target = Color.DarkGreen;
                else if (chance < 20)
                    target = Color.LightGreen;
                else if (chance < 30)
                    target = Color.MediumSpringGreen;
                else if (chance < 40)
                    target = Color.SpringGreen;
                else if (chance < 50)
                    target = Color.DarkOliveGreen;
                else if (chance < 60)
                    target = Color.DarkGray;
                else if (chance < 70)
                    target = Color.Olive;
                else if (chance < 80)
                    target = Color.OliveDrab;
                else if (chance < 90)
                    target = Color.DarkSeaGreen;
                else
                    target = Color.ForestGreen;

                color = color.MutateBy(target);
            }

            return color;
        }

        public static Color Redify(this Color color)
        {
            if (color.G > color.R || color.B > color.R)
            {
                Color target;
                int chance = Calculate.PercentValue();
                if (chance < 10)
                    target = Color.DarkRed;
                else if (chance < 20)
                    target = Color.Maroon;
                else if (chance < 30)
                    target = Color.Magenta;
                else if (chance < 40)
                    target = Color.Red;
                else if (chance < 50)
                    target = Color.IndianRed;
                else if (chance < 60)
                    target = Color.MediumVioletRed;
                else if (chance < 70)
                    target = Color.MistyRose;
                else if (chance < 80)
                    target = Color.OrangeRed;
                else if (chance < 90)
                    target = Color.PaleVioletRed;
                else
                    target = Color.DeepPink;

                color = color.MutateBy(target);
            }

            return color;
        }

        public static Color Blueify(this Color color)
        {
            if (color.R > color.B || color.G > color.B)
            {
                Color target;
                int chance = Calculate.PercentValue();
                if (chance < 10)
                    target = Color.DarkBlue;
                else if (chance < 20)
                    target = Color.BlueViolet;
                else if (chance < 30)
                    target = Color.AliceBlue;
                else if (chance < 40)
                    target = Color.CadetBlue;
                else if (chance < 50)
                    target = Color.CornflowerBlue;
                else if (chance < 60)
                    target = Color.DarkSlateBlue;
                else if (chance < 70)
                    target = Color.Navy;
                else if (chance < 80)
                    target = Color.OliveDrab;
                else if (chance < 90)
                    target = Color.DarkSeaGreen;
                else
                    target = Color.ForestGreen;

                color = color.MutateBy(target);
            }

            return color;
        }
    }
}
