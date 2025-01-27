using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class UnitAttribute : PropertyAttribute
    {
        public readonly string Unit;
        public UnitAttribute(string unit) 
        {
            Unit = unit;
            order = 1000000;
        }
    }
    public class MinMaxAttribute : PropertyAttribute
    {
        public readonly float Min;
        public readonly float Max;

        public MinMaxAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }

    public class  ConditionalAttribute : PropertyAttribute
    {
        public readonly string Condition;

        public ConditionalAttribute(string condition)
        {
            Condition = condition;
            order = -1;
        }
    }

    public class FontStyleAttribute : PropertyAttribute
    {
        public readonly bool UseBold;
        public readonly bool UseItalic;

        public FontStyleAttribute(bool bold, bool italic)
        {
            UseBold = bold;
            UseItalic = italic;
        }
    }
    public class ReadonlyAttribute : PropertyAttribute
    {
        public readonly bool ReadonlyWhenPlaying;

        public ReadonlyAttribute()
        {
            ReadonlyWhenPlaying = false;
        }
        public ReadonlyAttribute(bool whenPlaying)
        {
            ReadonlyWhenPlaying = whenPlaying;
        }
    }
}
