using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace StudioScor.Utilities
{
    public class TrailSphereCastComponent : BaseMonoBehaviour, ISphereCast
    {
        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField] private UnityEvent _onStartedRaycast;
            [SerializeField] private UnityEvent _onEndedRaycast;

            public void AddUnityEvents(ISphereCast sphereCast)
            {
                sphereCast.OnStartedRaycast += SphereCast_OnStartedRaycast;
                sphereCast.OnEndedRaycast += SphereCast_OnEndedRaycast;
            }
            public void RemoveUnityEvents(ISphereCast sphereCast)
            {
                sphereCast.OnStartedRaycast -= SphereCast_OnStartedRaycast;
                sphereCast.OnEndedRaycast -= SphereCast_OnEndedRaycast;
            }

            private void SphereCast_OnStartedRaycast(IRaycast raycast)
            {
                _onStartedRaycast?.Invoke();
            }
            private void SphereCast_OnEndedRaycast(IRaycast raycast)
            {
                _onEndedRaycast?.Invoke();
            }

        }
        [Header(" [ Trail Sphere Cast ] ")]
        [SerializeField] private TrailSphereCast _trailSphereCast;

        [Header(" Unity Event ")]
        [SerializeField] private bool _useUnityEvent;
        [SerializeField]private UnityEvents _unityEvents;

#if UNITY_EDITOR
        [Header(" Debug ")]
        [SerializeField] private bool _useAlwaysDrawGizmos = false;
        [SerializeField] private bool _isWire = true;
        [SerializeField] private Color _gizmosColor = Color.red;
#endif

        public GameObject Owner => _trailSphereCast.Owner;
        public bool IsPlaying => _trailSphereCast.IsPlaying;
        public float TraceRadius { get => _trailSphereCast.TraceRadius; set => _trailSphereCast.TraceRadius = value; }
        public int MaxHitCount { get => _trailSphereCast.MaxHitCount; set => _trailSphereCast.MaxHitCount = value; }
        public LayerMask TraceLayer { get => _trailSphereCast.TraceLayer; set => _trailSphereCast.TraceLayer = value; }
        public int HitCount => _trailSphereCast.HitCount;
        public IReadOnlyCollection<RaycastHit> HitResults => _trailSphereCast.HitResults; 
        public IReadOnlyList<Transform> IgnoreTransforms => _trailSphereCast.IgnoreTransforms;
        public Vector3 StartPosition => _trailSphereCast.StartPosition;
        public Vector3 EndPosition => _trailSphereCast.EndPosition;

        private bool _wasInitialized = false;


        public event IRaycast.RaycastStateHandler OnStartedRaycast { add => _trailSphereCast.OnStartedRaycast += value; remove => _trailSphereCast.OnStartedRaycast -= value; }
        public event IRaycast.RaycastStateHandler OnEndedRaycast { add => _trailSphereCast.OnEndedRaycast += value; remove => _trailSphereCast.OnEndedRaycast -= value; }

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
            if(!_useAlwaysDrawGizmos)
                DrawGizmos();
#endif
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void DrawGizmos()
        {
#if UNITY_EDITOR

            if (_trailSphereCast is null)
                return;

            var matrix = Gizmos.matrix;

            matrix.SetTRS(transform.position, transform.rotation, transform.localScale);

            Gizmos.matrix = matrix;

            Gizmos.color = _gizmosColor;

            if(_isWire)
                Gizmos.DrawWireSphere(Vector3.zero, _trailSphereCast.TraceRadius);
            else
                Gizmos.DrawSphere(Vector3.zero, _trailSphereCast.TraceRadius);
#endif
        }

        private void Awake()
        {
            Initialization();
        }
        private void OnDestroy()
        {
            if(_useUnityEvent)
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
            _trailSphereCast.AddIgnoreTransform(transform);
        }

        public void SetOwner(GameObject owner)
        {
            _trailSphereCast.SetOwner(owner);
        }
        public void OnTrace()
        {
            _trailSphereCast.OnTrace();
        }
        public void EndTrace()
        {
            _trailSphereCast.EndTrace();
        }

        public (int hitCount, RaycastHit[] raycastHits) UpdateTrace()
        {
            return _trailSphereCast.UpdateTrace();
        }
    }
}
