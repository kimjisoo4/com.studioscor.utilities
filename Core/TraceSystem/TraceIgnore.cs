using UnityEngine;
using StudioScor.Utilities;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public abstract class TraceIgnore : BaseScriptableObject
    {
        public abstract void Ignore(Transform tracer, ref List<RaycastHit> hits);
    }
}
