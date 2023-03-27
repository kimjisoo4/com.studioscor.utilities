using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        public static bool ContaineTransform(this RaycastHit hit, Transform transform)
        {
            return hit.transform == transform || (hit.rigidbody && hit.rigidbody.transform == transform);
        }
        public static bool ContaineTransform(this RaycastHit hit, List<Transform> transforms)
        {
            foreach (var transform in transforms)
            {
                if (hit.ContaineTransform(transform))
                {
                    return true;
                }
            }

            return false;
        }
        

        public static void IgnoreHitResultsTransforms(RaycastHit[] hits, ref List<RaycastHit> results, List<Transform> transforms)
        {
            if (transforms is null || transforms.Count == 0)
            {
                results.AddRange(hits);

                return;
            }

            foreach (var hit in hits)
            {
                if (transforms.Contains(hit.transform) || (hit.rigidbody && transforms.Contains(hit.rigidbody.transform)))
                {
                    continue;
                }
                else
                {
                    results.Add(hit);

                }
            }
        }

        public static void IgnoreHitResultsTransform(RaycastHit[] hits, ref List<RaycastHit> results, Transform transform)
        {
            if (!transform)
            {
                results.AddRange(hits);

                return;
            }

            foreach (var hit in hits)
            {
                if (hit.transform == transform || (hit.rigidbody && hit.rigidbody.transform == transform))
                {
                    continue;
                }
                else
                {
                    results.Add(hit);
                }
            }
        }

    }
}
