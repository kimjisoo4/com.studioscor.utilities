﻿using System.Collections.Generic;
using UnityEngine;


namespace StudioScor.Utilities
{
    [System.Serializable]
    public class TrailSphereCast : BaseClass, ISphereCast
    {
        [Header(" [ Trail Sphere Cast ] ")]
        [SerializeField] private GameObject _owner;
        [SerializeField] private float _traceRadius = 1f;
        [SerializeField] private LayerMask _traceLayer;
        [SerializeField] private int _maxHitCount = 10;

        private bool _isPlaying = false;
        private int _hitCount = 0;
        private readonly List<Transform> _ignoreTransforms = new List<Transform>();
        private RaycastHit[] _hitResults = new RaycastHit[20];

        public Vector3 StartPosition => _startPosition;
        public Vector3 EndPosition => _prevEndPosition;

        public GameObject Owner => _owner;
        public bool IsPlaying => _isPlaying;
        public float TraceRadius { get => _traceRadius; set => _traceRadius = value; }
        public int MaxHitCount { get => _maxHitCount; set => _maxHitCount = value; }
        public LayerMask TraceLayer { get => _traceLayer; set => _traceLayer = value; }
        public IReadOnlyCollection<RaycastHit> HitResults => _hitResults;
        public IReadOnlyList<Transform> IgnoreTransforms => _ignoreTransforms;
        public int HitCount => _hitCount;

        private Vector3 _startPosition;
        private Vector3 _prevEndPosition;

        public event IRaycast.RaycastStateHandler OnStartedRaycast;
        public event IRaycast.RaycastStateHandler OnEndedRaycast;

        public void SetOwner(GameObject newOwner)
        {
            _owner = newOwner;
        }

        public void OnTrace()
        {
            if (_isPlaying)
                return;

            if(_hitResults is null || _hitResults.Length != _maxHitCount)
            {
                _hitResults = new RaycastHit[_maxHitCount];
            }

            _isPlaying = true;

            _hitCount = 0;
            _prevEndPosition = _owner.transform.position;
            AddIgnoreTransform(Owner.transform);

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

        public void AddIgnoreTransform(Transform ignoreTransform)
        {
            _ignoreTransforms.Add(ignoreTransform);
        }


        public (int hitCount, RaycastHit[] raycastHits) UpdateTrace()
        {
            if (!_isPlaying)
                return (0, null);

            _startPosition = _prevEndPosition;
            var endPosition = _owner.transform.position;

            _prevEndPosition = endPosition;

            _hitCount = SUtility.Physics.DrawSphereCastAllNonAlloc(_startPosition, endPosition, _traceRadius, _hitResults, _traceLayer, QueryTriggerInteraction.UseGlobal, UseDebug);

            int removeCount = 0;

            if(_hitCount > 0)
            {
                for (int i = 0; i < _hitCount; i++)
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
                                hitResult.point = hitResult.collider.ClosestPoint(_startPosition);
                            }

                            hitResult.distance = Vector3.Distance(hitResult.point, _startPosition);
                            hitResult.normal = hitResult.point.Direction(_startPosition);
                        }

                        _hitResults[i - removeCount] = hitResult;
                    }
                    else
                    {
                        removeCount++;
                    }
                }
            }

            _hitCount -= removeCount;

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
