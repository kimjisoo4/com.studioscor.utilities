using System;
using System.Linq;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class CapsuleCast : TraceVariable
    {
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IPositionVariable _tracePoint1 = new LocalPositionVariable();

#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IPositionVariable _tracePoint2 = new LocalPositionVariable();

#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IDirectionVariable _traceDirection = new LocalDirectionVariable();

#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IFloatVariable _traceRadius = new DefaultFloatVariable(1f);

#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IFloatVariable _traceDistance = new DefaultFloatVariable(10f);

        [SerializeField] private Variable_LayerMask _traceLayer;


        protected new CapsuleCast _original;

        private Vector3 _direction;
        private LayerMask _layer;
        private float _radius;
        private float _distance;
        protected override void OnSetup()
        {
            base.OnSetup();

            _tracePoint1.Setup(Owner);
            _tracePoint2.Setup(Owner);
            _traceRadius.Setup(Owner);
            _traceDistance.Setup(Owner);
            _traceDirection.Setup(Owner);
        }

        protected override ITrace OnClone()
        {
            var clone = new CapsuleCast();

            clone._original = this;
            clone._tracePoint1 = _tracePoint1.Clone();
            clone._tracePoint2 = _tracePoint2.Clone();
            clone._traceRadius = _traceRadius.Clone();
            clone._traceDirection = _traceDirection.Clone();
            clone._traceDistance = _traceDistance.Clone();

            return clone;
        }
        protected override RaycastHit CreateHitResult()
        {
            Vector3 pointA = _tracePoint1.GetValue();

            var hitResult = new RaycastHit();

            hitResult.point = pointA + _direction * _distance;
            hitResult.distance = _distance;
            hitResult.normal = Vector3.up;

            return hitResult;
        }
        protected override void OnEnter()
        {
            base.OnEnter();

            _radius = _traceRadius.GetValue();
            _distance = _traceDistance.GetValue();
            _direction = _traceDirection.GetValue();
            _layer = _original is null ? _traceLayer.Value : _original._traceLayer.Value;

        }

        protected override int OnUpdate()
        {
            Vector3 pointA = _tracePoint1.GetValue();
            Vector3 pointB = _tracePoint2.GetValue();

            Vector3 point1 = pointA + pointA.Direction(pointB) * _radius;
            Vector3 point2 = pointB + pointB.Direction(pointA) * _radius;

            _traceInfo.TraceStart = (point1 + point2) * 0.5f;
            _traceInfo.TraceEnd = _traceInfo.TraceStart + (_direction * _distance);

            int hitCount = Physics.CapsuleCastNonAlloc(point1, point2, _radius, _direction, _hitResults, _distance, _layer);

            if (_debug)
            {
                if (hitCount > 0)
                {
                    Vector3 offset = point2 - point1;
                    Vector3 hitPoint1 = _hitResults.ElementAt(0).point;

                    SUtility.Debug.DrawCapsule(point1, point2, _radius, Color.green, 1f);
                    SUtility.Debug.DrawLine(point1 + offset * 0.5f, hitPoint1 + offset * 0.5f, Color.green, 1f);
                    SUtility.Debug.DebugPoint(hitPoint1, 1f, Color.green, 1f);
                }
                else
                {
                    Vector3 offset = point2 - point1;
                    Vector3 hitPoint1 = point1 + _direction * _distance;

                    SUtility.Debug.DrawCapsule(point1, point2, _radius, Color.red, 1f);
                    SUtility.Debug.DrawLine(point1 + offset * 0.5f, hitPoint1 + offset * 0.5f, Color.red, 1f);
                    SUtility.Debug.DrawCapsule(hitPoint1, hitPoint1 + offset, _radius, Color.red, 1f);
                }
            }

            return hitCount;
        }
    }



}
