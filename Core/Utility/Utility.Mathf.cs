using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        //Is Positive
        public static bool IsPositive(this float value)
        {
            return value > 0f;
        }
        public static bool IsPositive(this int value)
        {
            return value > 0;
        }
        // IsNegative
        public static bool IsNegative(this float value)
        {
            return value < 0f;
        }
        public static bool IsNegative(this int value)
        {
            return value < 0;
        }
        // In Range
        public static bool InRange(this float value, float min, float max, bool inclusiveMin = true, bool inclusiveMax = true)
        {
            if (inclusiveMin)
            {
                if (inclusiveMax)
                {
                    return value >= min && value <= max;
                }
                else
                {
                    return value >= min && value < max;
                }
            }
            else
            {
                if (inclusiveMax)
                {
                    return value > min && value <= max;
                }
                else
                {
                    return value > min && value < max;
                }
            }
        }
        public static bool InRange(this int value, int min, int max, bool inclusiveMin = true, bool inclusiveMax = true)
        {
            if (inclusiveMin)
            {
                if (inclusiveMax)
                {
                    return value >= min && value <= max;
                }
                else
                {
                    return value >= min && value < max;
                }
            }
            else
            {
                if (inclusiveMax)
                {
                    return value > min && value <= max;
                }
                else
                {
                    return value > min && value < max;
                }
            }
        }


        // Get Angle In Forward
        public static float AngleOnForward(this Transform transform, Vector3 targetPosition)
        {
            return Vector3.SignedAngle(transform.forward, targetPosition - transform.position, transform.up);
        }
        public static float AngleOnForward(this Transform transform, Transform target)
        {
            return Vector3.SignedAngle(transform.forward, target.position - transform.position, transform.up);
        }
        public static float AngleOnForward(this Vector3 position, Quaternion rotation, Vector3 targetPoisition)
        {
            return Vector3.SignedAngle(rotation * Vector3.forward, position - targetPoisition, rotation * Vector3.up);
        }


        // Clamp Angle
        public static float ClampAngle(float value, float min, float max)
        {
            if (value < -360f)
                value += 360f;

            if (value > 360f)
                value -= 360f;

            return UnityEngine.Mathf.Clamp(value, min, max);
        }
    }
}
