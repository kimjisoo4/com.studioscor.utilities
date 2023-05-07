using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName ="StudioScor/TraceSystem/Ignore/new Ignore Same Rigidbody", fileName = "Trace_Ignore_SameRigidbody")]
    public class TraceIgnore_SameRigidbody : TraceIgnore
    {
        [Header(" [ Trace Ignore Same Rigidbody ] ")]
        [SerializeField] private bool _IgnoreSelf = true;
        [SerializeField] private bool _CheckHasRigidbody = true;

        private List<Transform> _Ignores;

        protected override void OnReset()
        {
            base.OnReset();

            _Ignores = null;

        }
        public override void Ignore(Transform tracer, ref List<RaycastHit> hits)
        {
            if (_Ignores is null)
                _Ignores = new();

            for(int i = hits.LastIndex(); i >= 0; i--)
            {
                Rigidbody rigidbody = hits[i].rigidbody;

                if (rigidbody)
                {
                    if (_IgnoreSelf && rigidbody.transform == tracer)
                    {
                        hits.RemoveAt(i);
                    }
                    else if (_Ignores.Contains(rigidbody.transform))
                    {
                        hits.RemoveAt(i);
                    }
                    else
                    {
                        _Ignores.Add(rigidbody.transform);
                    }
                }
                else
                {
                    if (_CheckHasRigidbody)
                        hits.RemoveAt(i);
                }
            }

            _Ignores.Clear();
        }
    }
}
