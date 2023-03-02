using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public class TraceIgnore_NotHasComponent<T> : TraceIgnore
    {
        public override void Ingnore(Transform tracer, ref List<RaycastHit> hits)
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
