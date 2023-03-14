using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public abstract class TraceSystem<TData> : BaseScriptableObject
    {
        [Header(" [ Trace System ] ")]
        [SerializeField] protected bool _IgnoreSelf = true;
        [SerializeField] protected TraceIgnore[] _Ignores;
        [SerializeField] protected TraceAction[] _Actions;

        public bool Trace(Transform owner, TData data, ref List<RaycastHit> hits, List<Transform> ignoreHits, bool useDebug = false)
        {
            if (_IgnoreSelf)
                ignoreHits.Add(owner);

            if (OnTrace(owner, data, ref hits, ignoreHits, useDebug))
            {
                CheckIgnore(owner, ref hits);
                TryActions(owner, ref hits);

                return hits.Count > 0;
            }

            return false;
        }

        protected abstract bool OnTrace(Transform owner, TData data, ref List<RaycastHit> hits, List<Transform> ignoreHits, bool useDebug = false);
        protected void CheckIgnore(Transform owner, ref List<RaycastHit> hits)
        {
            if (_Ignores is null || _Ignores.Length == 0)
                return;

            foreach (var ignoreHit in _Ignores)
            {
                ignoreHit.Ignore(owner, ref hits);
            }
        }
        protected void TryActions(Transform owner, ref List<RaycastHit> hits)
        {
            if (_Actions is null || _Actions.Length == 0)
                return;

            foreach (var action in _Actions)
            {
                action.Action(owner, ref hits);
            }
        }

            
    }
}
