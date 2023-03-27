using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/TraceSystem/Ignore/new Ignore Group", fileName = "Trace_Ignore_Group_")]
    public class TraceIgnore_Group : TraceIgnore
    {
        [Header(" [ Trace Ignores ] ")]
        [SerializeField] private TraceIgnore[] _Ignores;
        public override void Ignore(Transform tracer, ref List<RaycastHit> hits)
        {
            foreach (var ignore in _Ignores)
            {
                ignore.Ignore(tracer, ref hits);
            }
        }
    }
}
