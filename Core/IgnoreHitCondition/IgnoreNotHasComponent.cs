using UnityEngine;
using System.Collections.Generic;


namespace Portfolio.Abilities
{
    public class IgnoreNotHasComponent<T> : IgnoreHitCondition where T : MonoBehaviour
    {
        public override void IngnoreHit(Transform tracer, ref List<RaycastHit> hits)
        {
            for(int i = hits.Count - 1; i >= 0; i--)
            {
                if(!hits[i].transform.TryGetComponent(out T _))
                {
                    hits.RemoveAt(i);
                }
            }
        }
    }
}
