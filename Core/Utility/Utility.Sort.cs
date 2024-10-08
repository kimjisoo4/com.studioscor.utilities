using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace StudioScor.Utilities
{

    public static partial class SUtility
    {
        public static class Sort
        {
            // RaycastHit
            #region Distance
            public static void SortDistanceToPointByRaycastHit(Vector3 position, List<RaycastHit> hits)
            {
                hits.Sort((a, b) => CompareDistanceToPosition(position, a.point, b.point));
            }
            public static void SortDistanceToPointByRaycastHit(Transform target, List<RaycastHit> hits)
            {
                hits.Sort((a, b) => CompareDistanceToPosition(target.position, a.point, b.point));
            }

            public static void SortDistanceToPointByBoundsCenter(Vector3 position, List<Collider> colliders)
            {
                colliders.Sort((a, b) => CompareDistanceToPosition(position, a.bounds.center, b.bounds.center));
            }

            public static void SortDistanceToPointByBoundsCenter(Transform target, List<Collider> colliders)
            {
                colliders.Sort((a, b) => CompareDistanceToPosition(target.position, a.bounds.center, b.bounds.center));
            }

            public static void SortDistanceToPoint(Vector3 position, List<Transform> transforms)
            {
                transforms.Sort((a, b) => CompareDistanceToPosition(position, a.position, b.position));
            }

            public static void SortDistanceToPoint(Transform target, List<Transform> transforms)
            {
                transforms.Sort((a, b) => CompareDistanceToPosition(target.position, a.position, b.position));
            }


            private static int CompareDistanceToPosition(Vector3 position, Vector3 lhs, Vector3 rhs)
            {
                float distanceLhs = Vector3.Distance(position, lhs);
                float distanceRhs = Vector3.Distance(position, rhs);

                return distanceLhs.CompareTo(distanceRhs);
            }


            public static void SortDistance2DToPointByRaycastHit(Vector2 position, List<RaycastHit2D> hits)
            {
                hits.Sort((a, b) => CompareDistance2DToPosition(position, a.point, b.point));
            }
            public static void SortDistance2DToPointByRaycastHit(Transform target, List<RaycastHit> hits)
            {
                hits.Sort((a, b) => CompareDistance2DToPosition(target.position, a.point, b.point));
            }

            public static void SortDistance2DToPointByBoundsCenter(Vector2 position, List<Collider> colliders)
            {
                colliders.Sort((a, b) => CompareDistance2DToPosition(position, a.bounds.center, b.bounds.center));
            }

            public static void SortDistance2DToPointByBoundsCenter(Transform target, List<Collider> colliders)
            {
                colliders.Sort((a, b) => CompareDistance2DToPosition(target.position, a.bounds.center, b.bounds.center));
            }

            public static void SortDistance2DToPoint(Vector2 position, List<Transform> transforms)
            {
                transforms.Sort((a, b) => CompareDistance2DToPosition(position, a.position, b.position));
            }

            public static void SortDistance2DToPoint(Transform target, List<Transform> transforms)
            {
                transforms.Sort((a, b) => CompareDistance2DToPosition(target.position, a.position, b.position));
            }

            public static void SortDistance2DToPoint(Vector2 position, List<GameObject> transforms)
            {
                transforms.Sort((a, b) => CompareDistance2DToPosition(position, a.transform.position, b.transform.position));
            }

            public static void SortDistance2DToPoint(Transform target, List<GameObject> transforms)
            {
                transforms.Sort((a, b) => CompareDistance2DToPosition(target.position, a.transform.position, b.transform.position));
            }


            private static int CompareDistance2DToPosition(Vector2 position, Vector2 lhs, Vector2 rhs)
            {
                float distanceLhs = Vector2.Distance(position, lhs);
                float distanceRhs = Vector2.Distance(position, rhs);

                return distanceLhs.CompareTo(distanceRhs);
            }

            #endregion

            #region Nearst Angle
            public static void SortAngleToPointByRaycastHit(Vector3 position, Quaternion rotation, List<RaycastHit> hits)
            {
                hits.Sort((a, b) => CompareAngleToPoint(position, rotation, a.point, b.point));
            }
            public static void SortAngleToPointByRaycastHit(Transform target, List<RaycastHit> hits)
            {
                hits.Sort((a, b) => CompareAngleToPoint(target.position, target.rotation, a.point, b.point));
            }
            public static void SortAngleToPointByRaycastHit(Vector3 position, Vector3 direction, List<RaycastHit> hits)
            {
                Quaternion rotation = Quaternion.Euler(direction);

                hits.Sort((a, b) => CompareAngleToPoint(position, rotation, a.point, b.point));
            }

            public static void SortAngleToPointByBounceCenter(Vector3 position, Quaternion rotation, List<Collider> transforms)
            {
                transforms.Sort((a, b) => CompareAngleToPoint(position, rotation, a.bounds.center, b.bounds.center));
            }
            public static void SortAngleToPointByBounceCenter(Vector3 position, Vector3 direction, List<Collider> transforms)
            {
                Quaternion rotation = Quaternion.Euler(direction);

                transforms.Sort((a, b) => CompareAngleToPoint(position, rotation, a.bounds.center, b.bounds.center));
            }
            public static void SortAngleToPointByBounceCenter(Transform target, List<Collider> transforms)
            {
                transforms.Sort((a, b) => CompareAngleToPoint(target.position, target.rotation, a.bounds.center, b.bounds.center));
            }


            public static void SortAngleToPoint(Vector3 position, Quaternion rotation, List<Transform> transforms)
            {
                transforms.Sort((a, b) => CompareAngleToPoint(position, rotation, a.position, b.position));
            }
            public static void SortAngleToPoint(Vector3 position, Vector3 direction, List<Transform> transforms)
            {
                Quaternion rotation = Quaternion.Euler(direction);

                transforms.Sort((a, b) => CompareAngleToPoint(position, rotation, a.position, b.position));
            }
            public static void SortAngleToPoint(Transform target, List<Transform> transforms)
            {
                transforms.Sort((a, b) => CompareAngleToPoint(target.position, target.rotation, a.position, b.position));
            }

            private static int CompareAngleToPoint(Vector3 position, Quaternion rotation, Vector3 lhs, Vector3 rhs)
            {
                var angleLhs = Mathf.Abs(AngleOnForward(position, rotation, lhs));
                var angleRhs = Mathf.Abs(AngleOnForward(position, rotation, rhs));

                return angleLhs.CompareTo(angleRhs);
            }
            #endregion
        }

    }
}