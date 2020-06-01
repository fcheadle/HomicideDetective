using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities
{
    public static class EnumUtils
    {
        public static int EnumLength<T>() where T : Enum
        {
            return Enum.GetNames(typeof(T)).Length;
        }
        public static T RandomEnumValue<T>() where T : Enum
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new Random().Next(v.Length));
        }
    }
}
