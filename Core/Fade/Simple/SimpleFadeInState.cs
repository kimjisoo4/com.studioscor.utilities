using UnityEngine;

namespace StudioScor.Utilities
{
    public class SimpleFadeInState : SimpleFadeState
    {
        [Header(" [ Simple Fading State ] ")]
        [SerializeField] private float _Duration;

        private float _ElapsedTime = 0f;

        private void OnEnable()
        {
            _ElapsedTime = _Duration * SimpleFade.Amount;
        }
        private void Update()
        {
            float deltaTime = Time.deltaTime;

            if (deltaTime.SafeEquals(0f))
                return;

            _ElapsedTime += deltaTime;

            float amount = _ElapsedTime.SafeDivide(_Duration);

            amount = Mathf.Min(1f, amount);

            SimpleFade.SetFadeAmount(amount);
        }
    }

}
