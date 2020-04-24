using System;
using Color = Microsoft.Xna.Framework.Color;

namespace Engine
{
    internal class Colors
    {
        internal static Color Darken(Color color)
        {
            int r = Convert.ToInt32(color.R);
            int g = Convert.ToInt32(color.G);
            int b = Convert.ToInt32(color.B);

            int total = 1 + r + g + b;
            double rPercent = r / total;
            double bPercent = b / total;
            double gPercent = g / total;

            Random rand = new Random();
            double chance;

            for (int i = 0; i < 10; i++)
            {
                chance = rand.Next(0, 100);
                if (chance <= rPercent)
                {
                    //if (Convert.ToInt32(color.R) > 1)
                    color.R -= Convert.ToByte(1);
                }
                else if (chance <= bPercent + rPercent)
                {
                    //if (Convert.ToInt32(color.B) > 1)
                    color.B -= Convert.ToByte(1);
                }
                else
                    //if (Convert.ToInt32(color.G) > 1)
                    color.G -= Convert.ToByte(1);
            }
            return color;
        }

        internal static Color FadeOut(Color color)
        {
            color.A += Convert.ToByte(1);
            return color;
        }

        internal static Color FadeIn(Color color)
        {
            color.A -= Convert.ToByte(1);
            return color;
        }

        internal static Color Brighten(Color color)
        {
            int total = 1;
            int r = Convert.ToInt32(color.R);
            int g = Convert.ToInt32(color.G);
            int b = Convert.ToInt32(color.B);
            byte one = Convert.ToByte(1);
            total += 1 + r + g + b;
            double rPercent = r / total;
            double bPercent = b / total;
            double gPercent = g / total;

            Random rand = new Random();
            double chance;

            for (int i = 0; i < 10; i++)
            {
                chance = (double)rand.Next(0, 100);
                if (chance <= rPercent)
                {
                    //if (Convert.ToInt32(color.R) < 255)
                    color.R += Convert.ToByte(1);
                }
                else if (chance <= bPercent + rPercent)
                {
                    //if (Convert.ToInt32(color.B) < 255)
                    color.B += Convert.ToByte(1);
                }
                else
                    //if (Convert.ToInt32(color.G) < 255)
                    color.G += Convert.ToByte(1);
            }
            return color;
        }

        internal static Color MutateBy(Color baseColor, Color target)
        {
            Color newC = Color.Black;
            newC.R = Convert.ToByte((baseColor.R + target.R) / (byte)4);
            newC.G = Convert.ToByte((baseColor.G + target.G) / (byte)4);
            newC.B = Convert.ToByte((baseColor.B + target.B) / (byte)4);
            return newC;
        }

        internal static Color Half(Color color)
        {
            int r = Convert.ToInt32(color.R);
            int g = Convert.ToInt32(color.G);
            int b = Convert.ToInt32(color.B);
            r /= 2;
            g /= 2;
            b /= 2;
            return new Color(r, g, b);
        }
    }
}
