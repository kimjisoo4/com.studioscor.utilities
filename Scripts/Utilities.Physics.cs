using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KimScor.Utilities
{

    public static partial class Utilities
    {
        public static class Physics
        {
            #region DrawOverlapSphere
            public static Collider[] DrawOverlapSphere(Vector3 position, float radius, LayerMask layerMask,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Collider[] colliders = UnityEngine.Physics.OverlapSphere(position, radius, layerMask);

#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (colliders.Length > 0)
                    {
                        Debug.DrawSphere(position, radius, successColor, duration);

                        foreach (var collider in colliders)
                        {
                            Debug.DrawPoint(collider.transform.position, 0.1f, successColor, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawSphere(position, radius, failedColor, duration);
                    }
                }
#endif
                return colliders;
            }
            public static List<Collider> DrawOverlapSphere(Vector3 position, float radius, LayerMask layerMask, List<Transform> IgnoreTransform,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Collider[] colliders = UnityEngine.Physics.OverlapSphere(position, radius, layerMask);

                List<Collider> hits = new();
                
                if (IgnoreTransform.Count > 0)
                {
                    foreach (var collider in colliders)
                    {
                        if (!IgnoreTransform.Contains(collider.transform) && !IgnoreTransform.Contains(collider.transform.root))
                        {
                            hits.Add(collider);
                        }
                    }
                }
                else
                {
                    hits = colliders.ToList();
                }

#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hits.Count > 0)
                    {
                        Debug.DrawSphere(position, radius, successColor, duration);

                        foreach (var hit in hits)
                        {
                            Debug.DrawPoint(hit.transform.position, 0.1f, successColor, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawSphere(position, radius, failedColor, duration);
                    }
                }
#endif
                return hits;
            }

            #endregion

            #region DrawSphereCastAll
            public static RaycastHit[] DrawSphereCastAll(Vector3 start, Vector3 end, float radius, LayerMask layermask,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Vector3 direction = start.Direction(end);
                float distance = Vector3.Distance(start, end);

                if (direction == Vector3.zero)
                {
                    direction = new Vector3(0, 0, 1f);
                    distance = 0.01f;
                }

                var hits = UnityEngine.Physics.SphereCastAll(start, radius, direction, distance, layermask);

#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hits.Length > 0)
                    {
                        DrawSphereCast(start, radius, direction, distance, out RaycastHit firstHit, layermask);

                        Debug.DrawCapsule(start, start + direction * firstHit.distance, radius, failedColor, duration);
                        Debug.DrawCapsule(start + direction * firstHit.distance, start + direction * distance, radius, successColor, duration);

                        foreach (var hit in hits)
                        {
                            Debug.DrawPoint(hit.point, 0.1f, successColor, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
                }
#endif
                return hits;
            }

            public static List<RaycastHit> DrawSphereCastAll(Vector3 start, Vector3 end, float radius, LayerMask layermask, List<Transform> ignoreTransform,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Vector3 direction = start.Direction(end);
                float distance = Vector3.Distance(start, end);

                if (direction == Vector3.zero)
                {
                    direction = new Vector3(0, 0, 1f);
                    distance = 0.01f;
                }

                var hits = UnityEngine.Physics.SphereCastAll(start, radius, direction, distance, layermask);

                if (hits.Length == 0)
                {
#if UNITY_EDITOR
                    if (useDebug)
                    {
                        Color failedColor = rayColor == default ? Color.red : rayColor;

                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
#endif
                    return null;
                }

                List<RaycastHit> hitList = new();

                for (int i = hits.Length; i >= 0; i--)
                {
                    if (!ignoreTransform.Contains(hits[i].transform) && !ignoreTransform.Contains(hits[i].transform.root))
                    {
                        hitList.Add(hits[i]);
                    }
                }

#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hitList.Count > 0)
                    {
                        DrawSphereCast(start, radius, direction, distance, out RaycastHit firstHit, layermask);

                        Debug.DrawCapsule(start, start + direction * firstHit.distance, radius, failedColor, duration);
                        Debug.DrawCapsule(start + direction * firstHit.distance, start + direction * distance, radius, successColor, duration);

                        foreach (var hit in hits)
                        {
                            Debug.DrawPoint(hit.point, 0.1f, successColor, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
                }
#endif
                return hitList;
            }
            public static RaycastHit[] DrawSphereCastAll(Vector3 start, float radius, Vector3 direction, float distance, LayerMask layermask,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                var hits = UnityEngine.Physics.SphereCastAll(start, radius, direction, distance, layermask);

#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hits.Length > 0)
                    {
                        DrawSphereCast(start, radius, direction, distance, out RaycastHit firstHit, layermask);

                        Debug.DrawCapsule(start, start + direction * firstHit.distance, radius, failedColor, duration);
                        Debug.DrawCapsule(start + direction * firstHit.distance, start + direction * distance, radius, successColor, duration);

                        foreach (var hit in hits)
                        {
                            Debug.DrawPoint(hit.point, 0.1f, successColor, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
                }
#endif

                return hits;
            }

            #endregion
            public static bool DrawSphereCast(Vector3 start, float radius, Vector3 direction, float distance, out RaycastHit hit, LayerMask layermask,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                bool isHit = UnityEngine.Physics.SphereCast(start, radius, direction, out hit, distance, layermask);

#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (isHit)
                    {
                        Debug.DrawCapsule(start, start + direction * hit.distance, radius, failedColor, duration);
                        Debug.DrawPoint(hit.point, 0.1f, successColor, duration);
                        Debug.DrawCapsule(start + direction * hit.distance, start + direction * distance, radius, successColor, duration);

                    }
                    else
                    {
                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
                }
#endif

                return isHit;
            }
            public static bool DrawRayCast(Vector3 start, Vector3 direction, float distance, out RaycastHit hit, LayerMask layerMask,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                bool isHit = UnityEngine.Physics.Raycast(start, direction, out hit, distance, layerMask);

#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (isHit)
                    {
                        UnityEngine.Debug.DrawLine(start, hit.point, failedColor, duration);
                        Debug.DrawPoint(hit.point, 0.1f, successColor, duration);
                        UnityEngine.Debug.DrawLine(hit.point, start + direction * distance, successColor, duration);
                    }
                    else
                    {
                        UnityEngine.Debug.DrawLine(start, start + direction * distance, failedColor, duration);
                        Debug.DrawPoint(start + direction * distance, 0.1f, failedColor, duration);
                    }
                }
#endif
                return isHit;
            }
        }
    }
}