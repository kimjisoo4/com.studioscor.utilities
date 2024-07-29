using UnityEngine;

namespace StudioScor.Utilities
{
    public class SimpleFadeInState : SimpleFadeState
    {
        [Header(" [ Simple Fading State ] ")]
        [SerializeField] private float duration = 1f;

        private float elapsedTime = 0f;

        private void OnEnable()
        {
            elapsedTime = duration * (1 - SimpleFade.Amount);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            if (deltaTime.SafeEquals(0f))
                return;

            elapsedTime += deltaTime;

            float amount = elapsedTime.SafeDivide(duration);

            amount = Mathf.Min(1f, amount);

            SimpleFade.SetFadeAmount(1f - amount);
        }
    }

}
