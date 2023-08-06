using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        public class SGizmos
        {
            public static void DrawPoint(Vector3 position, float scale, Color color = default)
            {
                color = color == default ? Gizmos.color : color;

                Color oldColor = Gizmos.color;

                Gizmos.color = color;
                Gizmos.DrawRay(position + (Vector3.up * (scale * 0.5f)), -Vector3.up * scale);
                Gizmos.DrawRay(position + (Vector3.right * (scale * 0.5f)), -Vector3.right * scale);
                Gizmos.DrawRay(position + (Vector3.forward * (scale * 0.5f)), -Vector3.forward * scale);

                Gizmos.color = oldColor;
            }

            public static void DrawBounds(Bounds bounds, Color color = default)
            {
                color = color == default ? Gizmos.color : color;

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

                Color oldColor = Gizmos.color;
                Gizmos.color = color;

                Gizmos.DrawLine(ruf, luf);
                Gizmos.DrawLine(ruf, rub);
                Gizmos.DrawLine(luf, lub);
                Gizmos.DrawLine(rub, lub);

                Gizmos.DrawLine(ruf, rdf);
                Gizmos.DrawLine(rub, rdb);
                Gizmos.DrawLine(luf, lfd);
                Gizmos.DrawLine(lub, lbd);

                Gizmos.DrawLine(rdf, lfd);
                Gizmos.DrawLine(rdf, rdb);
                Gizmos.DrawLine(lfd, lbd);
                Gizmos.DrawLine(lbd, rdb);

                Gizmos.color = oldColor;
            }

            public static void DrawLocalCube(Transform transform, Vector3 size, Vector3 center = default, Color color = default)
            {
                color = color == default ? Gizmos.color : color;

                Color oldColor = Gizmos.color;
                Gizmos.color = color;

                Vector3 lbb = transform.TransformPoint(center + ((-size) * 0.5f));
                Vector3 rbb = transform.TransformPoint(center + (new Vector3(size.x, -size.y, -size.z) * 0.5f));

                Vector3 lbf = transform.TransformPoint(center + (new Vector3(size.x, -size.y, size.z) * 0.5f));
                Vector3 rbf = transform.TransformPoint(center + (new Vector3(-size.x, -size.y, size.z) * 0.5f));

                Vector3 lub = transform.TransformPoint(center + (new Vector3(-size.x, size.y, -size.z) * 0.5f));
                Vector3 rub = transform.TransformPoint(center + (new Vector3(size.x, size.y, -size.z) * 0.5f));

                Vector3 luf = transform.TransformPoint(center + ((size) * 0.5f));
                Vector3 ruf = transform.TransformPoint(center + (new Vector3(-size.x, size.y, size.z) * 0.5f));

                Gizmos.DrawLine(lbb, rbb);
                Gizmos.DrawLine(rbb, lbf);
                Gizmos.DrawLine(lbf, rbf);
                Gizmos.DrawLine(rbf, lbb);

                Gizmos.DrawLine(lub, rub);
                Gizmos.DrawLine(rub, luf);
                Gizmos.DrawLine(luf, ruf);
                Gizmos.DrawLine(ruf, lub);

                Gizmos.DrawLine(lbb, lub);
                Gizmos.DrawLine(rbb, rub);
                Gizmos.DrawLine(lbf, luf);
                Gizmos.DrawLine(rbf, ruf);

                Gizmos.color = oldColor;
            }

            public static void DrawLocalCube(Matrix4x4 space, Vector3 size, Vector3 center = default, Color color = default)
            {
                color = color == default ? Gizmos.color : color;

                Color oldColor = Gizmos.color;
                Gizmos.color = color;

                Vector3 lbb = space.MultiplyPoint3x4(center + ((-size) * 0.5f));
                Vector3 rbb = space.MultiplyPoint3x4(center + (new Vector3(size.x, -size.y, -size.z) * 0.5f));

                Vector3 lbf = space.MultiplyPoint3x4(center + (new Vector3(size.x, -size.y, size.z) * 0.5f));
                Vector3 rbf = space.MultiplyPoint3x4(center + (new Vector3(-size.x, -size.y, size.z) * 0.5f));

                Vector3 lub = space.MultiplyPoint3x4(center + (new Vector3(-size.x, size.y, -size.z) * 0.5f));
                Vector3 rub = space.MultiplyPoint3x4(center + (new Vector3(size.x, size.y, -size.z) * 0.5f));

                Vector3 luf = space.MultiplyPoint3x4(center + ((size) * 0.5f));
                Vector3 ruf = space.MultiplyPoint3x4(center + (new Vector3(-size.x, size.y, size.z) * 0.5f));

                Gizmos.DrawLine(lbb, rbb);
                Gizmos.DrawLine(rbb, lbf);
                Gizmos.DrawLine(lbf, rbf);
                Gizmos.DrawLine(rbf, lbb);

                Gizmos.DrawLine(lub, rub);
                Gizmos.DrawLine(rub, luf);
                Gizmos.DrawLine(luf, ruf);
                Gizmos.DrawLine(ruf, lub);

                Gizmos.DrawLine(lbb, lub);
                Gizmos.DrawLine(rbb, rub);
                Gizmos.DrawLine(lbf, luf);
                Gizmos.DrawLine(rbf, ruf);

                Gizmos.color = oldColor;
            }

            public static void DrawCircle(Vector3 position, Vector3 up, float radius, Color color = default)
            {
                color = color == default ? Gizmos.color : color;

                up = ((up == Vector3.zero) ? Vector3.up : up).normalized * radius;
                Vector3 _forward = Vector3.Slerp(up, -up, 0.5f);
                Vector3 _right = Vector3.Cross(up, _forward).normalized * radius;

                Matrix4x4 matrix = new Matrix4x4();

                matrix[0] = _right.x;
                matrix[1] = _right.y;
                matrix[2] = _right.z;

                matrix[4] = up.x;
                matrix[5] = up.y;
                matrix[6] = up.z;

                matrix[8] = _forward.x;
                matrix[9] = _forward.y;
                matrix[10] = _forward.z;

                Vector3 _lastPoint = position + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)));
                Vector3 _nextPoint = Vector3.zero;

                Color oldColor = Gizmos.color;
                Gizmos.color = (color == default(Color)) ? Color.white : color;

                for (var i = 0; i < 91; i++)
                {
                    _nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
                    _nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
                    _nextPoint.y = 0;

                    _nextPoint = position + matrix.MultiplyPoint3x4(_nextPoint);

                    Gizmos.DrawLine(_lastPoint, _nextPoint);
                    _lastPoint = _nextPoint;
                }

                Gizmos.color = oldColor;
            }

            public static void DrawCylinder(Vector3 start, Vector3 end, float radius, Color color = default)
            {
                color = color == default ? Gizmos.color : color;

                Vector3 up = (end - start).normalized * radius;
                Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                Vector3 right = Vector3.Cross(up, forward).normalized * radius;

                //Radial circles
                DrawCircle(start, up, radius, color);
                DrawCircle(end, -up, radius, color);
                DrawCircle((start + end) * 0.5f, up, radius, color);

                Color oldColor = Gizmos.color;
                Gizmos.color = color;

                //Side lines
                Gizmos.DrawLine(start + right, end + right);
                Gizmos.DrawLine(start - right, end - right);

                Gizmos.DrawLine(start + forward, end + forward);
                Gizmos.DrawLine(start - forward, end - forward);

                //Start endcap
                Gizmos.DrawLine(start - right, start + right);
                Gizmos.DrawLine(start - forward, start + forward);

                //End endcap
                Gizmos.DrawLine(end - right, end + right);
                Gizmos.DrawLine(end - forward, end + forward);

                Gizmos.color = oldColor;
            }
            public static void DrawCone(Vector3 position, Vector3 direction, float angle, Color color = default)
            {
                color = color == default ? Gizmos.color : color;

                float length = direction.magnitude;

                Vector3 _forward = direction;
                Vector3 _up = Vector3.Slerp(_forward, -_forward, 0.5f);
                Vector3 _right = Vector3.Cross(_forward, _up).normalized * length;

                direction = direction.normalized;

                Vector3 slerpedVector = Vector3.Slerp(_forward, _up, angle / 90.0f);

                float dist;
                var farPlane = new Plane(-direction, position + _forward);
                var distRay = new Ray(position, slerpedVector);

                farPlane.Raycast(distRay, out dist);

                Color oldColor = Gizmos.color;
                Gizmos.color = color;

                Gizmos.DrawRay(position, slerpedVector.normalized * dist);
                Gizmos.DrawRay(position, Vector3.Slerp(_forward, -_up, angle / 90.0f).normalized * dist);
                Gizmos.DrawRay(position, Vector3.Slerp(_forward, _right, angle / 90.0f).normalized * dist);
                Gizmos.DrawRay(position, Vector3.Slerp(_forward, -_right, angle / 90.0f).normalized * dist);

                DrawCircle(position + _forward, direction, (_forward - (slerpedVector.normalized * dist)).magnitude, color);
                DrawCircle(position + (_forward * 0.5f), direction, ((_forward * 0.5f) - (slerpedVector.normalized * (dist * 0.5f))).magnitude, color);

                Gizmos.color = oldColor;
            }

            public static void DrawArrow(Vector3 position, Vector3 direction, Color color = default)
            {
                color = color == default ? Gizmos.color : color;

                Color oldColor = Gizmos.color;
                Gizmos.color = color;

                Gizmos.DrawRay(position, direction);
                DrawCone(position + direction, -direction * 0.333f, 15, color);

                Gizmos.color = oldColor;
            }

            public static void DrawCapsule(Vector3 start, Vector3 end, float radius, Color color = default)
            {
                color = color == default ? Gizmos.color : color;

                Vector3 up = (end - start).normalized * radius;
                Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                Vector3 right = Vector3.Cross(up, forward).normalized * radius;

                Color oldColor = Gizmos.color;
                Gizmos.color = color;

                float height = (start - end).magnitude;
                float sideLength = Mathf.Max(0, (height * 0.5f) - radius);
                Vector3 middle = (end + start) * 0.5f;

                start = middle + ((start - middle).normalized * sideLength);
                end = middle + ((end - middle).normalized * sideLength);

                //Radial circles
                DrawCircle(start, up, radius, color);
                DrawCircle(end, -up, radius, color);

                //Side lines
                Gizmos.DrawLine(start + right, end + right);
                Gizmos.DrawLine(start - right, end - right);

                Gizmos.DrawLine(start + forward, end + forward);
                Gizmos.DrawLine(start - forward, end - forward);

                for (int i = 1; i < 26; i++)
                {

                    //Start endcap
                    Gizmos.DrawLine(Vector3.Slerp(right, -up, i / 25.0f) + start, Vector3.Slerp(right, -up, (i - 1) / 25.0f) + start);
                    Gizmos.DrawLine(Vector3.Slerp(-right, -up, i / 25.0f) + start, Vector3.Slerp(-right, -up, (i - 1) / 25.0f) + start);
                    Gizmos.DrawLine(Vector3.Slerp(forward, -up, i / 25.0f) + start, Vector3.Slerp(forward, -up, (i - 1) / 25.0f) + start);
                    Gizmos.DrawLine(Vector3.Slerp(-forward, -up, i / 25.0f) + start, Vector3.Slerp(-forward, -up, (i - 1) / 25.0f) + start);

                    //End endcap
                    Gizmos.DrawLine(Vector3.Slerp(right, up, i / 25.0f) + end, Vector3.Slerp(right, up, (i - 1) / 25.0f) + end);
                    Gizmos.DrawLine(Vector3.Slerp(-right, up, i / 25.0f) + end, Vector3.Slerp(-right, up, (i - 1) / 25.0f) + end);
                    Gizmos.DrawLine(Vector3.Slerp(forward, up, i / 25.0f) + end, Vector3.Slerp(forward, up, (i - 1) / 25.0f) + end);
                    Gizmos.DrawLine(Vector3.Slerp(-forward, up, i / 25.0f) + end, Vector3.Slerp(-forward, up, (i - 1) / 25.0f) + end);
                }

                Gizmos.color = oldColor;
            }
        }
    }
}
