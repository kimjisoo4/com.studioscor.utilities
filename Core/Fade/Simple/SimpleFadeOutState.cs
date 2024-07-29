using UnityEngine;

namespace StudioScor.Utilities
{
    public class SimpleFadeOutState : SimpleFadeState
    {
        [Header(" [ Simple Fading State ] ")]
        [SerializeField] private float _duration = 1f;

        private float _elapsedTime = 0f;

        private void OnEnable()
        {
            _elapsedTime = _duration * SimpleFade.Amount;
        }
        private void Update()
        {
            float deltaTime = Time.deltaTime;

            if (deltaTime.SafeEquals(0f))
                return;

            _elapsedTime += deltaTime;

            float amount = _elapsedTime.SafeDivide(_duration);

            amount = Mathf.Min(1f, amount);

            SimpleFade.SetFadeAmount(amount);
        }
    }

}
