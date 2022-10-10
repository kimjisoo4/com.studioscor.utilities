using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimScor.Utilities
{

    public static partial class Utilities
    {
        public static RaycastHit[] DrawSphereCastAll(Vector3 start, float radius, Vector3 direction, float distance, LayerMask layermask,
            bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
        {
            var hits = Physics.SphereCastAll(start, radius, direction, distance, layermask);

#if UNITY_EDITOR
            if (useDebug)
            {
                Color successColor = hitColor == default ? Color.green : hitColor;
                Color failedColor = rayColor == default ? Color.red : rayColor;

                if (hits.Length > 0)
                {
                    DrawSphereCast(start, radius, direction, distance, out RaycastHit firstHit, layermask);

                    DrawCapsule(start, start + direction * firstHit.distance, radius, failedColor, duration);
                    DrawCapsule(start + direction * firstHit.distance, start + direction * distance, radius, successColor, duration);

                    foreach (var hit in hits)
                    {
                        DrawPoint(hit.point, 0.1f, successColor, duration);
                    }

                }
                else
                {
                    DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                }
            }
#endif

            return hits;
        }
        public static bool DrawSphereCast(Vector3 start, float radius, Vector3 direction, float distance, out RaycastHit hit, LayerMask layermask,
            bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
        {
            bool isHit = Physics.SphereCast(start, radius, direction, out hit, distance, layermask);

#if UNITY_EDITOR
            if (useDebug)
            {
                Color successColor = hitColor == default ? Color.green : hitColor;
                Color failedColor = rayColor == default ? Color.red : rayColor;

                if (isHit)
                {
                    DrawCapsule(start, start + direction * hit.distance, radius, failedColor, duration);
                    DrawPoint(hit.point, 0.1f, successColor, duration);
                    DrawCapsule(start + direction * hit.distance, start + direction * distance, radius, successColor, duration);
                    
                }
                else
                {
                    DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                }
            }
#endif

            return isHit;
        }
        public static bool DrawRayCast(Vector3 start, Vector3 direction, float distance, out RaycastHit hit, LayerMask layerMask,
            bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
        {
            bool isHit = Physics.Raycast(start, direction, out hit, distance, layerMask);

#if UNITY_EDITOR
            if (useDebug)
            {
                Color successColor = hitColor == default ? Color.green : hitColor;
                Color failedColor = rayColor == default ? Color.red : rayColor;

                if (isHit)
                {
                    UnityEngine.Debug.DrawLine(start, hit.point, failedColor, duration);
                    DrawPoint(hit.point, 0.1f, successColor, duration);
                    UnityEngine.Debug.DrawLine(hit.point, start + direction * distance, successColor, duration);
                }
                else
                {
                    UnityEngine.Debug.DrawLine(start, start + direction * distance, failedColor, duration);
                    DrawPoint(start + direction * distance, 0.1f, failedColor, duration);
                }
            }
#endif
            return isHit;
        }
    }
}
