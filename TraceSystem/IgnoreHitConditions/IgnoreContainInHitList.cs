using UnityEngine;
using System.Linq;
using System.Collections.Generic;


namespace StudioScor.TraceSystem
{
    [CreateAssetMenu(fileName = "IgnoreContainInHitList", menuName = "Trace/IgnoreHitCondition/New IgnoreContainHitList")]
    public class IgnoreContainInHitList : IgnoreHitCondition
    {
        public override void CheckIgnore(TraceBase owner, ref List<RaycastHit> hits)
        {
            for(int i = hits.Count - 1; i >= 0; i-- )
            {
                if (owner.HitList.Contains(hits[i].transform))
                {
                    hits.RemoveAt(i);
                }
            }
        }
    }
}