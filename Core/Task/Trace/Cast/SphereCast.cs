using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class MultiLineCast : TraceVariable
    {
        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReferenceDropdown]
#endif
        private IPositionVariable _tracePointA = new LocalPositionVariable();

        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReferenceDropdown]
#endif
        private IPositionVariable _tracePointB = new LocalPositionVariable();

        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReferenceDropdown]
#endif
        private IIntegerVariable _traceCount = new DefaultIntegerVariable();

        [SerializeField] private Variable_LayerMask _traceLayer;

        private Vector3 _prevPositionA;
        private Vector3 _prevPositionB;
        
        protected new MultiLineCast _original;

        private LayerMask _layer;
        private RaycastHit[] _subHitResults = new RaycastHit[10];

        protected override ITrace OnClone()
        {
            var clone = new MultiLineCast();

            clone._original = this;

            clone._tracePointA = _tracePointA.Clone();
            clone._tracePointB = _tracePointB.Clone();
            clone._traceCount = _traceCount.Clone();

            return clone;
        }

        protected override void OnSetup()
        {
            base.OnSetup();

            _tracePointA.Setup(Owner);
            _tracePointB.Setup(Owner);
            _traceCount.Setup(Owner);
        }
        protected override RaycastHit CreateHitResult()
        {
            return default;
        }

        protected override void OnEnter()
        {
            base.OnEnter();

            _prevPositionA = _tracePointA.GetValue();
            _prevPositionB = _tracePointB.GetValue();

            _layer = _original is null ? _traceLayer.Value : _original._traceLayer.Value;

            if (_subHitResults is null || _subHitResults.Length != _size)
                _subHitResults = new RaycastHit[_size];
        }
        protected override int OnUpdate()
        {
            Vector3 startPositionA = _prevPositionA;
            Vector3 startPositionB = _prevPositionB;

            Vector3 endPositionA = _tracePointA.GetValue();
            Vector3 endPositionB = _tracePointB.GetValue();

            _prevPositionA = endPositionA;
            _prevPositionB = endPositionB;

            _traceInfo.TraceStart = (startPositionA + startPositionB) * 0.5f;
            _traceInfo.TraceEnd = (endPositionA + endPositionB) * 0.5f;

            int traceCount = _traceCount.GetValue();
            float interval = 1f / (traceCount - 1);
            int prevHitCount = 0;

            for (int traceIndex = 0; traceIndex < traceCount; traceIndex++)
            {
                Vector3 start = Vector3.Lerp(startPositionA, startPositionB, interval * traceIndex);
                Vector3 end = Vector3.Lerp(endPositionA, endPositionB, interval * traceIndex);
                
                var hitCount = SUtility.Physics.DrawLineCastNonAlloc(start, end, _subHitResults, _layer, QueryTriggerInteraction.Collide, _debug);

                for(int resultIndex = 0; resultIndex < hitCount; resultIndex++)
                {
                    _hitResults[prevHitCount + resultIndex] = _subHitResults[resultIndex];
                }

                prevHitCount = hitCount;
            }

            return prevHitCount;
        }
    }
    [Serializable]
    public class SphereCast : TraceVariable
    {
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IPositionVariable _tracePosition = new LocalPositionVariable();

#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IFloatVariable _traceRadius = new DefaultFloatVariable(1f);

        [SerializeField] private Variable_LayerMask _traceLayer;

        private Vector3 _prevPosition;
        protected new SphereCast _original;
        private LayerMask _layer;
        private float _radius;

        protected override void OnSetup()
        {
            base.OnSetup();

            _tracePosition.Setup(Owner);
            _traceRadius.Setup(Owner);
        }
        protected override ITrace OnClone()
        {
            var clone = new SphereCast();

            clone._original = this;
            clone._tracePosition = _tracePosition.Clone();
            clone._traceRadius = _traceRadius.Clone();

            return clone;
        }
        protected override void OnEnter()
        {
            base.OnEnter();

            _prevPosition = _tracePosition.GetValue();
            _radius = _traceRadius.GetValue();
            _layer = _original is null ? _traceLayer.Value : _original._traceLayer.Value;
        }
        protected override int OnUpdate()
        {
            Vector3 startPosition = _prevPosition;
            Vector3 endPosition = _tracePosition.GetValue();

            _prevPosition = endPosition;

            _traceInfo.TraceStart = startPosition;
            _traceInfo.TraceEnd = endPosition;

            int hitCount = SUtility.Physics.DrawSphereCastAllNonAlloc(startPosition, endPosition, _radius, _hitResults, _layer, useDebug: _debug);

            return hitCount;
        }

        protected override RaycastHit CreateHitResult()
        {
            return default;
        }
    }



}
