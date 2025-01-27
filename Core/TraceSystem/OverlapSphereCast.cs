using System.Collections.Generic;
using UnityEngine;


namespace StudioScor.Utilities
{
    [System.Serializable]
    public class OverlapSphereCast : BaseClass, IOverlapSphere
    {
        [Header(" [ Overlap Sphere Cast ] ")]
        [SerializeField] private GameObject _owner;
        [SerializeField] private Vector3 _tracePosition = Vector3.zero;
        [SerializeField] private bool _useInLocalSpace = true;
        [SerializeField] private float _traceRadius = 1f;
        [SerializeField] private LayerMask _traceLayer;
        [SerializeField] private int _maxHitCount = 10;

        private bool _isPlaying = false;
        private int _hitCount = 0;
        private readonly List<Transform> _ignoreTransforms = new List<Transform>();
        private RaycastHit[] _hitResults;

        public GameObject Owner => _owner;
        public bool IsPlaying => _isPlaying;
        public Vector3 TracePosition { get => _tracePosition; set => _tracePosition = value; }
        public bool UseInLocalSpace { get => _useInLocalSpace; set => _useInLocalSpace = value; }
        public float TraceRadius { get => _traceRadius; set => _traceRadius = value; }
        public int MaxHitCount { get => _maxHitCount; set => _maxHitCount = value; }
        public LayerMask TraceLayer { get => _traceLayer; set => _traceLayer = value; }
        public int HitCount => _hitCount;
        public IReadOnlyCollection<RaycastHit> HitResults => _hitResults;
        public IReadOnlyList<Transform> IgnoreTransforms => _ignoreTransforms;


        public event IRaycast.RaycastStateHandler OnStartedRaycast;
        public event IRaycast.RaycastStateHandler OnEndedRaycast;

        public void SetOwner(GameObject owner)
        {
            _owner = owner;
        }

        public void AddIgnoreTransform(Transform transform)
        {
            _ignoreTransforms.Add(transform);
        }

        public void OnTrace()
        {
            if (IsPlaying)
                return;

            if(_hitResults is null || _hitResults.Length != _maxHitCount)
            {
                _hitResults = new RaycastHit[_maxHitCount];
            }

            _isPlaying = true;

            _hitCount = 0;

            Invoke_OnStartedRaycast();
        }

        public void EndTrace()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;

            _ignoreTransforms.Clear();

            Invoke_OnEndedRaycast();
        }

        public (int hitCount, RaycastHit[] raycastHits) UpdateTrace()
        {
            if (!_isPlaying)
                return (0, null);

            Vector3 center = UseInLocalSpace ? _owner.transform.TransformPoint(TracePosition) : TracePosition;

            _hitCount = SUtility.Physics.DrawSphereCastAllNonAlloc(center, center, TraceRadius, _hitResults, TraceLayer, QueryTriggerInteraction.UseGlobal, UseDebug);

            if(_hitCount > 0)
            {
                int removeCount = 0;

                for(int i = 0; i < _hitCount; i++)
                {
                    var hitResult = _hitResults[i];

                    if (!_ignoreTransforms.Contains(hitResult.transform))
                    {
                        AddIgnoreTransform(hitResult.transform);

                        if (hitResult.distance.SafeEquals(0f))
                        {
                            if (hitResult.collider is MeshCollider meshCollider)
                            {
                                hitResult.point = meshCollider.bounds.center;
                            }
                            else
                            {
                                hitResult.point = hitResult.collider.ClosestPoint(center);
                            }

                            hitResult.distance = Vector3.Distance(hitResult.point, center);
                            hitResult.normal = hitResult.point.Direction(center);
                        }

                        _hitResults[i - removeCount] = hitResult;

                    }
                    else
                    {
                        removeCount++;
                    }
                    
                }

                _hitCount -= removeCount;
            }

            
            return (_hitCount, _hitResults);
        }

        private void Invoke_OnStartedRaycast()
        {
            Log($"{nameof(OnStartedRaycast)}");

            OnStartedRaycast?.Invoke(this);
        }
        private void Invoke_OnEndedRaycast()
        {
            Log($"{nameof(OnEndedRaycast)}");

            OnEndedRaycast?.Invoke(this);
        }
    }
}
