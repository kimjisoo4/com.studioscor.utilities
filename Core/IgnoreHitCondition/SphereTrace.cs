using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/TraceSystem/Trace/new Sphere Trace", fileName = "Trace_SphereAll_")]
    public class SphereTrace : TraceSystem<FSphereTrace>
    {
        protected override bool OnTrace(Transform owner, FSphereTrace sphereTrace, ref List<RaycastHit> hits, List<Transform> ignoreHits = null, bool useDebug = false)
        {
            return SUtility.Physics.DrawSphereCastAll(sphereTrace.Start, sphereTrace.End, sphereTrace.Radius, sphereTrace.LayerMask, ref hits, ignoreHits, useDebug || UseDebug);
        }
    }
}
