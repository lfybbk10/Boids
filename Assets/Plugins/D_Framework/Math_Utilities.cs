using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace D_Framework
{
    public static class Math_Utilities 
    {
        public static int Remap(this int value, int aLow, int aHigh, int bLow, int bHigh)
        {
            return (int)value.Remap((float)aLow, aHigh, bLow, bHigh);
            //return (int)((float)(bLow + (value - aLow) * (bHigh - bLow)) / (aHigh - aLow) * bHigh);
        }

        public static float Remap(this float value, float aLow, float aHigh, float bLow, float bHigh)
        {
            float normal = Mathf.InverseLerp(aLow, aHigh, value);
            return Mathf.Lerp(bLow, bHigh, normal);
        }

        public static float Remap(this int value, float aLow, float aHigh, float bLow, float bHigh)
        {
            float normal = Mathf.InverseLerp(aLow, aHigh, value);
            return Mathf.Lerp(bLow, bHigh, normal);
        }
    }
}