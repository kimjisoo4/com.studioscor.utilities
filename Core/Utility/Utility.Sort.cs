using System.Collections.Generic;
using UnityEngine;


namespace StudioScor.Utilities
{

    public static partial class SUtility
    {
        public static class Sort
        {
            private static Vector3 _position;
            private static Quaternion _rotation;

            // RaycastHit
            #region Distance
            public static void SortRaycastHitByDistance(Transform target, ref List<RaycastHit> hits)
            {
                SortRaycastHitByDistance(target.position, ref hits);
            }
            public static void SortRaycastHitByDistance(Vector3 position, ref List<RaycastHit> hits)
            {
                _position = position;

                hits.Sort(CompareRaycastHitByDistance);

                _position = default;
            }
            private static int CompareRaycastHitByDistance(RaycastHit a, RaycastHit b)
            {
                if (a.distance < b.distance)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            #endregion

            #region Nearst Angle
            public static void SortRaycastHitPointByNearstAngle(Vector3 position, Quaternion rotation, ref List<RaycastHit> hits)
            {
                _position = position;
                _rotation = rotation;

                hits.Sort(CompareRaycastHitPointByNearstAngle);

                _position = default;
                _rotation = default;
            }
            public static void SortRaycastHitPointByNearstAngle(Transform target, ref List<RaycastHit> hits)
            {
                SortRaycastHitPointByNearstAngle(target.position, target.rotation, ref hits);
            }
            public static void SortRaycastHitPointByNearstAngle(Vector3 position, Vector3 direction, ref List<RaycastHit> hits)
            {
                SortRaycastHitPointByNearstAngle(position, Quaternion.Euler(direction), ref hits);
            }
            private static int CompareRaycastHitPointByNearstAngle(RaycastHit a, RaycastHit b)
            {
                if (Mathf.Abs(AngleOnForward(_position, _rotation, a.point)) > Mathf.Abs(AngleOnForward(_position, _rotation, b.point)))
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            #endregion

            // Collider
            #region Distance
            public static void SortTransformByDistance(Vector3 position, ref List<Collider> transforms)
            {
                _position = position;

                transforms.Sort(CompareTransformByDistance);

                _position = default;
            }

            public static void SortTransformByDistance(Transform target, ref List<Collider> transforms)
            {
                SortTransformByDistance(target.position, ref transforms);
            }
            private static int CompareTransformByDistance(Component a, Component b)
            {
                if (a == null)
                {
                    if (b == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (b == null)
                    {
                        return 1;
                    }
                    else
                    {
                        if (Vector3.Distance(_position, a.transform.position) < Vector3.Distance(_position, b.transform.position))
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
            #endregion

            #region Nearst Angle
            public static void SortTransformByNearstAngle(Vector3 position, Quaternion rotation, ref List<Collider> transforms)
            {
                _position = position;
                _rotation = rotation;

                transforms.Sort(CompareTransformByNearstAngle);

                _position = default;
                _rotation = default;
            }
            public static void SortTransformByNearstAngle(Vector3 position, Vector3 direction, ref List<Collider> transforms)
            {
                SortTransformByNearstAngle(position, Quaternion.Euler(direction), ref transforms);
            }
            public static void SortTransformByNearstAngle(Transform target, ref List<Collider> transforms)
            {
                SortTransformByNearstAngle(target.position, target.rotation, ref transforms);
            }
            private static int CompareTransformByNearstAngle(Collider a, Collider b)
            {
                if (a == null)
                {
                    if (b == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (b == null)
                    {
                        return 1;
                    }
                    else
                    {
                        if (Mathf.Abs(AngleOnForward(_position, _rotation, a.transform.position)) > Mathf.Abs(AngleOnForward(_position, _rotation, b.transform.position)))
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
            #endregion



            // Transform
            #region Distance
            public static void SortTransformByDistance(Vector3 position, ref List<Transform> transforms)
            {
                _position = position;

                transforms.Sort(CompareTransformByDistance);

                _position = default;
            }

            public static void SortTransformByDistance(Transform target, ref List<Transform> transforms)
            {
                SortTransformByDistance(target.position, ref transforms);
            }
            private static int CompareTransformByDistance(Transform a, Transform b)
            {
                if (a == null)
                {
                    if (b == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (b == null)
                    {
                        return 1;
                    }
                    else
                    {
                        if (Vector3.Distance(_position, a.position) < Vector3.Distance(_position, b.position))
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
            #endregion

            #region Nearst Angle
            public static void SortTransformByNearstAngle(Vector3 position, Quaternion rotation, ref List<Transform> transforms)
            {
                _position = position;
                _rotation = rotation;

                transforms.Sort(CompareTransformByNearstAngle);

                _position = default;
                _rotation = default;
            }
            public static void SortTransformByNearstAngle(Vector3 position, Vector3 direction, ref List<Transform> transforms)
            {
                SortTransformByNearstAngle(position, Quaternion.Euler(direction), ref transforms);
            }
            public static void SortTransformByNearstAngle(Transform target, ref List<Transform> transforms)
            {
                SortTransformByNearstAngle(target.position, target.rotation, ref transforms);
            }
            private static int CompareTransformByNearstAngle(Transform a, Transform b)
            {
                if (a == null)
                {
                    if (b == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (b == null)
                    {
                        return 1;
                    }
                    else
                    {
                        if (Mathf.Abs(AngleOnForward(_position, _rotation, a.position)) > Mathf.Abs(AngleOnForward(_position, _rotation, b.position)))
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
            #endregion
        }

    }
}