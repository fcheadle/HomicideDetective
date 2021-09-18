using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using HomicideDetective.Things;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.People
{
    /// <summary>
    /// A fingerprint, the unlimited marking which leaves partial prints
    /// </summary>
    /// <remarks>
    /// The pattern is represented in a 64x64 ArrayView of bool for easy memory/rending.
    /// </remarks>
    public class Fingerprint : Marking
    {
        private int _seed;
        private const int _width = 64;
        private const int _height = 64;
        
        public ArrayView<FingerPrintCAState> Pattern { get; }

        public Fingerprint(int seed)
        {
            _seed = seed;
            Name = "fingerprint";
            Pattern = Generate();
        }

        public ArrayView<FingerPrintCAState> Generate()
        {
            var random = new Random(_seed);
            var pattern = SeedStartingPattern(random);
            
            while (pattern.Positions().Any(p => pattern[p] == FingerPrintCAState.Undefined))
            {
                pattern = NextFingerPrintCycle(pattern);
            }

            pattern = FinalizeRidges(pattern);

            return pattern;
        }

        private ArrayView<FingerPrintCAState> FinalizeRidges(ArrayView<FingerPrintCAState> pattern)
        {
            for (int i = 0; i < pattern.Width; i++)
            {
                for (int j = 0; j < pattern.Height; j++)
                {
                    if (DistanceBetween((i, j), (_width / 2, _height / 2)) > _width/2)
                    {
                        pattern[i, j] = FingerPrintCAState.Groove;
                    }
                    else if (pattern[i, j] == FingerPrintCAState.RidgeUndefined)
                    {
                        pattern[i,j] = DistinguishRidgeFromNeighbors(pattern, (i, j));
                    }
                }
            }
        
            return pattern;
        }

        private ArrayView<FingerPrintCAState> SeedStartingPattern(Random random)
        {
            var pattern = new ArrayView<FingerPrintCAState>(_width, _height);

            //pick 8+4d8 starting points
            int startingPointCount = 8;
            startingPointCount += random.Next(0, 8) + 1;
            startingPointCount += random.Next(0, 8) + 1;
            startingPointCount += random.Next(0, 8) + 1;
            startingPointCount += random.Next(0, 8) + 1;

            for(int i = 0; i <  startingPointCount; i++)
            {
                var x = random.Next(1, 62);
                var y = random.Next(1, 62);
                
                int x1 = random.Next(1, 63);
                int y1 = random.Next(1, 63);
                int x2 = random.Next(1, 63);
                int y2 = random.Next(1, 63);
                
                foreach (var point in Lines.Get(x1, y1, x2, y2))
                {
                    var chance = random.Next(1, 101);
                    pattern[point] = chance % 100 <= 20 ? FingerPrintCAState.RidgeUndefined : FingerPrintCAState.Groove;
                }
            }
            return pattern;
        }

        private double DistanceBetween(Point p1, Point p2)
        {
            var xPrime = (p2.X - p1.X) * (p2.X - p1.X);
            var yPrime = (p2.Y - p1.Y) * (p2.Y - p1.Y);
            return Math.Sqrt(xPrime + yPrime);
        }

        private ArrayView<FingerPrintCAState> NextFingerPrintCycle(ArrayView<FingerPrintCAState> pattern)
        {
            var changes = new Dictionary<Point, FingerPrintCAState>();
            for (int i = 0; i < pattern.Width; i++)
            {
                for (int j = 0; j < pattern.Height; j++)
                {
                    if (pattern[i, j] == FingerPrintCAState.Undefined)
                    {
                        var change = SetStateFromUndefined(pattern, (i, j));
                        if (change != FingerPrintCAState.Undefined)
                            changes.Add((i, j), change);
                    }
                }
            }

            int k = 0;
            foreach (var change in changes)
            {
                k++;
                if (k % 13 == 0)
                {
                    if (Ridge(pattern, change.Key))
                        pattern[change.Key] = FingerPrintCAState.Groove;
                    else
                        pattern[change.Key] = FingerPrintCAState.RidgeUndefined;
                }
                pattern[change.Key] = change.Value;
            }

            return pattern;
        }

        private bool Ridge(ArrayView<FingerPrintCAState> pattern, Point pos) 
            => pattern[WrapPoint(pos)] >= FingerPrintCAState.RidgeUndefined;
        
        private FingerPrintCAState DistinguishRidgeFromNeighbors(ArrayView<FingerPrintCAState> pattern, Point pos)
        {
            var upRight = WrapPoint(pos + (1, -1));
            var downRight = WrapPoint(pos + (1, 1));
            var upLeft = WrapPoint(pos + (-1, -1));
            var downLeft = WrapPoint(pos + (-1, 1));

            var left = WrapPoint(pos + (-1, 0));
            var right = WrapPoint(pos + (1, 0));
            var down = WrapPoint(pos + (0, 1));
            var up = WrapPoint(pos + (0, -1));
            
            //Major lines
            if (Ridge(pattern, left) && Ridge(pattern, right))
                return FingerPrintCAState.RidgeLeftRightThick;
            
            if (Ridge(pattern, down) && Ridge(pattern, up))
                return FingerPrintCAState.RidgeUpDownThick;
            
            //top-left and bottom-right quadrants
            if ((pos.X < _width / 2 && pos.Y < _height/2) || (pos.X >= _width/2 && pos.Y >= _height/2))
            {
                if (Ridge(pattern, upRight) && Ridge(pattern, downLeft))
                    return FingerPrintCAState.RidgeSlash;
                if (Ridge(pattern, upRight) && Ridge(pattern, left))
                    return FingerPrintCAState.RidgeSlash;
                if (Ridge(pattern, downLeft) && Ridge(pattern, right))
                    return FingerPrintCAState.RidgeSlash;
                
                if (Ridge(pattern, upLeft) && Ridge(pattern, downRight))
                    return FingerPrintCAState.RidgeBackSlash;
                if (Ridge(pattern, upLeft) && Ridge(pattern, right))
                    return FingerPrintCAState.RidgeBackSlash;
                if (Ridge(pattern, left) && Ridge(pattern, downRight))
                    return FingerPrintCAState.RidgeBackSlash;
            }
            else
            {
                if (Ridge(pattern, upLeft) && Ridge(pattern, downRight))
                    return FingerPrintCAState.RidgeBackSlash;
                if (Ridge(pattern, upLeft) && Ridge(pattern, right))
                    return FingerPrintCAState.RidgeBackSlash;
                if (Ridge(pattern, left) && Ridge(pattern, downRight))
                    return FingerPrintCAState.RidgeBackSlash;
                
                if (Ridge(pattern, upRight) && Ridge(pattern, downLeft))
                    return FingerPrintCAState.RidgeSlash;
                if (Ridge(pattern, upRight) && Ridge(pattern, left))
                    return FingerPrintCAState.RidgeSlash;
                if (Ridge(pattern, downLeft) && Ridge(pattern, right))
                    return FingerPrintCAState.RidgeSlash;
            }
            
            //connecting long lines
            if (Ridge(pattern, upRight) && Ridge(pattern, down))
                return FingerPrintCAState.RidgeSlash;
            if (Ridge(pattern, up) && Ridge(pattern, downLeft))
                return FingerPrintCAState.RidgeSlash;
            if (Ridge(pattern, upLeft) && Ridge(pattern, down))
                return FingerPrintCAState.RidgeBackSlash;
            if (Ridge(pattern, up) && Ridge(pattern, downRight))
                return FingerPrintCAState.RidgeBackSlash;
            
            //multiple connections on bottom
            if (Ridge(pattern, downLeft) && Ridge(pattern, downRight))
                return FingerPrintCAState.RidgeA;
            if (Ridge(pattern, downLeft) && Ridge(pattern, down))
                return FingerPrintCAState.RidgeA;
            if (Ridge(pattern, downRight) && Ridge(pattern, down))
                return FingerPrintCAState.RidgeA;
            
            //Multiple connections from above
            if (Ridge(pattern, upLeft) && Ridge(pattern, upRight))
                return FingerPrintCAState.RidgeV;
            if (Ridge(pattern, upLeft) && Ridge(pattern, up))
                return FingerPrintCAState.RidgeV;
            if (Ridge(pattern, upRight) && Ridge(pattern, up))
                return FingerPrintCAState.RidgeV;
            
            //multiple connections from the right
            if (Ridge(pattern, downRight) && Ridge(pattern, upRight))
                return FingerPrintCAState.RidgeLessThan;
            if (Ridge(pattern, downRight) && Ridge(pattern, right))
                return FingerPrintCAState.RidgeLessThan;
            if (Ridge(pattern, upRight) && Ridge(pattern, right))
                return FingerPrintCAState.RidgeLessThan;
            
            //multiple connections from the left
            if (Ridge(pattern, downLeft) && Ridge(pattern, upLeft))
                return FingerPrintCAState.RidgeGreaterThan;
            if (Ridge(pattern, downLeft) && Ridge(pattern, left))
                return FingerPrintCAState.RidgeGreaterThan;
            if (Ridge(pattern, upLeft) && Ridge(pattern, left))
                return FingerPrintCAState.RidgeGreaterThan;
            
            //minor lines
            if (Ridge(pattern, left) || Ridge(pattern, right))
                return FingerPrintCAState.RidgeLeftRightThin;
            if (Ridge(pattern, down) || Ridge(pattern, up))
                return FingerPrintCAState.RidgeUpDownThin;
            
            return FingerPrintCAState.RidgeDefined;
        }
        

        private FingerPrintCAState SetStateFromUndefined(ArrayView<FingerPrintCAState> pattern, Point pos)
        {
            var neighbors = GetCardinalNeighboringStates(pattern, pos);
            if(neighbors.Contains(FingerPrintCAState.Groove))
                return FingerPrintCAState.RidgeUndefined;
            else if(neighbors.Any(Ridge))
            {
                if (Ridge(pattern, pos + (1, 0)))
                    return FingerPrintCAState.Groove;
                if (Ridge(pattern, pos + (-1, 0)))
                    return FingerPrintCAState.Groove;
                if (Ridge(pattern, pos + (0, -1)))
                    return FingerPrintCAState.Groove;
                if (Ridge(pattern, pos + (0, 1)))
                    return FingerPrintCAState.Groove;
            }

            return FingerPrintCAState.Undefined;
        }

        private bool Ridge(FingerPrintCAState pattern) => pattern >= FingerPrintCAState.RidgeUndefined;

        public virtual IEnumerable<FingerPrintCAState> GetCardinalNeighboringStates(ArrayView<FingerPrintCAState> pattern, Point point)
        {
            //cardinals
            yield return pattern[WrapPoint(point + (0, 1))];
            yield return pattern[WrapPoint(point + (0, -1))];
            yield return pattern[WrapPoint(point + (-1, 0))];
            yield return pattern[WrapPoint(point + (1, 0))];
        }
        public virtual IEnumerable<FingerPrintCAState> GetDiagonalNeighboringStates(ArrayView<FingerPrintCAState> pattern, Point point)
        {
            //diagonals
            yield return pattern[WrapPoint(point + (1, 1))];
            yield return pattern[WrapPoint(point + (1, -1))];
            yield return pattern[WrapPoint(point + (-1, 1))];
            yield return pattern[WrapPoint(point + (-1, -1))];
        }
        
        protected Point WrapPoint(Point point)
        {
            if (point.X >= _width)
                point = (_width - 1, point.Y);
            if (point.X < 0)
                point = (0, point.Y);

            if (point.Y >= _height)
                point = (point.X, _height - 1);
            if (point.Y < 0)
                point = (point.X, 0);

            return point;
        }
        public IEnumerable<string> PatternMap()
        {
            for (int i = 0; i < Pattern.Height; i++)
            {
                var line = "";

                for (int j = 0; j < Pattern.Width; j++)
                {
                    line += GetSymbol(Pattern[j, i]);
                }

                yield return line;
            }
        }

        private char GetSymbol(FingerPrintCAState fingerPrintCaState)
        {
            switch (fingerPrintCaState)
            {
                case FingerPrintCAState.Groove: return ' ';
                case FingerPrintCAState.RidgeA: return '^';
                case FingerPrintCAState.RidgeDefined: return '#';
                case FingerPrintCAState.RidgeV: return 'V';
                case FingerPrintCAState.RidgeSlash: return '/';
                case FingerPrintCAState.RidgeBackSlash: return '\\';
                case FingerPrintCAState.RidgeGreaterThan: return ')';
                case FingerPrintCAState.RidgeLessThan: return '(';
                case FingerPrintCAState.RidgeLeftRightThick: return '-';//=
                case FingerPrintCAState.RidgeLeftRightThin: return '\'';//-
                case FingerPrintCAState.RidgeUpDownThick: return '|';
                case FingerPrintCAState.RidgeUpDownThin: return ':';//:
                default: return '#';
            }
        }
    }

    public enum FingerPrintCAState
    {
        Undefined, 
        Groove,
        RidgeUndefined,
        RidgeDefined,
        RidgeV,
        RidgeA,
        RidgeLessThan,
        RidgeGreaterThan,
        RidgeUpDownThin,
        RidgeUpDownThick,
        RidgeLeftRightThin,
        RidgeLeftRightThick,
        RidgeSlash,
        RidgeBackSlash,
    }
}
