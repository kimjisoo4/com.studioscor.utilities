using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/TraceSystem/Ignore/new Ignore Self", fileName = "IgnoreTrace_Self")]
    public class IgnoreTrace_Self : IgnoreTrace
    {
        public override void Ignore(Transform tracer, ref List<RaycastHit> hits)
        {
            for (int i = hits.LastIndex(); i >= 0; i--)
            {
                if (hits[i].transform == tracer || (hits[i].rigidbody && hits[i].rigidbody.transform == tracer))
                {
                    hits.RemoveAt(i);
                }
            }
        }
    }
}
