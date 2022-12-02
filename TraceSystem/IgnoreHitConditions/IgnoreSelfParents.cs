using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.TraceSystem
{
    [CreateAssetMenu(menuName = "Trace/IgnoreHitCondition/New IgnoreSelfParents", fileName = "IgnoreSelfParents")]
    public class IgnoreSelfParents : IgnoreHitCondition
    {
        public override void CheckIgnore(TraceBase owner, ref List<RaycastHit> hits)
        {
            for(int i = hits.Count; i >= 0; i--)
            {
                if (hits[i].transform.root == owner.Transform.root)
                {
                    hits.RemoveAt(i);
                }
            }
        }
    }
}