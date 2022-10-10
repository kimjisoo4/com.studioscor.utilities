using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Internal;


namespace KimScor.Utilities
{

    public static partial class Utilities
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
        public static void Log(object message, Object context = null)
        {
            if(UseDebug)
                UnityEngine.Debug.Log(message, context);
        }

        public static void DrawCapsule(Vector4 start, Vector4 end, float radius, Color color, float duration = 0f)
        {
            if (!UseDebug)
                return;

            DrawSphere(start, radius, color, duration);

            Vector3 startPos = start;
            Vector3 endPos = end;

            Quaternion rotation = Quaternion.LookRotation(startPos.Direction(endPos), Vector3.up);

            Vector3 offset = new();

            for (int i = -1; i <= 1; i += 2)
            {
                offset.x = i;
                offset.y = 0;
                offset.z = 0;

                offset = rotation * offset;

                UnityEngine.Debug.DrawLine(startPos + offset, endPos + offset, color, duration);
            }
            for (int i = -1; i <= 1; i += 2)
            {
                offset.x = 0;
                offset.y = i;
                offset.z = 0;

                offset = rotation * offset;

                UnityEngine.Debug.DrawLine(startPos + offset, endPos + offset, color, duration);
            }


            DrawSphere(end, radius, color, duration);
        }
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
