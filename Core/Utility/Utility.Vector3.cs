using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class Utility
    {
        // Distance XZ
        public static float HorizontalDistance(this Vector3 from, Vector3 to)
        {
            from.y = 0;
            to.y = 0;

            return Vector3.Distance(from, to);
        }
        public static float HorizontaDistance(this Vector3 from, Transform to)
        {
            return HorizontalDistance(from, to.position);
        }
        public static float HorizontaDistance(this Transform from, Vector3 to)
        {
            return HorizontalDistance(from.position, to);
        }
        public static float HorizontalDistance(this Transform from, Transform to)
        {
            return HorizontalDistance(from.position, to.position);
        }


        // Turn Direction From Y
        public static Vector3 TurnDirectionFromY(Vector2 direction, Transform target)
        {
            Vector3 forward = target.forward;
            Vector3 right = target.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            return forward * direction.y + right * direction.x;
        }
        public static Vector3 TurnDirectionFromY(this Vector3 direction, Transform target)
        {
            Vector3 forward = target.forward;
            Vector3 right = target.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            return forward * direction.z + right * direction.x;
        }

        // Direction
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
    }
}
