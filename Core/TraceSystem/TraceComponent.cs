using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public abstract class TraceComponent : BaseStateMono
    {
        [Header(" [ Trace Component ] ")]
        [SerializeField] protected Transform _Transform;

        [SerializeField] protected Vector3 _Offset;
        [SerializeField] protected LayerMask _Layer;
        [SerializeField] protected TraceIgnore[] _TraceIgnores;

        [Header(" [ Evemt ] ")]
        [SerializeField] protected UnityEvent<List<RaycastHit>> _OnHits;
        public UnityAction<List<RaycastHit>> OnHits;

        protected Vector3 _PrevPosition;
        protected List<RaycastHit> _Hits = new();
        protected List<Transform> _IgnoreTransforms = new();

        public override bool CanEnterState()
        {
            if (!base.CanEnterState())
                return false;

            if (!_Transform)
                return false;

            return true;
        }

        public abstract Vector3 CalcPosition();

        protected override void EnterState()
        {
            _PrevPosition = CalcPosition();

            OnTrace();
        }

        protected override void ExitState()
        {
            _IgnoreTransforms.Clear();
            _Hits.Clear();
        }

        private void FixedUpdate()
        {
            OnTrace();
        }

        public void OnTrace()
        {
            _Hits.Clear();

            if (TryTrace())
                Callback_OnHits();
        }

        protected abstract bool TryTrace();

        private void Callback_OnHits()
        {
            Log(" On Hits ");

            _OnHits?.Invoke(_Hits);
            OnHits?.Invoke(_Hits);
        }
    }
}
