using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public abstract class TraceComponent : BaseMonoBehaviour
    {
        [Header(" [ Trace Component ] ")]
        [SerializeField] protected Transform _Owner;
        [SerializeField] protected bool _IgnoreSelf = true;

        [SerializeField] protected Vector3 _Offset;
        [SerializeField] protected LayerMask _Layer;
        [SerializeField] protected TraceIgnore[] _TraceIgnores;

        [Header(" [ Event ] ")]
        [SerializeField] protected UnityEvent<List<RaycastHit>> _OnHits;
        public UnityAction<List<RaycastHit>> OnHits;

        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool _AutoPlaying = true;

        protected bool _IsPlaying = false;
        protected Vector3 _PrevPosition;
        protected List<RaycastHit> _Hits = new();
        protected List<Transform> _IgnoreTransforms = new();

        public IReadOnlyList<RaycastHit> Hits => _Hits;

        private void Reset()
        {
#if UNITY_EDITOR
            _Owner = transform;
#endif
        }

        private void OnEnable()
        {
            if (_AutoPlaying)
                OnTrace();
        }
        private void OnDisable()
        {
            EndTrace();
        }

        public void SetOwner(Component component)
        {
            SetOwner(component.transform);
        }
        public void SetOwner(GameObject owner)
        {
            SetOwner(owner.transform);
        }
        public void SetOwner(Transform transform)
        {
            _Owner = transform;
        }

        public void AddIgnoreTransforms(Transform transform)
        {
            _IgnoreTransforms.Add(transform);
        }
        public void AddIgnoreTransforms(IEnumerable<Transform> transforms)
        {
            _IgnoreTransforms.AddRange(transforms);
        }

        public void RemoveIgnoreTransforms(Transform transform)
        {
            _IgnoreTransforms.Remove(transform);
        }
        public void RemoveIgnoreTransforms(IEnumerable<Transform> transforms)
        {
            foreach (var remove in transforms)
            {
                _IgnoreTransforms.Remove(remove);
            }
        }

        public virtual Vector3 CalcPosition()
        {
            return _Owner.TransformPoint(_Offset);
        }


        private void FixedUpdate()
        {
            UpdateTrace();
        }

        public void OnTrace()
        {
            if (_IsPlaying)
                return;

            _IsPlaying = true;

            if (_IgnoreSelf)
                _IgnoreTransforms.Add(_Owner);

            _PrevPosition = CalcPosition();
        }
        public void EndTrace()
        {
            if (!_IsPlaying)
                return;

            _IsPlaying = false;

            _IgnoreTransforms.Clear();
            _Hits.Clear();
        }

        public bool UpdateTrace()
        {
            _Hits.Clear();

            if (TryTrace())
            {
                Callback_OnHits();
                
                return true;
            }
            else
            {
                return false;
            }
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
