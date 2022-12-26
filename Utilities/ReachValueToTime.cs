using UnityEngine;

namespace StudioScor.Utilities
{
    public class ReachValueToTime
    {
        public float Distance;
        public AnimationCurve Curve;
        private float _PrevDistance;

        public ReachValueToTime(float distance, AnimationCurve curve)
        {
            Distance = distance;
            Curve = curve;
        }

        public void OnMovement()
        {
            _PrevDistance = 0f;
        }
        public void OnMovement(float distance)
        {
            Distance = distance;

            OnMovement();
        }
        public void OnMovement(float distance, AnimationCurve curve)
        {
            Distance = distance;
            Curve = curve;

            OnMovement();
        }
        public float UpdateMovement(float normalizeTime)
        {
            float distance = Curve.Evaluate(normalizeTime) * Distance;

            float currentDistance = distance - _PrevDistance;

            _PrevDistance = distance;

            return currentDistance;
        }
    }
}