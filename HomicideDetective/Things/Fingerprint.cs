﻿using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using HomicideDetective.Mysteries;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace HomicideDetective.Things
{
    /// <summary>
    /// A fingerprint, the unlimited marking which leaves partial prints
    /// </summary>
    /// <remarks>
    /// The pattern is represented in a 64x64 ArrayView of bool for easy memory/rending.
    /// </remarks>
    public class Fingerprint : Marking, IGameObjectComponent, IDetailed
    {
        public IGameObject? Parent { get; set; }
        
        private int _seed;
        
        public ArrayView<bool> Pattern { get; }

        public Fingerprint(int seed)
        {
            _seed = seed;
            Name = "fingerprint";
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

            var startingPoints = new List<Point>();
            for(int i = 0; i <  startingPointCount; i++)
            {
                int x = random.Next(16, 49);
                int y = random.Next(16, 49);
                startingPoints.Add((x,y));
            }

            foreach (var point in startingPoints)
            {
                Pattern[point] = true;
            }
            
            //todo - use a CA / pathfinder to finish from starting points
        }

        //returns the pattern as a list of strings
        public List<string> Details
        {
            get
            {
                var answer = new string[64];
                for (int i = 0; i < Pattern.Height; i++)
                {
                    var line = "";

                    for (int j = 0; j < Pattern.Width; j++)
                    {
                        line += Pattern[j, i] ? "X" : " ";
                    }

                    answer[i] = line;
                }

                return answer.ToList();
            }
        }
    }
}
