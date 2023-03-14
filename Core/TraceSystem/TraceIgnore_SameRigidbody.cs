using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName ="StudioScor/TraceSystem/Ignore/new Ignore Same Rigidbody", fileName = "Trace_Ignore_SameRigidbody")]
    public class TraceIgnore_SameRigidbody : TraceIgnore
    {
        [Header(" [ Trace Ignore Same Rigidbody ] ")]
        [SerializeField] private bool _CheckHasRigidbody = true;

        private List<Rigidbody> _Rigidbodys;

        protected override void OnReset()
        {
            base.OnReset();

            _Rigidbodys = null;

        }
        public override void Ignore(Transform tracer, ref List<RaycastHit> hits)
        {
            if (_Rigidbodys is null)
                _Rigidbodys = new();

            for(int i = hits.LastIndex(); i >= 0; i--)
            {
                if(hits[i].rigidbody)
                {
                    if (_Rigidbodys.Contains(hits[i].rigidbody))
                    {
                        hits.RemoveAt(i);
                    }
                    else
                    {
                        _Rigidbodys.Add(hits[i].rigidbody);
                    }
                }
                else
                {
                    if (_CheckHasRigidbody)
                        hits.RemoveAt(i);
                }
                
            }

            _Rigidbodys.Clear();
        }
    }
}
