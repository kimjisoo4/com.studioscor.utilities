using UnityEngine;

namespace StudioScor.Utilities
{
    public class SphereTraceComponent : TraceComponent
    {
        [Header(" [ Sphere Trace Component ] ")]
        [SerializeField] private float _Radius = 1f;

        protected override bool TryTrace()
        {
            var startPosition = CalcPosition();
            var endPosition = _PrevPosition;

            _PrevPosition = startPosition;

            if (!SUtility.Physics.DrawSphereCastAll(startPosition, endPosition, _Radius, _Layer, ref _Hits, _IgnoreTransforms, UseDebug))
                return false;

            foreach (var traceIgnore in _TraceIgnores)
            {
                traceIgnore.Ignore(_Owner, ref _Hits);

                if (_Hits.Count <= 0)
                    return false;
            }

            return true;
        }
    }
}
