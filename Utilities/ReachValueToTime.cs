using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class ReachValueToTime
    {
        [Header(" [ Reach Value To Time] ")]
        [SerializeField] private float _Distance = 5f;
        [SerializeField] private AnimationCurve _Curve;

        public float Distance => _Distance;
        public AnimationCurve Curve => _Curve;
        private float _PrevDistance = 0f;
        private bool _IsPlaying = false;

        public ReachValueToTime(float distance, AnimationCurve curve)
        {
            _Distance = distance;
            _Curve = curve;
        }

        public void OnMovement()
        {
            _PrevDistance = 0f;

            if (_Distance != 0f)
            {
                _IsPlaying = true;
            }
        }
        public void OnMovement(float distance)
        {
            _Distance = distance;

            OnMovement();
        }
        public void OnMovement(float distance, AnimationCurve curve)
        {
            _Distance = distance;
            _Curve = curve;

            OnMovement();
        }
        public void SetDistance(float distance)
        {
            _Distance = distance;
        }
        public float UpdateMovement(float normalizeTime)
        {
            if (!_IsPlaying)
                return 0f;

            float distance = Curve.Evaluate(normalizeTime) * Distance;

            float currentDistance = distance - _PrevDistance;

            _PrevDistance = distance;

            if (normalizeTime >= 1f)
            {
                _IsPlaying = false;
            }

            return currentDistance;
        }
    }
}