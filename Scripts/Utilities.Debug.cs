using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KimScor.Utilities
{
    public static partial class Utilities
    {
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
            UnityEngine.Debug.Log(message, context);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawSphere(Vector4 pos, float radius, Color color)
        {
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

                UnityEngine.Debug.DrawLine(sX, eX, color);
                UnityEngine.Debug.DrawLine(sY, eY, color);
                UnityEngine.Debug.DrawLine(sZ, eZ, color);
            }
        }
    }
    

}
