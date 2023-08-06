using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/TraceSystem/Ignore/new Ignore Group", fileName = "IgnoreTrace_Group_")]
    public class IgnoreTrace_Group : IgnoreTrace
    {
        [Header(" [ Ignore Trace Group ] ")]
        [SerializeField] private IgnoreTrace[] ignoreTraces;
        public override void Ignore(Transform tracer, ref List<RaycastHit> hits)
        {
            foreach (var ignore in ignoreTraces)
            {
                ignore.Ignore(tracer, ref hits);
            }
        }
    }
}
