using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class CheckDistanceDecorator : TraceDecorator
    {
        [SerializeField] private float minDistance = 1f;
        protected new CheckDistanceDecorator _original;

        public override ITraceDecorator OnClone()
        {
            var clone = new CheckDistanceDecorator();

            clone._original = this;

            return clone;
        }
        protected override bool PerformConditionCheck(RaycastHit hitResult)
        {
            return hitResult.distance >= (_original is null ? minDistance : _original.minDistance);
        }
    }

}
