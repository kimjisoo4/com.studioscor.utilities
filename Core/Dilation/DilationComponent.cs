using UnityEngine;


namespace StudioScor.Utilities
{
    public class DilationComponent : BaseMonoBehaviour, IDilationSystem
    {
        [Header(" [ Dilation Component ] ")]
        [SerializeField] private float speed = 1f;

        private float defaultSpeed;
        public float Speed => speed;

        public event DilationEventHandler OnChangedDilation;

        void Awake()
        {
            defaultSpeed = speed;
        }

        public void ResetDilation()
        {
            SetDilation(defaultSpeed);
        }
        public void SetDilation(float newDilation)
        {
            if (speed.SafeEquals(newDilation))
                return;

            var prevDilation = speed;
            speed = newDilation;

            Callback_OnChangedDilation(prevDilation);
        }

        private void Callback_OnChangedDilation(float prevDilation)
        {
            Log($"On Changed Dilation [ Current : {speed:N2} | Prev : {prevDilation:N2} ]");

            OnChangedDilation?.Invoke(this, speed, prevDilation);
        }
    }
}