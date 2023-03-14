using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class ReachValueToTime
    {
        [Header(" [ Reach Value To Time] ")]
        [SerializeField] private float _Distance = 5f;
        [SerializeField] private AnimationCurve _Curve;

        private bool _IsPlaying = false;
        private float _PrevDistance = 0f;

        public float Distance => _Distance;
        public AnimationCurve Curve => _Curve;
        public bool IsPlaying => _IsPlaying;

        private bool _NeedUpdate = false;
        private float _TempDistance;
        private AnimationCurve _TempCurve;

        public ReachValueToTime()
        {

        }
        public ReachValueToTime(float distance, AnimationCurve curve)
        {
            _Distance = distance;
            _Curve = curve;
        }

        public void Copy(ReachValueToTime reachValueToTime)
        {
            _Distance = reachValueToTime._Distance;
            _Curve = reachValueToTime._Curve;
            _PrevDistance = reachValueToTime._PrevDistance;
        }
        public void SetDistance(float distance)
        {
            if (IsPlaying)
            {
                _TempDistance = distance;

                _NeedUpdate = true;
            }
            else
            {
                _Distance = distance;
            }
        }
        public void SetCurve(AnimationCurve curve)
        {
            if (IsPlaying)
            {
                _TempCurve = curve;

                _NeedUpdate = true;
            }
            else
            {
                _Curve = curve;
            }
        }


        public void OnMovement(float distance, AnimationCurve curve)
        {
            if (_IsPlaying || distance.SafeEquals(0f))
                return;

            _Distance = distance;
            _Curve = curve;
            _PrevDistance = 0f;

            _IsPlaying = true;
        }
        public void OnMovement(float distance)
        {
            if (_IsPlaying || distance.SafeEquals(0f))
                return;

            _Distance = distance; 
            _PrevDistance = 0f;

            _IsPlaying = true;
        }

        public void OnMovement()
        {
            if (_IsPlaying || _Distance.SafeEquals(0f))
                return;

            _PrevDistance = 0f;

            _IsPlaying = true;
        }

        public void EndMovement()
        {
            if (!IsPlaying)
                return;

            _IsPlaying = false;

            if(_NeedUpdate)
            {
                _Distance = _TempDistance;
                _Curve = _TempCurve;
            }
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
                EndMovement();
            }

            return currentDistance;
        }
    }
}