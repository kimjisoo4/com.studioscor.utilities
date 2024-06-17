using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class RayCastVariable : TraceVariable
    {
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IPositionVariable _tracePosition = new LocalPositionVariable();
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IDirectionVariable _traceDirection = new LocalDirectionVariable();

#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IFloatVariable _traceDistance = new DefaultFloatVariable(5);
        [SerializeField] private Variable_LayerMask _traceLayer;

        protected new RayCastVariable _original;

        protected override RaycastHit CreateHitResult()
        {
            Vector3 direction = _traceDirection.GetValue();
            float distacne = _traceDistance.GetValue();

            var hit = new RaycastHit();

            hit.normal = Vector3.up;
            hit.distance = distacne;
            hit.point = direction * distacne;

            return hit;
        }

        protected override ITrace OnClone()
        {
            var clone = new RayCastVariable();

            clone._original = this;

            return clone;
        }

        protected override int OnUpdate()
        {
            Vector3 position = _tracePosition.GetValue();
            Vector3 direction = _traceDirection.GetValue();
            float distance = _traceDistance.GetValue();

            _traceInfo.TraceStart = position;
            _traceInfo.TraceEnd = position + (direction * distance);

            return SUtility.Physics.DrawRayCastNonAlloc(position, direction, _hitResults, distance, _traceLayer.Value, QueryTriggerInteraction.UseGlobal, _useDebug);
        }
    }



}
