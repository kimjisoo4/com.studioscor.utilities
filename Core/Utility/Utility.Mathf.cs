using Codice.Client.BaseCommands;
using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        public static bool IsZero(this float lhs, float tolerance = 0.001f)
        {
            return lhs.IsPositive() ? lhs <= tolerance : lhs >= -tolerance;
        }
        public static bool SafeEquals(this float lhs, int rhs, float tolerance = 0.001f)
        {
            float value = lhs - rhs;

            return value.IsPositive() ? value <= tolerance : value >= -tolerance;
        }
        public static bool SafeEquals(this int lhs, float rhs, float tolerance = 0.001f)
        {
            float value = lhs - rhs;

            return value.IsPositive() ? value <= tolerance : value >= -tolerance;
        }
        public static bool SafeEquals(this float lhs, float rhs, float tolerance = 0.001f)
        {
            float value = lhs - rhs;

            return value.IsPositive()? value <= tolerance : value >= -tolerance;
        }

        // Safe Divide
        public static Vector3 SafeDivide(this Vector3 lhs, float rhs)
        {
            if (rhs == 0)
                return lhs;

            return lhs / rhs;
        }

        public static float SafeDivide(this float lhs, float rhs)
        {
            if (rhs == 0)
                return -1f;

            return lhs / rhs;
        }
        public static float SafeDivide(this int lhs, float rhs)
        {
            return SafeDivide((float)lhs, rhs);
        }
        public static float SafeDivide(this float lhs, int rhs)
        {
            return SafeDivide(lhs, (float)rhs);
        }
        public static float SafeDivide(this int lhs, int rhs)
        {
            return SafeDivide((float)lhs, (float)rhs);
        }

        // Set Negative
        public static float Inverse(this ref float value)
        {
            return -value;
        }
        public static float Positive(this ref float value)
        {
            return value > 0 ? value : -value;
        }
        public static float Negative(this ref float value)
        {
            return value < 0 ? value : -value;
        }

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

        public static bool InRange(this float value, Vector2 range, bool inclusiveMin = true, bool inclusiveMax = true)
        {
            if (inclusiveMin)
            {
                if (inclusiveMax)
                {
                    return value >= range.x && value <= range.y;
                }
                else
                {
                    return value >= range.x && value < range.y;
                }
            }
            else
            {
                if (inclusiveMax)
                {
                    return value > range.x && value <= range.y;
                }
                else
                {
                    return value > range.x && value < range.y;
                }
            }
        }
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

        public static bool InRangeAngle(this float angle, float range, bool inclusive = true)
        {
            if(inclusive)
                return -range <= angle && angle <= range;
            else
                return -range < angle && angle < range;
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
