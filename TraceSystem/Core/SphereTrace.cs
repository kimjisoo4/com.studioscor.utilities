using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.TraceSystem
{
    [System.Serializable]
    public struct FSphereTrace
    {
        [Header(" [ Trace Base Setting ] ")]
        public LayerMask layerMask;
        public Vector3 Offset;
        public bool IsIgnoreSelf;
        public bool IsIgnoreSelfRoot;
        public bool UseDebugMode;
        public IgnoreHitCondition[] ignoreContainInHitList;

        [Header(" [ Sphere Trace Setting ] ")]
        public float Radius;
    }

    [System.Serializable]
    public class SphereTrace : SphereTraceBase
    {
        public override float Radius => _SphereTrace.Radius;

        public override LayerMask LayerMask => _SphereTrace.layerMask;

        public override IReadOnlyCollection<IgnoreHitCondition> IgnoreHitConditions => _SphereTrace.ignoreContainInHitList;

        public override bool IsIgnoreSelf => _SphereTrace.IsIgnoreSelf;

        public override Transform Transform => _Transform;

        public override bool UseDebugMode => _SphereTrace.UseDebugMode;

        public override Vector3 Offset => _SphereTrace.Offset;

        public SphereTrace(FSphereTrace sphereTrace, Transform transform)
        {
            _SphereTrace = sphereTrace;
            _Transform = transform;
        }

        public void SetTrasnform(Transform transform)
        {
            _Transform = transform;
        }

        private FSphereTrace _SphereTrace;
        private Transform _Transform;
    }

    [System.Serializable]
    public abstract class SphereTraceBase : TraceBase
    {
        public abstract float Radius { get; }

        public override RaycastHit[] Trace()
        {
            RaycastHit[] hits;

            hits = Physics.SphereCastAll(Transform.position + Transform.TransformDirection(Offset),
                                                        Radius,
                                                        Transform.forward,
                                                        0.1f,
                                                        LayerMask
                                                        );

            return hits;
        }

        public override void OnDrawGizmos()
        {
            if (!UseDebugMode || Transform is null || !IsActive)
                return;


            Matrix4x4 matrix = Matrix4x4.TRS(Transform.position + Transform.TransformDirection(Offset), Transform.rotation, Vector3.one);
            Gizmos.matrix = matrix;

            Gizmos.DrawWireSphere(Vector3.zero, Radius);
        }
    }
}