using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{

    public static partial class SUtility
    {
        public static class Physics
        {
            public static void IgnoreCollision(GameObject lhs, GameObject rhs, bool isIgnore)
            {
                var lhsColliders = lhs.GetComponentsInChildren<Collider>();
                var rhsColliders = rhs.GetComponentsInChildren<Collider>();

                foreach (var lhsCollider in lhsColliders)
                {
                    foreach (var rhsCollider in rhsColliders)
                    {
                        UnityEngine.Physics.IgnoreCollision(lhsCollider, rhsCollider, isIgnore);
                    }
                }

            }

            #region ConeCast

            public static int DrawConeCastNonAlloc(Vector3 position, Vector3 direction, float angle, float distance, LayerMask layerMask, Collider[] hitResult,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                int hitCount = UnityEngine.Physics.OverlapSphereNonAlloc(position, distance, hitResult, layerMask);



                if (hitCount == 0)
                {
#if UNITY_EDITOR
                    if (useDebug)
                    {
                        Color failedColor = rayColor == default ? Color.red : rayColor;

                        Debug.DrawCone(position, direction, distance, angle, failedColor, duration);
                    }
#endif
                    return 0;
                }

                float halfAngle = angle * 0.5f;
                int offsetCount = 0;

                for(int i = 0; i < hitCount; i++)
                {
                    var hit = hitResult[i];

                    Vector3 targetDirection = position.Direction(hit.transform.position);

                    float hitAngle = Vector3.Angle(direction, targetDirection);

                    if (hitAngle > 180)
                        hitAngle -= 360f;

                    if (!hitAngle.InRange(-halfAngle, halfAngle))
                    {
                        offsetCount++;
                    }
                    else
                    {
                        if (offsetCount > 0)
                        {
                            hitResult[i - offsetCount] = hitResult[i];
                        }
                    }
                }

#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = rayColor == default ? Color.green : rayColor;

                    Debug.DrawCone(position, direction, distance, angle, successColor, duration);

                    for(int i = 0; i < hitCount - offsetCount; i++)
                    {
                        var hit = hitResult[i];

                        Debug.DrawPoint(hit.transform.position, successColor, 1, duration);
                    }
                }
#endif

                return hitCount - offsetCount;
            }
            public static bool DrawConeCast(Vector3 position, Vector3 direction, float angle, float distance, LayerMask layerMask, ref List<Collider> hitResult, List<Transform> ignoreTransform,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Collider[] overlapHit = UnityEngine.Physics.OverlapSphere(position, distance, layerMask);

                if (overlapHit.Length == 0)
                {
#if UNITY_EDITOR
                    if (useDebug)
                    {
                        Color failedColor = rayColor == default ? Color.red : rayColor;

                        Debug.DrawCone(position, direction, distance, angle, failedColor, duration);
                    }
#endif
                    return false;
                }

                float halfAngle = angle * 0.5f;

                foreach (Collider hit in overlapHit)
                {
                    if (ignoreTransform.Count > 0)
                    {
                        if (ignoreTransform.Contains(hit.transform) || ignoreTransform.Contains(hit.transform.root))
                        {
                            continue;
                        }
                    }

                    Vector3 targetDirection = position.Direction(hit.transform.position);

                    float hitAngle = Vector3.Angle(direction, targetDirection);

                    if (hitAngle > 180)
                        hitAngle -= 360f;

                    if (hitAngle <= halfAngle && hitAngle >= -halfAngle)
                    {
                        hitResult.Add(hit);
                    }
                }

                #region DEBUG_DRAW
#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hitResult.Count > 0)
                    {
                        Debug.DrawCone(position, direction, distance, angle, successColor, duration);

                        foreach (var collider in hitResult)
                        {
                            Debug.DrawPoint(collider.transform.position, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawCone(position, direction, distance, angle, failedColor, duration);
                    }
                }
#endif
                #endregion

                return hitResult.Count > 0;
            }


            public static bool DrawConeCast(Transform transform, float angle, float distance, LayerMask layerMask, ref List<Collider> hitResult, List<Transform> ignoreTransform,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Collider[] overlapHit = UnityEngine.Physics.OverlapSphere(transform.position, distance, layerMask);

                if (overlapHit.Length == 0)
                {
#if UNITY_EDITOR
                    if (useDebug)
                    {
                        Color failedColor = rayColor == default ? Color.red : rayColor;

                        Debug.DrawCone(transform.position, transform.rotation, distance, angle, failedColor, duration);
                    }
#endif
                    return false;
                }

                float halfAngle = angle * 0.5f;

                foreach (Collider hit in overlapHit)
                {
                    if (ignoreTransform.Count > 0)
                    {
                        if (ignoreTransform.Contains(hit.transform) || ignoreTransform.Contains(hit.transform.root))
                        {
                            continue;
                        }
                    }

                    Vector3 direction = transform.Direction(hit.transform);

                    float hitAngle = Vector3.Angle(transform.forward, direction);

                    if (hitAngle > 180)
                        hitAngle -= 360f;

                    if (hitAngle <= halfAngle && hitAngle >= -halfAngle)
                    {
                        hitResult.Add(hit);
                    }
                }

                #region DEBUG_DRAW
#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hitResult.Count > 0)
                    {
                        Debug.DrawCone(transform.position, transform.rotation, distance, angle, successColor, duration);

                        foreach (var collider in hitResult)
                        {
                            Debug.DrawPoint(collider.transform.position, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawCone(transform.position, transform.rotation, distance, angle, failedColor, duration);
                    }
                }
#endif
                #endregion

                return hitResult.Count > 0;
            }

            public static List<Collider> DrawConeCast(Transform transform, float angle, float distance, LayerMask layerMask, List<Transform> ignoreTransform,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Collider[] overlapHit = UnityEngine.Physics.OverlapSphere(transform.position, distance, layerMask);

                if (overlapHit.Length == 0)
                {
#if UNITY_EDITOR
                    if (useDebug)
                    {
                        Color failedColor = rayColor == default ? Color.red : rayColor;

                        Debug.DrawCone(transform.position, transform.rotation, distance, angle, failedColor, duration);
                    }
#endif
                    return null;
                }

                List<Collider> colliders = new List<Collider>();

                float halfAngle = angle * 0.5f;

                foreach (Collider hit in overlapHit)
                {
                    if (ignoreTransform.Count > 0)
                    {
                        if (ignoreTransform.Contains(hit.transform) || ignoreTransform.Contains(hit.transform.root))
                        {
                            continue;
                        }
                    }

                    Vector3 direction = transform.Direction(hit.transform);

                    float hitAngle = Vector3.Angle(transform.forward, direction);

                    if (hitAngle > 180)
                        hitAngle -= 360f;

                    if (hitAngle <= halfAngle && hitAngle >= -halfAngle)
                    {
                        colliders.Add(hit);
                    }
                }

                #region DEBUG_DRAW
#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (colliders.Count > 0)
                    {
                        Debug.DrawCone(transform.position, transform.rotation, distance, angle, successColor, duration);

                        foreach (var collider in colliders)
                        {
                            Debug.DrawPoint(collider.transform.position, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawCone(transform.position, transform.rotation, distance, angle, failedColor, duration);
                    }
                }
#endif
                #endregion

                return colliders;
            }


            public static List<Collider> DrawConeCast(Transform transform, float angle, float distance, LayerMask layerMask,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Collider[] overlapHit = UnityEngine.Physics.OverlapSphere(transform.position, distance, layerMask);

                if (overlapHit.Length == 0)
                {
#if UNITY_EDITOR
                    if (useDebug)
                    {
                        Color failedColor = rayColor == default ? Color.red : rayColor;

                        Debug.DrawCone(transform.position, transform.rotation, distance, angle, failedColor, duration);
                    }
#endif
                    return null;
                }

                List<Collider> colliders = new List<Collider>();

                float halfAngle = angle * 0.5f;

                foreach (Collider hit in overlapHit)
                {
                    Vector3 direction = transform.Direction(hit.transform);

                    float hitAngle = Vector3.Angle(transform.forward, direction);

                    if (hitAngle > 180)
                        hitAngle -= 360f;

                    if (hitAngle <= halfAngle && hitAngle >= -halfAngle)
                    {
                        colliders.Add(hit);
                    }
                }

#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (colliders.Count > 0)
                    {
                        Debug.DrawCone(transform.position, transform.rotation, distance, angle, successColor, duration);

                        foreach (var collider in colliders)
                        {
                            Debug.DrawPoint(collider.transform.position, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawCone(transform.position, transform.rotation, distance, angle, failedColor, duration);
                    }
                }
#endif

                return colliders;
            }
            #endregion

            #region Draw Slice OverlapSphere

            public static List<Collider> DrawSliceOverlapSphere(Vector3 position, Quaternion rotation, float radius, float horizontalAngle, float verticalAngle, LayerMask layerMask, List<Transform> ignoreTransforms,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Collider[] hits = UnityEngine.Physics.OverlapSphere(position, radius, layerMask);

                if (hits.Length == 0)
                {
                    Debug.DrawSliceSphere(position, rotation, radius, horizontalAngle, verticalAngle, rayColor == default ? Color.red : rayColor, duration);

                    return null;
                }

                var Hits = new List<Collider>();

                float halfHorizontal = horizontalAngle * 0.5f;
                float halfVertical = verticalAngle * 0.5f;

                Vector3 forward = rotation * Vector3.forward;

                foreach (Collider hit in hits)
                {
                    if (ignoreTransforms.Contains(hit.transform) || ignoreTransforms.Contains(hit.transform.root))
                    {
                        continue;
                    }

                    Vector3 direction = hit.transform.position - position;

                    Vector3 angle = Quaternion.FromToRotation(forward, direction).eulerAngles;

                    if (angle.y > 180)
                        angle.y -= 360f;

                    if (angle.x > 180)
                        angle.x -= 360f;


                    if (angle.y <= halfHorizontal && angle.y >= -halfHorizontal && angle.x <= halfVertical && angle.x >= -halfVertical)
                    {
                        Hits.Add(hit);
                    }
                }

                #region DEBUG_DRAW
#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (Hits.Count > 0)
                    {
                        Debug.DrawSliceSphere(position, rotation, radius, horizontalAngle, verticalAngle, successColor, duration);

                        foreach (var collider in hits)
                        {
                            Debug.DrawPoint(collider.transform.position, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawSliceSphere(position, rotation, radius, horizontalAngle, verticalAngle, failedColor, duration);
                    }
                }
#endif
                #endregion

                return Hits;
            }

            public static List<Collider> DrawOverlapSphereSlice(Vector3 position, Quaternion rotation, float radius, float horizontalAngle, float verticalAngle, LayerMask layerMask,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Collider[] hits = UnityEngine.Physics.OverlapSphere(position, radius, layerMask);

                if (hits.Length == 0)
                {
                    Debug.DrawSliceSphere(position, rotation, radius, horizontalAngle, verticalAngle, rayColor == default ? Color.red : rayColor, duration);

                    return null;
                }

                var Hits = new List<Collider>();

                float halfHorizontal = horizontalAngle * 0.5f;
                float halfVertical = verticalAngle * 0.5f;

                foreach (Collider hit in hits)
                {

                    Vector3 direction = hit.transform.position - position;

                    Vector3 angle = Quaternion.FromToRotation(rotation * Vector3.forward, direction).eulerAngles;

                    if (angle.y > 180)
                        angle.y -= 360f;

                    if (angle.x > 180)
                        angle.x -= 360f;


                    if (angle.y <= halfHorizontal && angle.y >= -halfHorizontal && angle.x <= halfVertical && angle.x >= -halfVertical)
                    {
                        Hits.Add(hit);
                    }
                }

                #region DEBUG_DRAW
#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (Hits.Count > 0)
                    {
                        Debug.DrawSliceSphere(position, rotation, radius, horizontalAngle, verticalAngle, successColor, duration);

                        foreach (var collider in hits)
                        {
                            Debug.DrawPoint(collider.transform.position, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawSliceSphere(position, rotation, radius, horizontalAngle, verticalAngle, failedColor, duration);
                    }
                }
#endif
                #endregion

                return Hits;
            }

            #endregion

            #region DrawOverlapSphereNoneAlloc
            public static int DrawOverlapSphereNoneAlloc(Vector3 position, float radius, Collider[] results,
                                                     int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                                                     bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                if (results is null || results.Length <= 0)
                {
                    Debug.LogError($"[ {nameof(results)} ] is Null or Length is Zero!.", null, STRING_COLOR_RED);
                    return -1;
                }

                int hitCount = UnityEngine.Physics.OverlapSphereNonAlloc(position, radius, results, layerMask, queryTriggerInteraction);

                #region EDITOR ONLY
#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hitCount > 0)
                    {
                        Debug.DrawSphere(position, radius, successColor, duration);

                        for(int i = 0; i < hitCount; i++)
                        {
                            var result = results[i];
                            Debug.DrawPoint(result.bounds.center, 1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawSphere(position, radius, failedColor, duration);
                    }
                }
#endif
                #endregion

                return hitCount;
            }
            #endregion
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
                            Debug.DrawPoint(collider.transform.position, successColor, 0.1f, duration);
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

            public static bool DrawOverlapSphere(Vector3 position, float radius, LayerMask layerMask, ref List<Collider> hitResults, List<Transform> ignoreTransform = null,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Collider[] colliders = UnityEngine.Physics.OverlapSphere(position, radius, layerMask);

                if (colliders.Length == 0)
                {
#if UNITY_EDITOR
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    Debug.DrawSphere(position, radius, failedColor, duration);
#endif
                    return false;
                }

                if (ignoreTransform is not null && ignoreTransform.Count > 0)
                {
                    foreach (var collider in colliders)
                    {
                        if (!ignoreTransform.Contains(collider.transform)
                            && (!collider.attachedRigidbody || !ignoreTransform.Contains(collider.attachedRigidbody.transform)))
                        {
                            hitResults.Add(collider);
                        }
                    }
                }
                else
                {
                    hitResults.AddRange(colliders);
                }

#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hitResults.Count > 0)
                    {
                        Debug.DrawSphere(position, radius, successColor, duration);

                        foreach (var hit in hitResults)
                        {
                            Debug.DrawPoint(hit.transform.position, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawSphere(position, radius, failedColor, duration);
                    }
                }
#endif

                return hitResults.Count > 0;
            }



            public static List<Collider> DrawOverlapSphere(Vector3 position, float radius, LayerMask layerMask, List<Transform> IgnoreTransform = null,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Collider[] colliders = UnityEngine.Physics.OverlapSphere(position, radius, layerMask);

                if (colliders.Length == 0)
                {
#if UNITY_EDITOR
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    Debug.DrawSphere(position, radius, failedColor, duration);
#endif
                    return null;
                }

                List<Collider> hits = new();

                if (IgnoreTransform is not null && IgnoreTransform.Count > 0)
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
                    hits.AddRange(colliders);
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
                            Debug.DrawPoint(hit.transform.position, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawSphere(position, radius, failedColor, duration);
                    }
                }
#endif

                if (hits.Count > 0)
                {
                    return hits;
                }
                else
                {
                    return null;
                }
            }
            #endregion

            #region Draw Ray Cast 
            public static bool DrawRayCast(Vector3 position, Vector3 direction, out RaycastHit hitResult,
                                          float distance = Mathf.Infinity, int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                                          bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                bool isHit = UnityEngine.Physics.Raycast(position, direction, out hitResult, distance, layerMask, queryTriggerInteraction);

                #region EDITOR ONLY
#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;


                    if (isHit)
                    {
                        Debug.DrawLine(position, hitResult.point, failedColor, duration);
                        Debug.DrawPoint(hitResult.point, successColor, 1f, duration);
                        Debug.DrawLine(hitResult.point, position + direction * distance, successColor, duration);
                    }
                    else
                    {
                        Debug.DrawRay(position, direction * distance, failedColor, duration);
                    }
                }
#endif
                #endregion

                return isHit;
            }
            public static bool DrawRayCast(Ray ray, out RaycastHit hitResult,
                                          float distance = Mathf.Infinity, int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                                          bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                return DrawRayCast(ray.origin, ray.direction, out hitResult, distance, layerMask, queryTriggerInteraction, useDebug, duration, rayColor, hitColor);
            }

            public static int DrawRayCastNonAlloc(Vector3 position, Vector3 direction, RaycastHit[] hitResult,
                                          float distance = Mathf.Infinity, int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                                          bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                int hitCount = UnityEngine.Physics.RaycastNonAlloc(position, direction, hitResult, distance, layerMask, queryTriggerInteraction);
                #region EDITOR ONLY
#if UNITY_EDITOR
                if (hitCount > 0)
                {
                    Vector3 prevPosition = position;
                    for (int i = 0; i < hitCount; i++)
                    {
                        var hit = hitResult[i];

                        Debug.DrawLine(prevPosition, hit.point, hitColor, duration);
                        Debug.DrawPoint(hit.point, 1f, duration);

                        prevPosition = hit.point;
                    }

                    Debug.DrawLine(prevPosition, position + direction * distance, rayColor, duration);
                }
                else
                {
                    Debug.DrawRay(position, direction * distance, rayColor, duration);
                }
#endif
                #endregion
                return hitCount;
            }


            public static int DrawLineCastNonAlloc(Vector3 startPosition, Vector3 endPosition, RaycastHit[] hitResult,
                                     int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                                     bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Vector3 direction = startPosition.Direction(endPosition, false);
                float distance = direction.magnitude;
                direction.Normalize();

                int hitCount = DrawRayCastNonAlloc(startPosition, direction, hitResult, distance, layerMask, queryTriggerInteraction);

                return hitCount;
            }
            #endregion

            #region Draw Shpere Cast All Non Alloc

            public static int DrawSphereCastAllNonAlloc(Vector3 start, float radius, Vector3 direction, RaycastHit[] hitResults,
                                                       float distance = float.PositiveInfinity, int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                                                       bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                int hitCount = UnityEngine.Physics.SphereCastNonAlloc(start, radius, direction, hitResults, distance, layerMask, queryTriggerInteraction);

                #region EDITOR ONLY
#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hitResults.Length > 0)
                    {
                        DrawSphereCast(start, radius, direction, distance, out RaycastHit firstHit, layerMask);

                        Debug.DrawCapsule(start, start + direction * firstHit.distance, radius, failedColor, duration);
                        Debug.DrawCapsule(start + direction * firstHit.distance, start + direction * distance, radius, successColor, duration);

                        foreach (var hit in hitResults)
                        {
                            Debug.DrawPoint(hit.point, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
                }
#endif
                #endregion

                return hitCount;
            }
            public static int DrawSphereCastAllNonAlloc(Ray ray, float radius, RaycastHit[] hitResults,
                                           float distance = float.PositiveInfinity, int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                                           bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Vector3 start = ray.origin;
                Vector3 direction = ray.direction;

                return DrawSphereCastAllNonAlloc(start, radius, direction, hitResults, distance, layerMask, queryTriggerInteraction, useDebug, duration, rayColor, hitColor);
            }
            public static int DrawSphereCastAllNonAlloc(Vector3 start, Vector3 end, float radius, RaycastHit[] hitResults,
                                                       int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                                                       bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Vector3 direction = start.Direction(end, false);
                float distance;

                if (direction == Vector3.zero)
                {
                    direction = Vector3.forward;
                    distance = 0.01f;
                }
                else
                {
                    distance = direction.magnitude;
                    direction.Normalize();
                }


                return DrawSphereCastAllNonAlloc(start, radius, direction, hitResults, distance, layerMask, queryTriggerInteraction, useDebug, duration, rayColor, hitColor);
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

                #region DEBUG DRAW
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
                            Debug.DrawPoint(hit.point, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
                }
#endif
                #endregion

                return hits;
            }
            public static RaycastHit[] DrawSphereCastAll(Ray ray, float distance, float radius, LayerMask layermask,
               bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                return DrawSphereCastAll(ray.origin, ray.origin + ray.direction * distance, radius, layermask, useDebug, duration, rayColor, hitColor);
            }
            public static RaycastHit[] DrawSphereCastAll(Vector3 origin, Vector3 direction, float distance, float radius, LayerMask layermask,
               bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                return DrawSphereCastAll(origin, origin + direction * distance, radius, layermask, useDebug, duration, rayColor, hitColor);
            }

            public static bool DrawSphereCastAll(Vector3 start, Vector3 end, float radius, LayerMask layermask, ref List<RaycastHit> hitResults,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                var hits = DrawSphereCastAll(start, end, radius, layermask, useDebug, duration, rayColor, hitColor);

                if (hits.Length == 0)
                    return false;

                hitResults.AddRange(hits);

                return true;
            }
            public static bool DrawSphereCastAll(Vector3 origin, Vector3 direction, float distance, float radius, LayerMask layermask, ref List<RaycastHit> hitResults,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                return DrawSphereCastAll(origin, origin + direction * distance, radius, layermask, ref hitResults, useDebug, duration, rayColor, hitColor);
            }
            public static bool DrawSphereCastAll(Ray ray, float distance, float radius, LayerMask layermask, ref List<RaycastHit> hitResults,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                return DrawSphereCastAll(ray.origin, ray.origin + ray.direction * distance, radius, layermask, ref hitResults, useDebug, duration, rayColor, hitColor);
            }


            public static bool DrawSphereCastAll(Vector3 start, Vector3 end, float radius, LayerMask layerMask, ref List<RaycastHit> hitResults, Transform owner, List<Transform> ignoreTransforms = null,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                bool isZero = start.SafeEquals(end);

                Vector3 direction = isZero ? Vector3.forward : start.Direction(end);
                float distance = isZero ? 0.01f : Vector3.Distance(start, end);

                var hits = UnityEngine.Physics.SphereCastAll(start, radius, direction, distance, layerMask);

                if (hits.Length == 0)
                {
                    #region DEBUG DRAW
#if UNITY_EDITOR
                    if (useDebug)
                    {
                        Color failedColor = rayColor == default ? Color.red : rayColor;

                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
#endif
                    #endregion

                    return false;
                }

                IgnoreHitResultsTransform(hits, ref hitResults, owner);
                IgnoreHitResultsTransforms(hits, ref hitResults, ignoreTransforms);

                #region DEBUG DRAW
#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hitResults.Count > 0)
                    {
                        SUtility.Sort.SortDistanceToPointByRaycastHit(start, hitResults);

                        Debug.DrawCapsule(start, start + direction * hitResults[0].distance, radius, failedColor, duration);
                        Debug.DrawCapsule(start + direction * hitResults[0].distance, start + direction * distance, radius, successColor, duration);

                        foreach (var hit in hitResults)
                        {
                            Debug.DrawPoint(hit.point, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
                }
#endif
                #endregion

                return hitResults.Count > 0;
            }
            public static bool DrawSphereCastAll(Vector3 origin, Vector3 direction, float distance, float radius, LayerMask layerMask, ref List<RaycastHit> hitResults, Transform owner, List<Transform> ignoreTransforms = null,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                return DrawSphereCastAll(origin, origin + direction * distance, radius, layerMask, ref hitResults, owner, ignoreTransforms, useDebug, duration, rayColor, hitColor);
            }
            public static bool DrawSphereCastAll(Ray ray, float distance, float radius, LayerMask layerMask, ref List<RaycastHit> hitResults, Transform owner, List<Transform> ignoreTransforms = null,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                return DrawSphereCastAll(ray.origin, ray.origin + ray.direction * distance, radius, layerMask, ref hitResults, owner, ignoreTransforms, useDebug, duration, rayColor, hitColor);
            }
            public static bool DrawSphereCastAll(Vector3 start, Vector3 end, float radius, LayerMask layermask, ref List<RaycastHit> hitResults, List<Transform> ignoreTransforms = null,
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
                    #region DEBUG DRAW
#if UNITY_EDITOR
                    if (useDebug)
                    {
                        Color failedColor = rayColor == default ? Color.red : rayColor;

                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
#endif
                    #endregion

                    return false;
                }

                IgnoreHitResultsTransforms(hits, ref hitResults, ignoreTransforms);

                #region DEBUG DRAW
#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hitResults.Count > 0)
                    {
                        SUtility.Sort.SortDistanceToPointByRaycastHit(start, hitResults);

                        Debug.DrawCapsule(start, start + direction * hitResults[0].distance, radius, failedColor, duration);
                        Debug.DrawCapsule(start + direction * hitResults[0].distance, start + direction * distance, radius, successColor, duration);

                        foreach (var hit in hitResults)
                        {
                            Debug.DrawPoint(hit.point, successColor, 0.1f, duration);
                        }
                    }
                    else
                    {
                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
                }
#endif
                #endregion

                return hitResults.Count > 0;
            }
            public static bool DrawSphereCastAll(Vector3 origin, Vector3 direction, float distance, float radius, LayerMask layermask, ref List<RaycastHit> hitResults, List<Transform> ignoreTransforms = null,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                return DrawSphereCastAll(origin, origin + direction * distance, radius, layermask, ref hitResults, ignoreTransforms, useDebug, duration, rayColor, hitColor);
            }
            public static bool DrawSphereCastAll(Ray ray, float distance, float radius, LayerMask layermask, ref List<RaycastHit> hitResults, List<Transform> ignoreTransforms = null,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                return DrawSphereCastAll(ray.origin, ray.origin + ray.direction * distance, radius, layermask, ref hitResults, ignoreTransforms, useDebug, duration, rayColor, hitColor);
            }

            #endregion

            #region DrawSphereCast
            public static bool DrawSphereCast(Vector3 start, Vector3 end, float radius, out RaycastHit hit, LayerMask layerMask,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Vector3 direction = start.Direction(end, false);
                float distance = direction.magnitude;

                return DrawSphereCast(start, radius, direction, distance, out hit, layerMask, useDebug, duration, rayColor, hitColor);
            }

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
                        Debug.DrawPoint(hit.point, successColor, 0.1f, duration);
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
        }
        #endregion
    }
}
