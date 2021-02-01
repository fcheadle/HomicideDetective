using System;
using System.Collections.Generic;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Things.Markings
{
    class Fingerprint : Marking
    {
        private int _seed;
        public ArrayView<bool> Pattern { get; }

        public Fingerprint(int seed)
        {
            _seed = seed;
            Pattern = new ArrayView<bool>(64, 64);
        }

        public void Generate()
        {
            var random = new Random(_seed);
            
            //pick 3d2 starting points
            int startingPointCount = 0;
            startingPointCount += random.Next(0, 2) + 1;
            startingPointCount += random.Next(0, 2) + 1;
            startingPointCount += random.Next(0, 2) + 1;

            List<Point> startingPoints = new List<Point>();
            for(int i = 0; i <  startingPointCount; i++)
            {
                int x = random.Next(16, 49);
                int y = random.Next(16, 49);
                startingPoints.Add((x,y));
            }
            
            
        }
    }
}
