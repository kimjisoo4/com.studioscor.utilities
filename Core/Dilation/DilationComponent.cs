using UnityEngine;


namespace StudioScor.Utilities
{
    public class DilationComponent : BaseMonoBehaviour, IDilationSystem
    {
        [Header(" [ Dilation Component ] ")]
        [SerializeField] private float _speed = 1f;

        private float _defaultSpeed;
        public float Speed => _speed;

        public event IDilationSystem.DilationEventHandler OnChangedDilation;

        void Awake()
        {
            _defaultSpeed = _speed;
        }

        public void ResetDilation()
        {
            SetDilation(_defaultSpeed);
        }

        public void SetDilation(float newDilation)
        {
            if (_speed.SafeEquals(newDilation))
                return;

            var prevDilation = _speed;
            _speed = newDilation;

            Invoke_OnChangedDilation(prevDilation);
        }

        private void Invoke_OnChangedDilation(float prevDilation)
        {
            Log($"On Changed Dilation [ Current : {_speed:N2} | Prev : {prevDilation:N2} ]");

            OnChangedDilation?.Invoke(this, _speed, prevDilation);
        }
    }
}