using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class SUnitAttribute : PropertyAttribute
    {
        public readonly string Unit;
        public SUnitAttribute(string unit) 
        {
            Unit = unit;
            order = 1000000;
        }
    }
    public class SMinMaxAttribute : PropertyAttribute
    {
        public readonly float Min;
        public readonly float Max;

        public SMinMaxAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }

    public class  SConditionalAttribute : PropertyAttribute
    {
        public readonly string Condition;

        public SConditionalAttribute(string condition)
        {
            Condition = condition;
            order = -1;
        }
    }

    public class SFontStyleAttribute : PropertyAttribute
    {
        public readonly bool UseBold;
        public readonly bool UseItalic;

        public SFontStyleAttribute(bool bold, bool italic)
        {
            UseBold = bold;
            UseItalic = italic;
        }
    }
    public class SReadonlyAttribute : PropertyAttribute
    {
        public readonly bool ReadonlyWhenPlaying;

        public SReadonlyAttribute()
        {
            ReadonlyWhenPlaying = false;
        }
        public SReadonlyAttribute(bool whenPlaying)
        {
            ReadonlyWhenPlaying = whenPlaying;
        }
    }
}
