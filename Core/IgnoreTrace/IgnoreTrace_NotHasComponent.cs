using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public class IgnoreTrace_NotHasComponent<T> : IgnoreTrace
    {
        [Header(" [ Not Has Component ] ")]
        [SerializeField] private bool useRigidbody = true;
        [SerializeField] private bool checkHasRigidbody = true;
        public override void Ignore(Transform tracer, ref List<RaycastHit> hits)
        {
            if(useRigidbody)
            {
                for (int i = hits.Count - 1; i >= 0; i--)
                {
                    if (hits[i].rigidbody)
                    {
                        if (!hits[i].rigidbody.TryGetComponent(out T _))
                            hits.RemoveAt(i);
                    }
                    else
                    {
                        if (checkHasRigidbody)
                            hits.RemoveAt(i);
                    }
                }
            }
            else
            {
                for (int i = hits.Count - 1; i >= 0; i--)
                {
                    if (!hits[i].collider.gameObject.TryGetComponentInRootOrParent(out T _))
                    {
                        hits.RemoveAt(i);
                    }
                }
            }
            
        }
    }
}
