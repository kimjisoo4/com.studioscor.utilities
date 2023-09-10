using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        public static bool SafeEquals(this Vector3 lhs, Vector3 rhs, float equaly = 0.0001f)
        {
            return Vector3.SqrMagnitude(lhs - rhs) < equaly;
        }

        public static bool SafeEqauls(this Vector2 lhs, Vector2 rhs, float equaly = 0.0001f)
        {
            return Vector2.SqrMagnitude(lhs - rhs) < equaly;
        }

        // horizontal Magnitude

        public static float HorizontalMagnitude(this Vector3 lhs)
        {
            lhs.y = 0;

            return lhs.magnitude;
        }

        // Sqr Distance
        public static float SqrDistance(this Vector3 lhs, Vector3 rhs)
        {
            return (rhs - lhs).sqrMagnitude;
        }
        public static float SqrDistance(this Transform lhs, Transform rhs)
        {
            return SqrDistance(lhs.position, rhs.position);
        }
        public static float SqrDistance(this Transform lhs, Vector3 rhs)
        {
            return SqrDistance(lhs.position, rhs);
        }
        public static float SqrDistance(this Vector3 lhs, Transform rhs)
        {
            return SqrDistance(lhs, rhs.position);
        }


        // Distance XZ
        public static float HorizontalDistance(this Vector3 from, Vector3 to)
        {
            from.y = 0;
            to.y = 0;

            return Vector3.Distance(from, to);
        }
        public static float HorizontalDistance(this Vector3 from, Transform to)
        {
            return HorizontalDistance(from, to.position);
        }
        public static float HorizontalDistance(this Transform from, Vector3 to)
        {
            return HorizontalDistance(from.position, to);
        }
        public static float HorizontalDistance(this Transform from, Transform to)
        {
            return HorizontalDistance(from.position, to.position);
        }


        // Turn Direction From Y
        public static Vector3 TurnDirectionFromY(this Vector2 direction, Transform target)
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
            return normalized ? (target - start).normalized : target - start;
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

        // horizontal Direction
        public static Vector3 HorizontalDirection(this Vector3 start, Vector3 target, bool normalized = true)
        {
            Vector3 direction = target - start;

            direction.y = 0;

            return normalized ? direction.normalized : direction;
        }
        public static Vector3 HorizontalDirection(this Transform lhs, Transform rhs, bool normalized = true)
        {
            return HorizontalDirection(lhs.position, rhs.position, normalized);
        }
        public static Vector3 HorizontalDirection(this Vector3 lhs, Transform rhs, bool normalized = true)
        {
            return HorizontalDirection(lhs, rhs.position, normalized);
        }
        public static Vector3 HorizontalDirection(this Transform lhs, Vector3 rhs, bool normalized = true)
        {
            return HorizontalDirection(lhs.position, rhs, normalized);
        }

        public static Vector3 HorizontalForward(this Transform lhs)
        {
            Vector3 forward = lhs.forward;

            forward.y = 0;

            return forward.normalized;
        }
        public static Vector3 HorizontalRight(this Transform lhs)
        {
            Vector3 forward = lhs.right;

            forward.y = 0;

            return forward.normalized;
        }



        // Align
        public static Vector3[] AlignToline(Vector3 position, Quaternion rotation, float space, int count, EAlign align)
        {
            Vector3[] positions = new Vector3[count];

            float width = space * (count - 1);

            switch (align)
            {
                case EAlign.Center:
                    width *= -0.5f;
                    break;
                case EAlign.Left:
                    width = 0f;
                    break;
                case EAlign.Right:
                    width = -width;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = new Vector3(width + (space * i), 0, 0);
                pos = rotation * pos;

                positions[i] = position + pos;
            }

            return positions;
        }

        public static Vector3[] AlignToCircle(Vector3 position, Quaternion rotation, float radius, float angle, int count, EAlign align, EAxis axis)
        {
            Vector3[] positions = new Vector3[count];

            float interval;

            if (angle < 360)
            {
                interval = angle / (count - 1);
            }
            else
            {
                interval = angle / count;
            }

            Vector3 pos = Vector3.zero;
            Vector3 eulurAngle = Vector3.zero;

            switch (align)
            {
                case EAlign.Center:
                    angle *= -0.5f;
                    break;
                case EAlign.Left:
                    angle = -angle;
                    break;
                case EAlign.Right:
                    angle = 0;
                    break;
                default:
                    break;
            }

            switch (axis)
            {
                case EAxis.X:
                    pos.y = radius;
                    eulurAngle.x = 1f;
                    break;
                case EAxis.Y:
                    pos.z = radius;
                    eulurAngle.y = 1f;
                    break;
                case EAxis.Z:
                    pos.y = radius;
                    eulurAngle.z = 1f;
                    break;
                default:
                    break;
            }

            for(int i = 0; i < count; i++)
            {
                Vector3 alingPos = rotation * Quaternion.Euler(eulurAngle * (angle + interval * i)) * pos;

                positions[i] = position + alingPos;
            }

            return positions;
        }
    }
}
