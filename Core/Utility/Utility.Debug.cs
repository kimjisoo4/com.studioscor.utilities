using System.Collections;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;

using UnityEngine.Internal;

namespace StudioScor.Utilities
{

    public static partial class SUtility
    {
        public const string NAME_COLOR_RED = "red";
        public const string NAME_COLOR_YELLOW = "yellow";
        public const string NAME_COLOR_BLUE = "blue";
        public const string NAME_COLOR_GREEN = "green";
        public const string NAME_COLOR_GRAY = "gray";

        public static class Debug
        {
            public static bool UseDebug = true;

            static readonly Vector4[] s_NdcFrustum =
            {
            new Vector4(-1, 1,  -1, 1),
            new Vector4(1, 1,  -1, 1),
            new Vector4(1, -1, -1, 1),
            new Vector4(-1, -1, -1, 1),

            new Vector4(-1, 1,  1, 1),
            new Vector4(1, 1,  1, 1),
            new Vector4(1, -1, 1, 1),
            new Vector4(-1, -1, 1, 1)
        };

            // Cube with edge of length 1
            private static readonly Vector4[] s_UnitCube =
            {
            new Vector4(-0.5f,  0.5f, -0.5f, 1),
            new Vector4(0.5f,  0.5f, -0.5f, 1),
            new Vector4(0.5f, -0.5f, -0.5f, 1),
            new Vector4(-0.5f, -0.5f, -0.5f, 1),

            new Vector4(-0.5f,  0.5f,  0.5f, 1),
            new Vector4(0.5f,  0.5f,  0.5f, 1),
            new Vector4(0.5f, -0.5f,  0.5f, 1),
            new Vector4(-0.5f, -0.5f,  0.5f, 1)
        };

            // Sphere with radius of 1
            private static readonly Vector4[] s_UnitSphere = MakeUnitSphere(16);

            // Square with edge of length 1
            private static readonly Vector4[] s_UnitSquare =
            {
            new Vector4(-0.5f, 0.5f, 0, 1),
            new Vector4(0.5f, 0.5f, 0, 1),
            new Vector4(0.5f, -0.5f, 0, 1),
            new Vector4(-0.5f, -0.5f, 0, 1),
        };

            private static Vector4[] MakeUnitSphere(int len)
            {
                UnityEngine.Debug.Assert(len > 2);
                var v = new Vector4[len * 3];
                for (int i = 0; i < len; i++)
                {
                    var f = i / (float)len;
                    float c = Mathf.Cos(f * (float)(Mathf.PI * 2.0));
                    float s = Mathf.Sin(f * (float)(Mathf.PI * 2.0));
                    v[0 * len + i] = new Vector4(c, s, 0, 1);
                    v[1 * len + i] = new Vector4(0, c, s, 1);
                    v[2 * len + i] = new Vector4(s, 0, c, 1);
                }
                return v;
            }

            [Conditional("UNITY_EDITOR")]
            public static void Log(object message, Object context = null, string color = NAME_COLOR_GRAY)
            {
                if (!UseDebug)
                    return;

                UnityEngine.Debug.Log($"<color={color}>{ message}</color>", context);
            }

            [Conditional("UNITY_EDITOR")]
            public static void LogError(object message, Object context = null, string color = NAME_COLOR_GRAY)
            {
                UnityEngine.Debug.LogError($"<color={color}>{message}</color>", context);
            }

            [Conditional("UNITY_EDITOR")]
            public static void DrawSliceSphere(Vector3 position, Quaternion rotation, float radius, float horizontalAngle, float verticalAngle,Color color, float duration)
            {
                if (!UseDebug)
                    return;

                Vector3 endDistance = new Vector3(0, 0, radius);

                float halfVerticalAngle = verticalAngle * 0.5f;
                float halfHorizontalAngle = horizontalAngle * 0.5f;

                UnityEngine.Debug.DrawLine(position, position + rotation * Quaternion.Euler(0, -halfHorizontalAngle, 0) * endDistance, color, duration);
                UnityEngine.Debug.DrawLine(position, position + rotation * Quaternion.Euler(0, halfHorizontalAngle, 0) * endDistance, color, duration);
                  
                UnityEngine.Debug.DrawLine(position, position + rotation * Quaternion.Euler(-halfVerticalAngle, 0, 0) * endDistance, color, duration);
                UnityEngine.Debug.DrawLine(position, position + rotation * Quaternion.Euler(halfVerticalAngle, 0, 0) * endDistance, color, duration);

                UnityEngine.Debug.DrawLine(position, position +rotation * Quaternion.Euler(halfVerticalAngle, -halfHorizontalAngle, 0) * endDistance, color, duration);
                UnityEngine.Debug.DrawLine(position, position +rotation * Quaternion.Euler(halfVerticalAngle, halfHorizontalAngle, 0) * endDistance, color, duration);
                UnityEngine.Debug.DrawLine(position, position +rotation * Quaternion.Euler(halfVerticalAngle, 0, 0) * endDistance, color, duration);

                UnityEngine.Debug.DrawLine(position, position +rotation * Quaternion.Euler(-halfVerticalAngle, -halfHorizontalAngle, 0) * endDistance, color, duration);
                UnityEngine.Debug.DrawLine(position, position +rotation * Quaternion.Euler(-halfVerticalAngle, halfHorizontalAngle, 0) * endDistance, color, duration);
                UnityEngine.Debug.DrawLine(position, position + rotation * Quaternion.Euler(-halfVerticalAngle, 0, 0) * endDistance, color, duration);


                int horizontalCount = Mathf.RoundToInt(horizontalAngle / 10f);

                float horizontalTickAngle = horizontalAngle / horizontalCount;

                

                for (int i = 1; i <= horizontalCount; i++)
                {
                    float startAngle = (-halfHorizontalAngle) + (horizontalTickAngle * (i - 1));
                    float endAngle = (-halfHorizontalAngle) + (horizontalTickAngle * i);

                    for (int j = -1; j <= 1; j++)
                    {
                        Vector3 startPosition = position + rotation * Quaternion.Euler(verticalAngle * j * 0.5f, startAngle, 0) * endDistance;
                        Vector3 endPosition = position + rotation * Quaternion.Euler(verticalAngle * j * 0.5f, endAngle, 0) * endDistance;

                        UnityEngine.Debug.DrawLine(startPosition,endPosition,color, duration);
                    }
                }

                int verticalCount = Mathf.RoundToInt(verticalAngle / 10f);
                float verticalTickAngle = verticalAngle / verticalCount;

                for (int i = 1; i <= verticalCount; i++)
                {
                    float startAngle = (-halfVerticalAngle) + (verticalTickAngle * (i - 1));
                    float endAngle = (-halfVerticalAngle) + (verticalTickAngle * i);

                    for (int j = -1; j <= 1; j++)
                    {
                        Vector3 startPosition = position + rotation * Quaternion.Euler(startAngle, horizontalAngle * j * 0.5f, 0) * endDistance;
                        Vector3 endPosition = position + rotation * Quaternion.Euler(endAngle, horizontalAngle * j * 0.5f, 0) * endDistance;

                        UnityEngine.Debug.DrawLine(startPosition, endPosition, color, duration);
                    }
                }
            }

            [Conditional("UNITY_EDITOR")]
            public static void DrawCone(Vector3 position, Quaternion rotation, float distance, float angle, Color color, float duration = 0f)
            {
                if (!UseDebug)
                    return;

                Vector3 direction = new Vector3(0, 0, distance);
                float halfAngle = angle * 0.5f;

                UnityEngine.Debug.DrawLine(Vector3.zero, direction, color, duration);

                List<Vector3> lines = new List<Vector3>();

                // Line //
                for (int i = 0; i < 17; i++)
                {
                    lines.Add(rotation * (Quaternion.Euler(0, 0, 22.5f * i) * (Quaternion.Euler(halfAngle, 0, 0) * direction)));

                    UnityEngine.Debug.DrawLine(position + Vector3.zero, position + lines[i], color, duration);
                }

                for (int i = 0; i < lines.Count - 1; i++)
                {
                    UnityEngine.Debug.DrawLine(position + lines[i], position + lines[i + 1], color, duration);
                }

                // Arc //
                int count = Mathf.RoundToInt(angle / 10f);
                float tickAngle = angle / count;

                for (int i = 1; i <= count; i++)
                {
                    float startAngle = (tickAngle * (i - 1)) - halfAngle;
                    float endAngle = (tickAngle * i) - halfAngle;

                    for (int j = 0; j < 8; j++)
                    {
                        Vector3 startPosition = rotation * (Quaternion.Euler(0, 0, 22.5f * j) * Quaternion.Euler(0, startAngle, 0) * direction);
                        Vector3 endPosition = rotation * (Quaternion.Euler(0, 0, 22.5f * j) * Quaternion.Euler(0, endAngle, 0) * direction);

                        UnityEngine.Debug.DrawLine(position + startPosition, position + endPosition, color, duration);
                    }
                }

            }

            [Conditional("UNITY_EDITOR")]
            public static void DrawCone(Vector3 position, Vector3 direction, float distance, float angle, Color color, float duration = 0f)
            {
                if (!UseDebug)
                    return;

                DrawCone(position, Quaternion.LookRotation(direction), distance, angle, color, duration);
            }



            [Conditional("UNITY_EDITOR")]
            public static void DrawCapsule(Vector4 start, Vector4 end, float radius, Color color, float duration = 0f)
            {
                if (!UseDebug)
                    return;

                DrawSphere(start, radius, color, duration);

                if (start == end)
                {
                    return;
                }

                Vector3 startPos = start;
                Vector3 endPos = end;

                Quaternion rotation = Quaternion.LookRotation(startPos.Direction(endPos), Vector3.up);

                Vector3 offset = new();

                for (int i = -1; i <= 1; i += 2)
                {
                    offset.x = i;
                    offset.y = 0;
                    offset.z = 0;

                    offset = (rotation * offset) * radius;

                    UnityEngine.Debug.DrawLine(startPos + offset, endPos + offset, color, duration);
                }
                for (int i = -1; i <= 1; i += 2)
                {
                    offset.x = 0;
                    offset.y = i;
                    offset.z = 0;

                    offset = (rotation * offset) * radius;

                    UnityEngine.Debug.DrawLine(startPos + offset, endPos + offset, color, duration);
                }


                DrawSphere(end, radius, color, duration);
            }
            [Conditional("UNITY_EDITOR")]
            public static void DrawCapsule(Vector4 pos, float radius, Vector4 direction, float distance, Color color, float duration = 0f)
            {
                if (!UseDebug)
                    return;

                DrawSphere(pos, radius, color, duration);

                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

                Vector3 start = pos;
                Vector3 end = pos + direction * distance;

                Vector3 offset = new();

                for (int i = -1; i <= 1; i += 2)
                {
                    offset.x = i;
                    offset.y = 0;
                    offset.z = 0;

                    offset = rotation * offset;

                    UnityEngine.Debug.DrawLine(start + offset, end + offset, color, duration);
                }
                for (int i = -1; i <= 1; i += 2)
                {
                    offset.x = 0;
                    offset.y = i;
                    offset.z = 0;

                    offset = rotation * offset;

                    UnityEngine.Debug.DrawLine(start + offset, end + offset, color, duration);
                }

                DrawSphere(end, radius, color, duration);
            }



            [Conditional("UNITY_EDITOR")]
            public static void DrawSphere(Vector4 pos, float radius, Color color, float duration = 0f)
            {
                if (!UseDebug)
                    return;

                Vector4[] v = s_UnitSphere;
                int len = s_UnitSphere.Length / 3;
                for (int i = 0; i < len; i++)
                {
                    var sX = pos + radius * v[0 * len + i];
                    var eX = pos + radius * v[0 * len + (i + 1) % len];
                    var sY = pos + radius * v[1 * len + i];
                    var eY = pos + radius * v[1 * len + (i + 1) % len];
                    var sZ = pos + radius * v[2 * len + i];
                    var eZ = pos + radius * v[2 * len + (i + 1) % len];

                    UnityEngine.Debug.DrawLine(sX, eX, color, duration);
                    UnityEngine.Debug.DrawLine(sY, eY, color, duration);
                    UnityEngine.Debug.DrawLine(sZ, eZ, color, duration);
                }
            }

            [Conditional("UNITY_EDITOR")]
            public static void DrawPoint(Vector4 pos, float scale, Color color, float duration = 0f)
            {
                if (!UseDebug)
                    return;

                var sX = pos + new Vector4(+scale, 0, 0);
                var eX = pos + new Vector4(-scale, 0, 0);
                var sY = pos + new Vector4(0, +scale, 0);
                var eY = pos + new Vector4(0, -scale, 0);
                var sZ = pos + new Vector4(0, 0, +scale);
                var eZ = pos + new Vector4(0, 0, -scale);

                UnityEngine.Debug.DrawLine(sX, eX, color);
                UnityEngine.Debug.DrawLine(sY, eY, color);
                UnityEngine.Debug.DrawLine(sZ, eZ, color);
            }
        }
    }
    

}
