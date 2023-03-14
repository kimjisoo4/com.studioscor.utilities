using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public class TraceIgnore_NotHasComponent<T> : TraceIgnore
    {
        [SerializeField] private bool _UseRigidbody = true;
        [SerializeField][SCondition(nameof(_UseRigidbody))] private bool _CheckHasRigidbody = true;
        public override void Ignore(Transform tracer, ref List<RaycastHit> hits)
        {
            if(_UseRigidbody)
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
                        if (_CheckHasRigidbody)
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
