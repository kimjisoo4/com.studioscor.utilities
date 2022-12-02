using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.TraceSystem
{
    public abstract class IgnoreHitCondition : ScriptableObject
    {
        public abstract void CheckIgnore(TraceBase owner, ref List<RaycastHit> hits);
    }
}