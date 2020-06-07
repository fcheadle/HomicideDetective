using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities
{
    public static class Formulae
    {
        public static float HeartBeat(float period) => 2f * (float)Math.Sin(Math.Sin(10 / period));
    }
}
