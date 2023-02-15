using UnityEngine;
using StudioScor.Utilities;
using System.Collections.Generic;


namespace Portfolio.Abilities
{
    public abstract class HitResultAction : BaseScriptableObject
    {
        public abstract void HitActions(Transform tracer, List<RaycastHit> hits);
    }
}
