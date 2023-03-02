using UnityEngine;
using StudioScor.Utilities;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public abstract class TraceAction : BaseScriptableObject
    {
        public abstract void Action(Transform tracer, ref List<RaycastHit> hits);
    }
}
