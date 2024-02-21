using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public abstract class IgnoreTrace : BaseScriptableObject
    {
        public abstract void Ignore(Transform tracer, ref List<RaycastHit> hits);
    }
}
