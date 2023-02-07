using System.Collections.Generic;
using UnityEngine;


namespace StudioScor.Utilities
{

    public static partial class Utility
    {
        public static class Sort
        {
            private static Vector3 _Position;
            private static Quaternion _Rotation;

            // RaycastHit
            #region Distance
            public static void SortRaycastHitByDistance(Transform target, ref List<RaycastHit> hits)
            {
                SortRaycastHitByDistance(target.position, ref hits);
            }
            public static void SortRaycastHitByDistance(Vector3 position, ref List<RaycastHit> hits)
            {
                _Position = position;

                hits.Sort(CompareRaycastHitByDistance);

                _Position = default;
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
                _Position = position;
                _Rotation = rotation;

                hits.Sort(CompareRaycastHitPointByNearstAngle);

                _Position = default;
                _Rotation = default;
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
                if (Mathf.Abs(AngleOnForward(_Position, _Rotation, a.point)) > Mathf.Abs(AngleOnForward(_Position, _Rotation, b.point)))
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            #endregion


            // Transform
            #region Distance
            public static void SortTransformByDistance(Vector3 position, ref List<Transform> transforms)
            {
                _Position = position;

                transforms.Sort(CompareTransformByDistance);

                _Position = default;
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
                        if (Vector3.Distance(_Position, a.position) < Vector3.Distance(_Position, b.position))
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
                _Position = position;
                _Rotation = rotation;

                transforms.Sort(CompareTransformByNearstAngle);

                _Position = default;
                _Rotation = default;
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
                        if (Mathf.Abs(AngleOnForward(_Position, _Rotation, a.position)) > Mathf.Abs(AngleOnForward(_Position, _Rotation, b.position)))
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