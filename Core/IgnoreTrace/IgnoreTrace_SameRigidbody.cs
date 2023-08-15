using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{

    [CreateAssetMenu(menuName ="StudioScor/TraceSystem/Ignore/new Ignore Same Rigidbody", fileName = "IgnoreTrace_SameRigidbody")]
    public class IgnoreTrace_SameRigidbody : IgnoreTrace
    {
        [Header(" [ Ignore Trace Same Rigidbody ] ")]
        [SerializeField] private bool ignoreSelf = true;
        [SerializeField] private bool checkHasRigidbody = true;

        private readonly List<Transform> ignores = new();

        protected override void OnReset()
        {
            base.OnReset();

            ignores.Clear();

        }
        public override void Ignore(Transform tracer, ref List<RaycastHit> hits)
        {
            ignores.Clear();

            for (int i = hits.LastIndex(); i >= 0; i--)
            {
                Rigidbody rigidbody = hits[i].rigidbody;

                if (rigidbody)
                {
                    if (ignoreSelf && rigidbody.transform == tracer)
                    {
                        hits.RemoveAt(i);
                    }
                    else if (ignores.Contains(rigidbody.transform))
                    {
                        hits.RemoveAt(i);
                    }
                    else
                    {
                        ignores.Add(rigidbody.transform);
                    }
                }
                else
                {
                    if (checkHasRigidbody)
                        hits.RemoveAt(i);
                }
            }

            ignores.Clear();
        }
    }
}
