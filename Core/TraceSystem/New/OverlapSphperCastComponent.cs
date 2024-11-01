using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace StudioScor.Utilities
{
    public class OverlapSphperCastComponent : BaseMonoBehaviour, IOverlapSphere
    {
        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField] private UnityEvent _onStartedRaycast;
            [SerializeField] private UnityEvent _onEndedRaycast;

            public void AddUnityEvents(IOverlapSphere overlapSphere)
            {
                overlapSphere.OnStartedRaycast += OverlapSphere_OnStartedRaycast;
                overlapSphere.OnEndedRaycast += OverlapSphere_OnEndedRaycast;
            }
            public void RemoveUnityEvents(IOverlapSphere overlapSphere)
            {
                overlapSphere.OnStartedRaycast -= OverlapSphere_OnStartedRaycast;
                overlapSphere.OnEndedRaycast -= OverlapSphere_OnEndedRaycast;
            }

            private void OverlapSphere_OnStartedRaycast(IRaycast raycast)
            {
                _onStartedRaycast?.Invoke();
            }
            private void OverlapSphere_OnEndedRaycast(IRaycast raycast)
            {
                _onEndedRaycast?.Invoke();
            }

        }
        [Header(" [ Overlap Sphere Cast ] ")]
        [SerializeField] private OverlapSphereCast _overlapSphereCast;

        [Header(" Unity Event ")]
        [SerializeField] private bool _useUnityEvent;
        [SerializeField] private UnityEvents _unityEvents;

#if UNITY_EDITOR
        [Header(" Debug ")]
        [SerializeField] private bool _useAlwaysDrawGizmos = false;
        [SerializeField] private bool _isWire = true;
        [SerializeField] private Color _gizmosColor = Color.red;
#endif

        public GameObject Owner => _overlapSphereCast.Owner;
        public bool IsPlaying => _overlapSphereCast.IsPlaying;
        public float TraceRadius { get => _overlapSphereCast.TraceRadius; set => _overlapSphereCast.TraceRadius = value; }
        public int MaxHitCount { get => _overlapSphereCast.MaxHitCount; set => _overlapSphereCast.MaxHitCount = value; }
        public LayerMask TraceLayer { get => _overlapSphereCast.TraceLayer; set => _overlapSphereCast.TraceLayer = value; }
        public int HitCount => _overlapSphereCast.HitCount;
        public IReadOnlyCollection<RaycastHit> HitResults => _overlapSphereCast.HitResults;
        public IReadOnlyList<Transform> IgnoreTransforms => _overlapSphereCast.IgnoreTransforms;

        public bool UseInLocalSpace { get => _overlapSphereCast.UseInLocalSpace; set => _overlapSphereCast.UseInLocalSpace = value; }
        public Vector3 TracePosition { get => _overlapSphereCast.TracePosition; set => _overlapSphereCast.TracePosition = value; }

        private bool _wasInitialized = false;


        public event IRaycast.RaycastStateHandler OnStartedRaycast { add => _overlapSphereCast.OnStartedRaycast += value; remove => _overlapSphereCast.OnStartedRaycast -= value; }
        public event IRaycast.RaycastStateHandler OnEndedRaycast { add => _overlapSphereCast.OnEndedRaycast += value; remove => _overlapSphereCast.OnEndedRaycast -= value; }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (_useAlwaysDrawGizmos)
                DrawGizmos();
#endif
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!_useAlwaysDrawGizmos)
                DrawGizmos();
#endif
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void DrawGizmos()
        {
#if UNITY_EDITOR

            if (_overlapSphereCast is null)
                return;

            var matrix = Gizmos.matrix;

            matrix.SetTRS(transform.position, transform.rotation, transform.localScale);

            Gizmos.matrix = matrix;

            Gizmos.color = _gizmosColor;

            if (_isWire)
                Gizmos.DrawWireSphere(Vector3.zero, _overlapSphereCast.TraceRadius);
            else
                Gizmos.DrawSphere(Vector3.zero, _overlapSphereCast.TraceRadius);
#endif
        }

        private void Awake()
        {
            Initialization();
        }
        private void OnDestroy()
        {
            if (_useUnityEvent)
            {
                _unityEvents.RemoveUnityEvents(this);
            }
        }

        private void Initialization()
        {
            if (_wasInitialized)
                return;

            _wasInitialized = true;

            if (_useUnityEvent)
            {
                _unityEvents.AddUnityEvents(this);
            }
        }

        public void AddIgnoreTransform(Transform transform)
        {
            _overlapSphereCast.AddIgnoreTransform(transform);
        }

        public void SetOwner(GameObject owner)
        {
            _overlapSphereCast.SetOwner(owner);
        }
        public void OnTrace()
        {
            _overlapSphereCast.OnTrace();
        }
        public void EndTrace()
        {
            _overlapSphereCast.EndTrace();
        }

        public (int hitCount, RaycastHit[] raycastHits) UpdateTrace()
        {
            return _overlapSphereCast.UpdateTrace();
        }
    }
}
