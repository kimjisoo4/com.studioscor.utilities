using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class ReachValueToTime
    {
        [Header(" [ Reach Value To Time] ")]
        [SerializeField] private float _distance = 5f;
        [SerializeField] private AnimationCurve _curve;

        private bool _isPlaying = false;
        private float _prevDistance = 0f;
        private float _deltaDistance = 0f;

        public float DeltaDistance => _deltaDistance;
        public float Distance => _distance;
        public float RemainDistance => Distance - _prevDistance;
        public AnimationCurve Curve => _curve;
        public bool IsPlaying => _isPlaying;

        private bool _needUpdate = false;
        private float _tempDistance;
        private AnimationCurve _tempCurve;

        public ReachValueToTime()
        {

        }
        public ReachValueToTime(float distance, AnimationCurve curve)
        {
            _distance = distance;
            _curve = curve;
        }

        public void Copy(ReachValueToTime reachValueToTime)
        {
            _distance = reachValueToTime._distance;
            _curve = reachValueToTime._curve;
            _prevDistance = reachValueToTime._prevDistance;
            _deltaDistance = reachValueToTime._deltaDistance;
        }
        public void SetDistance(float distance)
        {
            if (IsPlaying)
            {
                _tempDistance = distance;

                _needUpdate = true;
            }
            else
            {
                _distance = distance;
            }
        }
        public void SetCurve(AnimationCurve curve)
        {
            if (IsPlaying)
            {
                _tempCurve = curve;

                _needUpdate = true;
            }
            else
            {
                _curve = curve;
            }
        }


        public void OnMovement(float distance, AnimationCurve curve)
        {
            if (_isPlaying || distance.SafeEquals(0f))
                return;

            _distance = distance;
            _curve = curve;
            _prevDistance = 0f;
            _deltaDistance = 0f;

            _isPlaying = true;
        }
        public void OnMovement(float distance)
        {
            if (_isPlaying || distance.SafeEquals(0f))
                return;

            _distance = distance; 
            _prevDistance = 0f;
            _deltaDistance = 0f;

            _isPlaying = true;
        }

        public void OnMovement()
        {
            if (_isPlaying || _distance.SafeEquals(0f))
                return;

            _prevDistance = 0f;
            _deltaDistance = 0f;

            _isPlaying = true;
        }

        public void EndMovement()
        {
            if (!IsPlaying)
                return;

            _isPlaying = false;

            if(_needUpdate)
            {
                _distance = _tempDistance;
                _curve = _tempCurve;
            }
        }

        public float UpdateMovement(float normalizeTime)
        {
            if (!_isPlaying)
                return 0f;

            float distance;
            
            if(Curve is null)
            {
                distance = normalizeTime * Distance;
            }
            else
            {
                distance = Curve.Evaluate(normalizeTime) * Distance;
            }
            

            _deltaDistance = distance - _prevDistance;
            _prevDistance = distance;

            if (normalizeTime >= 1f)
            {
                EndMovement();
            }

            return _deltaDistance;
        }
    }
}