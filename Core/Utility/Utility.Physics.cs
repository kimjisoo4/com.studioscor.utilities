using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace StudioScor.Utilities
{

    public static partial class SUtility
    {
        public static class Physics
        {
            #region ConeCast
            public static bool DrawConeCast(Vector3 position, Vector3 direction, float angle, float distance, LayerMask layerMask, ref List<Collider> hitResult, List<Transform> ignoreTransform,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
                Collider[] overlapHit = UnityEngine.Physics.OverlapSphere(position, distance, layerMask);

                if (overlapHit.Length == 0)
                {
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
                            Debug.DrawPoint(collider.transform.position, 0.1f, successColor, duration);
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
                            Debug.DrawPoint(collider.transform.position, 0.1f, successColor, duration);
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
                            Debug.DrawPoint(collider.transform.position, 0.1f, successColor, duration);
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
                            Debug.DrawPoint(collider.transform.position, 0.1f, successColor, duration);
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
                            Debug.DrawPoint(collider.transform.position, 0.1f, successColor, duration);
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
                            Debug.DrawPoint(collider.transform.position, 0.1f, successColor, duration);
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

            public static bool DrawOverlapSphere(Vector3 position, float radius, LayerMask layerMask, ref List<Collider> hitResults, List<Transform> IgnoreTransform, 
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

                if (IgnoreTransform.Count > 0)
                {
                    foreach (var collider in colliders)
                    {
                        if (!IgnoreTransform.Contains(collider.transform) && !IgnoreTransform.Contains(collider.transform.root))
                        {
                            hitResults.Add(collider);
                        }
                    }
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
                            Debug.DrawPoint(hit.transform.position, 0.1f, successColor, duration);
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



            public static List<Collider> DrawOverlapSphere(Vector3 position, float radius, LayerMask layerMask, List<Transform> IgnoreTransform,
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
                            Debug.DrawPoint(hit.point, 0.1f, successColor, duration);
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

            public static bool DrawSphereCastAll(Vector3 start, Vector3 end, float radius, LayerMask layermask, ref List<RaycastHit> hitResults, List<Transform> ignoreTransform = null,
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

                if(ignoreTransform is not null)
                {
                    foreach (var hit in hits)
                    {
                        if (!ignoreTransform.Contains(hit.transform) && !ignoreTransform.Contains(hit.transform.root))
                        {
                            hitResults.Add(hit);
                        }
                    }
                }
                else
                {
                    hitResults.AddRange(hits);
                }

                #region DEBUG DRAW
#if UNITY_EDITOR
                if (useDebug)
                {
                    Color successColor = hitColor == default ? Color.green : hitColor;
                    Color failedColor = rayColor == default ? Color.red : rayColor;

                    if (hitResults.Count > 0)
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
                #endregion

                return hitResults.Count > 0;
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
#region DEBUG DRAW
#if UNITY_EDITOR
                    if (useDebug)
                    {
                        Color failedColor = rayColor == default ? Color.red : rayColor;

                        Debug.DrawCapsule(start, start + direction * distance, radius, failedColor, duration);
                    }
#endif
#endregion

                    return null;
                }

                List<RaycastHit> hitList = new();


                foreach (var hit in hits)
                {
                    if (!ignoreTransform.Contains(hit.transform) && !ignoreTransform.Contains(hit.transform.root))
                    {
                        hitList.Add(hit);
                    }
                }

#region DEBUG DRAW
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
#endregion

                return hitList;
            }

            public static List<RaycastHit> DrawSphereCastAll(Vector3 start, float radius, Vector3 direction, float distance, LayerMask layermask, List<Transform> ignoreTransform,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
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

                    return null;
                }

                List<RaycastHit> hitList = new();


                foreach (var hit in hits)
                {
                    if (!ignoreTransform.Contains(hit.transform) && !ignoreTransform.Contains(hit.transform.root))
                    {
                        hitList.Add(hit);
                    }
                }

#region DEBUG DRAW
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
#endregion

                return hitList;
            }

            public static RaycastHit[] DrawSphereCastAll(Vector3 start, float radius, Vector3 direction, float distance, LayerMask layermask,
                bool useDebug = false, float duration = 0.2f, Color rayColor = default, Color hitColor = default)
            {
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
                            Debug.DrawPoint(hit.point, 0.1f, successColor, duration);
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
            #endregion
        }

    }
}