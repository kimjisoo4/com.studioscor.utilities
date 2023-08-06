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
            public static void DrawRay(Vector3 start, Vector3 direction, Color color = default, float duration = 0f, bool depthTest = true)
            {
                color = (color == default) ? Color.white : color;

                UnityEngine.Debug.DrawRay(start, direction, color, duration, depthTest);
            }
            [Conditional("UNITY_EDITOR")]
            public static void DrawLine(Vector3 start, Vector3 end, Color color = default, float duration = 0f, bool depthTest = true)
            {
                color = (color == default) ? Color.white : color;

                UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
            }


            [Conditional("UNITY_EDITOR")]
            public static void DebugPoint(Vector3 position, float scale = 1.0f, Color color = default, float duration = 0, bool depthTest = true)
            {
                color = (color == default(Color)) ? Color.white : color;

                
                DrawRay(position + (Vector3.up * (scale * 0.5f)), -Vector3.up * scale, color, duration, depthTest);
                
                DrawRay(position + (Vector3.right * (scale * 0.5f)), -Vector3.right * scale, color, duration, depthTest);
                
                DrawRay(position + (Vector3.forward * (scale * 0.5f)), -Vector3.forward * scale, color, duration, depthTest);
            }


            [Conditional("UNITY_EDITOR")]
            public static void DebugBounds(Bounds bounds, Color color = default, float duration = 0, bool depthTest = true)
            {
                Vector3 center = bounds.center;

                float x = bounds.extents.x;
                float y = bounds.extents.y;
                float z = bounds.extents.z;

                Vector3 ruf = center + new Vector3(x, y, z);
                Vector3 rub = center + new Vector3(x, y, -z);
                Vector3 luf = center + new Vector3(-x, y, z);
                Vector3 lub = center + new Vector3(-x, y, -z);

                Vector3 rdf = center + new Vector3(x, -y, z);
                Vector3 rdb = center + new Vector3(x, -y, -z);
                Vector3 lfd = center + new Vector3(-x, -y, z);
                Vector3 lbd = center + new Vector3(-x, -y, -z);

                DrawLine(ruf, luf, color, duration, depthTest);
                DrawLine(ruf, rub, color, duration, depthTest);
                DrawLine(luf, lub, color, duration, depthTest);
                DrawLine(rub, lub, color, duration, depthTest);

                DrawLine(ruf, rdf, color, duration, depthTest);
                DrawLine(rub, rdb, color, duration, depthTest);
                DrawLine(luf, lfd, color, duration, depthTest);
                DrawLine(lub, lbd, color, duration, depthTest);

                DrawLine(rdf, lfd, color, duration, depthTest);
                DrawLine(rdf, rdb, color, duration, depthTest);
                DrawLine(lfd, lbd, color, duration, depthTest);
                DrawLine(lbd, rdb, color, duration, depthTest);
            }

            [Conditional("UNITY_EDITOR")]
            public static void DebugLocalCube(Transform transform, Vector3 size, Vector3 center = default, Color color = default, float duration = 0, bool depthTest = true)
            {
                Vector3 lbb = transform.TransformPoint(center + ((-size) * 0.5f));
                Vector3 rbb = transform.TransformPoint(center + (new Vector3(size.x, -size.y, -size.z) * 0.5f));

                Vector3 lbf = transform.TransformPoint(center + (new Vector3(size.x, -size.y, size.z) * 0.5f));
                Vector3 rbf = transform.TransformPoint(center + (new Vector3(-size.x, -size.y, size.z) * 0.5f));

                Vector3 lub = transform.TransformPoint(center + (new Vector3(-size.x, size.y, -size.z) * 0.5f));
                Vector3 rub = transform.TransformPoint(center + (new Vector3(size.x, size.y, -size.z) * 0.5f));

                Vector3 luf = transform.TransformPoint(center + ((size) * 0.5f));
                Vector3 ruf = transform.TransformPoint(center + (new Vector3(-size.x, size.y, size.z) * 0.5f));

                DrawLine(lbb, rbb, color, duration, depthTest);
                DrawLine(rbb, lbf, color, duration, depthTest);
                DrawLine(lbf, rbf, color, duration, depthTest);
                DrawLine(rbf, lbb, color, duration, depthTest);

                DrawLine(lub, rub, color, duration, depthTest);
                DrawLine(rub, luf, color, duration, depthTest);
                DrawLine(luf, ruf, color, duration, depthTest);
                DrawLine(ruf, lub, color, duration, depthTest);

                DrawLine(lbb, lub, color, duration, depthTest);
                DrawLine(rbb, rub, color, duration, depthTest);
                DrawLine(lbf, luf, color, duration, depthTest);
                DrawLine(rbf, ruf, color, duration, depthTest);
            }

            [Conditional("UNITY_EDITOR")]
            public static void DebugCircle(Vector3 position, Vector3 upAxis, float radius, Color color = default, float duration = 0, bool depthTest = true)
            {
                Vector3 up = upAxis.normalized * radius;
                Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                Vector3 right = Vector3.Cross(up, forward).normalized * radius;

                Matrix4x4 matrix = new Matrix4x4();

                matrix[0] = right.x;
                matrix[1] = right.y;
                matrix[2] = right.z;

                matrix[4] = up.x;
                matrix[5] = up.y;
                matrix[6] = up.z;

                matrix[8] = forward.x;
                matrix[9] = forward.y;
                matrix[10] = forward.z;

                Vector3 lastPoint = position + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)));
                Vector3 nextPoint = Vector3.zero;

                color = (color == default) ? Color.white : color;

                for (var i = 0; i < 91; i++)
                {
                    nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
                    nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
                    nextPoint.y = 0;

                    nextPoint = position + matrix.MultiplyPoint3x4(nextPoint);

                    DrawLine(lastPoint, nextPoint, color, duration, depthTest);
                    lastPoint = nextPoint;
                }
            }

            [Conditional("UNITY_EDITOR")]
            public static void DebugWireSphere(Vector3 position, float radius = 1.0f, Color color = default, float duration = 0, bool depthTest = true)
            {
                float angle = 10.0f;

                Vector3 x = new Vector3(position.x, position.y + radius * Mathf.Sin(0), position.z + radius * Mathf.Cos(0));
                Vector3 y = new Vector3(position.x + radius * Mathf.Cos(0), position.y, position.z + radius * Mathf.Sin(0));
                Vector3 z = new Vector3(position.x + radius * Mathf.Cos(0), position.y + radius * Mathf.Sin(0), position.z);

                Vector3 newX;
                Vector3 newY;
                Vector3 newZ;

                for (int i = 1; i < 37; i++)
                {

                    newX = new Vector3(position.x, position.y + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad), position.z + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad));
                    newY = new Vector3(position.x + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad), position.y, position.z + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad));
                    newZ = new Vector3(position.x + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad), position.y + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad), position.z);

                    DrawLine(x, newX, color, duration, depthTest);
                    DrawLine(y, newY, color, duration, depthTest);
                    DrawLine(z, newZ, color, duration, depthTest);

                    x = newX;
                    y = newY;
                    z = newZ;
                }
            }

            [Conditional("UNITY_EDITOR")]
            public static void DebugCylinder(Vector3 start, Vector3 end, float radius, Color color = default, float duration = 0, bool depthTest = true)
            {
                Vector3 up = (end - start).normalized * radius;
                Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                Vector3 right = Vector3.Cross(up, forward).normalized * radius;

                //Radial circles
                DebugCircle(start, up, radius, color, duration, depthTest);
                DebugCircle(end, -up, radius, color, duration, depthTest);
                DebugCircle((start + end) * 0.5f, up, radius, color, duration, depthTest);

                //Side lines
                DrawLine(start + right, end + right, color, duration, depthTest);
                DrawLine(start - right, end - right, color, duration, depthTest);

                DrawLine(start + forward, end + forward, color, duration, depthTest);
                DrawLine(start - forward, end - forward, color, duration, depthTest);

                //Start endcap
                DrawLine(start - right, start + right, color, duration, depthTest);
                DrawLine(start - forward, start + forward, color, duration, depthTest);

                //End endcap
                DrawLine(end - right, end + right, color, duration, depthTest);
                DrawLine(end - forward, end + forward, color, duration, depthTest);
            }

            [Conditional("UNITY_EDITOR")]
            public static void DebugCone(Vector3 position, Vector3 direction, float angle, Color color = default, float duration = 0, bool depthTest = true)
            {
                float length = direction.magnitude;

                Vector3 forward = direction;
                Vector3 up = Vector3.Slerp(forward, -forward, 0.5f);
                Vector3 right = Vector3.Cross(forward, up).normalized * length;

                direction = direction.normalized;

                Vector3 slerpedVector = Vector3.Slerp(forward, up, angle / 90.0f);

                float dist;
                var farPlane = new Plane(-direction, position + forward);
                var distRay = new Ray(position, slerpedVector);

                farPlane.Raycast(distRay, out dist);

                DrawRay(position, slerpedVector.normalized * dist, color);
                DrawRay(position, Vector3.Slerp(forward, -up, angle / 90.0f).normalized * dist, color, duration, depthTest);
                DrawRay(position, Vector3.Slerp(forward, right, angle / 90.0f).normalized * dist, color, duration, depthTest);
                DrawRay(position, Vector3.Slerp(forward, -right, angle / 90.0f).normalized * dist, color, duration, depthTest);

                DebugCircle(position + forward, direction,(forward - (slerpedVector.normalized * dist)).magnitude, color, duration, depthTest);
                DebugCircle(position + (forward * 0.5f), direction, ((forward * 0.5f) - (slerpedVector.normalized * (dist * 0.5f))).magnitude, color, duration, depthTest);
            }

            [Conditional("UNITY_EDITOR")]
            public static void DebugArrow(Vector3 position, Vector3 direction, Color color = default, float duration = 0, bool depthTest = true)
            {
                DrawRay(position, direction, color, duration, depthTest);
                DebugCone(position + direction, -direction * 0.333f, 15, color, duration, depthTest);
            }

            [Conditional("UNITY_EDITOR")]
            public static void DebugCapsule(Vector3 start, Vector3 end, float radius, Color color = default, float duration = 0, bool depthTest = true)
            {
                Vector3 up = (end - start).normalized * radius;
                Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                Vector3 right = Vector3.Cross(up, forward).normalized * radius;

                float height = (start - end).magnitude;
                float sideLength = Mathf.Max(0, (height * 0.5f) - radius);
                Vector3 middle = (end + start) * 0.5f;

                start = middle + ((start - middle).normalized * sideLength);
                end = middle + ((end - middle).normalized * sideLength);

                //Radial circles
                DebugCircle(start, up, radius, color, duration, depthTest);
                DebugCircle(end, -up, radius, color,  duration, depthTest);

                //Side lines
                DrawLine(start + right, end + right, color, duration, depthTest);
                DrawLine(start - right, end - right, color, duration, depthTest);

                DrawLine(start + forward, end + forward, color, duration, depthTest);
                DrawLine(start - forward, end - forward, color, duration, depthTest);

                for (int i = 1; i < 26; i++)
                {
                    //Start endcap
                    DrawLine(Vector3.Slerp(right, -up, i / 25.0f) + start, Vector3.Slerp(right, -up, (i - 1) / 25.0f) + start, color, duration, depthTest);
                    DrawLine(Vector3.Slerp(-right, -up, i / 25.0f) + start, Vector3.Slerp(-right, -up, (i - 1) / 25.0f) + start, color, duration, depthTest);
                    DrawLine(Vector3.Slerp(forward, -up, i / 25.0f) + start, Vector3.Slerp(forward, -up, (i - 1) / 25.0f) + start, color, duration, depthTest);
                    DrawLine(Vector3.Slerp(-forward, -up, i / 25.0f) + start, Vector3.Slerp(-forward, -up, (i - 1) / 25.0f) + start, color, duration, depthTest);

                    //End endcap
                    DrawLine(Vector3.Slerp(right, up, i / 25.0f) + end, Vector3.Slerp(right, up, (i - 1) / 25.0f) + end, color, duration, depthTest);
                    DrawLine(Vector3.Slerp(-right, up, i / 25.0f) + end, Vector3.Slerp(-right, up, (i - 1) / 25.0f) + end, color, duration, depthTest);
                    DrawLine(Vector3.Slerp(forward, up, i / 25.0f) + end, Vector3.Slerp(forward, up, (i - 1) / 25.0f) + end, color, duration, depthTest);
                    DrawLine(Vector3.Slerp(-forward, up, i / 25.0f) + end, Vector3.Slerp(-forward, up, (i - 1) / 25.0f) + end, color, duration, depthTest);
                }
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
            public static void DrawPoint(Vector4 pos, Color color, float scale = 1f, float duration = 1f)
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
            [Conditional("UNITY_EDITOR")]
            public static void DrawPoint(Vector4 pos, float scale = 1f, float duration = 1f)
            {
                if (!UseDebug)
                    return;

                var sX = pos + new Vector4(+scale, 0, 0);
                var eX = pos + new Vector4(-scale, 0, 0);
                var sY = pos + new Vector4(0, +scale, 0);
                var eY = pos + new Vector4(0, -scale, 0);
                var sZ = pos + new Vector4(0, 0, +scale);
                var eZ = pos + new Vector4(0, 0, -scale);

                UnityEngine.Debug.DrawLine(sX, eX, Color.red);
                UnityEngine.Debug.DrawLine(sY, eY, Color.red);
                UnityEngine.Debug.DrawLine(sZ, eZ, Color.red);
            }
        }
    }
    

}
