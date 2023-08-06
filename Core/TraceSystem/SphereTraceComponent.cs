using UnityEngine;

namespace StudioScor.Utilities
{
    public class SphereTraceComponent : TraceComponent
    {
        [Header(" [ Sphere Trace Component ] ")]
        [SerializeField] private float radius = 1f;

        protected override bool TryTrace()
        {
            var startPosition = CalcPosition();
            var endPosition = prevPosition;

            prevPosition = startPosition;

            if (!SUtility.Physics.DrawSphereCastAll(startPosition, endPosition, radius, layer, ref hits, ignoreTransforms, UseDebug))
                return false;

            foreach (var traceIgnore in traceIgnores)
            {
                traceIgnore.Ignore(owner, ref hits);

                if (hits.Count <= 0)
                    return false;
            }

            return true;
        }
    }
}
