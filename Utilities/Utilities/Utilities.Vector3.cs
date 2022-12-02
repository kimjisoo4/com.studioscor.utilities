using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class Utility
    {
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
    }
    public static partial class Utility
    {
        #region Vector3.Direction
        public static Vector3 Direction(this Vector3 start, Vector3 target, bool normalized = true)
        {
            return normalized? (target - start).normalized : target - start;
        }
        public static Vector3 Direction(this Transform start, Vector3 target, bool normalized = true)
        {
            return start.position.Direction(target, normalized);
        }
        public static Vector3 Direction(this Transform start, Transform target, bool normalized = true)
        {
            return start.Direction(target.position, normalized);
        }
        public static Vector3 Direction(this Vector3 start, Transform target, bool normalized = true)
        {
            return start.Direction(target.position, normalized);
        }
        #endregion
    }
}
