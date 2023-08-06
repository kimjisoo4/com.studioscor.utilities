using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;


namespace StudioScor.Utilities
{
    public abstract class TraceComponent : BaseMonoBehaviour
    {
        [Header(" [ Trace Component ] ")]
        [SerializeField] protected Transform owner;
        [SerializeField] protected bool ignoreSelf = true;

        [SerializeField] protected Vector3 offset;
        [SerializeField] protected LayerMask layer;
        [SerializeField] protected IgnoreTrace[] traceIgnores;

        [Header(" [ Event ] ")]
        [SerializeField] protected UnityEvent<List<RaycastHit>> onHits;
        
        public event UnityAction<List<RaycastHit>> OnHits;

        [Header(" [ Auto Playing ] ")]
        [SerializeField] private bool isAutoPlaying = true;

        protected bool isPlaying = false;
        protected Vector3 prevPosition;
        protected List<RaycastHit> hits = new();
        protected List<Transform> ignoreTransforms = new();

        public IReadOnlyList<RaycastHit> Hits => hits;

        private void Reset()
        {
#if UNITY_EDITOR
            owner = transform;
#endif
        }

        private void OnEnable()
        {
            if (isAutoPlaying)
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
            owner = transform;
        }

        public void AddIgnoreTransforms(Transform transform)
        {
            ignoreTransforms.Add(transform);
        }
        public void AddIgnoreTransforms(IEnumerable<Transform> transforms)
        {
            ignoreTransforms.AddRange(transforms);
        }

        public void RemoveIgnoreTransforms(Transform transform)
        {
            ignoreTransforms.Remove(transform);
        }
        public void RemoveIgnoreTransforms(IEnumerable<Transform> transforms)
        {
            foreach (var remove in transforms)
            {
                ignoreTransforms.Remove(remove);
            }
        }

        public virtual Vector3 CalcPosition()
        {
            return owner.TransformPoint(offset);
        }


        private void FixedUpdate()
        {
            UpdateTrace();
        }

        public void OnTrace()
        {
            if (isPlaying)
                return;

            isPlaying = true;

            if (ignoreSelf)
                ignoreTransforms.Add(owner);

            prevPosition = CalcPosition();
        }
        public void EndTrace()
        {
            if (!isPlaying)
                return;

            isPlaying = false;

            ignoreTransforms.Clear();
            hits.Clear();
        }

        public bool UpdateTrace()
        {
            hits.Clear();

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

            onHits?.Invoke(hits);
            OnHits?.Invoke(hits);
        }
    }
}
