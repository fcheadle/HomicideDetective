using GoRogue;
using GoRogue.GameFramework;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;

namespace Engine.Maps
{
    internal static class TerrainFactory
    {
        internal static BasicTerrain Copy(BasicTerrain source, Coord target) => new BasicTerrain(source.Foreground, source.Background, source.Glyph, target, source.IsWalkable, source.IsTransparent);
        #region tiles
        internal static BasicTerrain Grass(Coord position, double z = 0)
        {
            Color color = Color.Green;
            int chance;
            for (double k = 0; k < z; k++)
                color = Colors.Brighten(color);
            for (double k = z; k < 0; k++)
                color = Colors.Darken(color);

            if (color.B > color.G)
            {
                Color target;
                chance = Calculate.Percent();
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

                color = Colors.MutateBy(color, target);
            }

            if (color.R > color.G)
            {
                Color target;
                chance = Calculate.Percent();
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

                color = Colors.MutateBy(color, target);
            }

            int glyph;
            chance = Calculate.Percent();
            if (chance < 10)
                glyph = '\'';
            else if (chance < 20)
                glyph = '`';
            else if (chance < 30)
                glyph = '.';
            else if (chance < 40)
                glyph = ',';
            else if (chance < 90)
                glyph = '"';
            else
                glyph = '*';
            BasicTerrain t = new BasicTerrain(color, Color.Black, glyph, position, true, true);
            return t;
        }
        internal static BasicTerrain Pavement(Coord position) => new BasicTerrain(Color.DarkGray, Color.Black, 247, position, true,true);
        internal static BasicTerrain Wall(Coord position) => new BasicTerrain(Color.White, Color.Black, '#', position, false,false);
        internal static BasicTerrain Fence(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 140, position, false, true);
        internal static BasicTerrain FenceGate(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 15, position, true, true);
        internal static BasicTerrain Door(Coord position) => new BasicTerrain(Color.LightGray, Color.Black, 234, position, true,false);
        internal static BasicTerrain Carpet(Coord position) => new BasicTerrain(Colors.Half(Color.Olive), Colors.Half(Color.DarkOliveGreen), position.Y % 2 == 1 ? 16 : 17, position, true, true);
        internal static BasicTerrain Linoleum(Coord position) => new BasicTerrain(Color.LightGray, Color.DarkGray, 4, position, true, true);
        internal static BasicTerrain Window(Coord position) => new BasicTerrain(Color.Transparent, Color.Black, 0, position, false, true);
        internal static BasicTerrain HardwoodFloor(Coord position) => new BasicTerrain(Colors.Half(Color.SaddleBrown), Colors.Half(Color.Brown), position.Y % 2 == 1 ? 174 : 175, position, true, true);
        internal static BasicTerrain Tree(Coord position) => new BasicTerrain(Colors.MutateBy(Color.Brown, Color.Black), Color.Black, '0', position, false, false);
        #endregion

        #region test tiles
        internal static BasicTerrain TestSquare1(Coord position) => new BasicTerrain(Color.Cyan, Color.Navy, '1', position, true, true);
        internal static BasicTerrain TestSquare2(Coord position) => new BasicTerrain(Color.Magenta, Color.Red, '2', position, true, true);
        internal static BasicTerrain TestSquare3(Coord position) => new BasicTerrain(Color.Green, Color.Olive, '3', position, true, true);
        internal static BasicTerrain TestSquare4(Coord position) => new BasicTerrain(Color.LightGreen, Color.DarkGreen, '4', position, true, true);
        internal static BasicTerrain TestSquare5(Coord position) => new BasicTerrain(Color.Yellow, Color.Orange, '5', position, true, true);
        internal static BasicTerrain TestSquare6(Coord position) => new BasicTerrain(Color.Blue, Color.Purple, '6', position, true, true);
        internal static BasicTerrain TestSquare7(Coord position) => new BasicTerrain(Color.Purple, Color.Red, '7', position, true, true);
        internal static BasicTerrain TestSquare8(Coord position) => new BasicTerrain(Color.Red, Color.Orange, '8', position, true, true);
        internal static BasicTerrain TestSquare9(Coord position) => new BasicTerrain(Color.Yellow, Color.Violet, '9', position, true, true);
        internal static BasicTerrain TestSquare0(Coord position) => new BasicTerrain(Color.Tan, Color.DarkGray, '0', position, true, true);
        #endregion
    }
}