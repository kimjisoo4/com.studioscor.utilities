using UnityEngine;
using StudioScor.Utilities;
using System.Collections.Generic;


namespace Portfolio.Abilities
{
    public abstract class IgnoreHitCondition : BaseScriptableObject
    {
        public abstract void IngnoreHit(Transform tracer, ref List<RaycastHit> hits);
    }
}
