using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public abstract class TraceSystem<TData> : BaseScriptableObject
    {
        [Header(" [ Trace System ] ")]
        [SerializeField] protected bool ignoreSelf = true;
        [SerializeField] protected IgnoreTrace[] ignores;
        [SerializeField] protected TraceAction[] actions;

        public bool Trace(Transform owner, TData data, ref List<RaycastHit> hits, List<Transform> ignoreHits, bool useDebug = false)
        {
            if (OnTrace(owner, data, ref hits, ignoreHits, UseDebug || useDebug))
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
            if (ignores is null || ignores.Length == 0)
                return;

            foreach (var ignoreHit in ignores)
            {
                ignoreHit.Ignore(owner, ref hits);
            }
        }
        protected void TryActions(Transform owner, ref List<RaycastHit> hits)
        {
            if (actions is null || actions.Length == 0)
                return;

            foreach (var action in actions)
            {
                action.Action(owner, ref hits);
            }
        }
    }
}
