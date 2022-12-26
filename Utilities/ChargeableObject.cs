using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ChargeableObject : BaseMonoBehaviour
    {
        [Header(" [ Charageable Object ] ")]
        public UnityEvent<float> OnUpdatedStrength;
        public UnityEvent OnReacthMaxStrength;

        private float _ChargeStrength;
        public float ChargeStrength => _ChargeStrength;

        public void ResetChargeable()
        {
            _ChargeStrength = 0f;
        }

        public void SetChargeStrength(float normalizedCharge)
        {
            if (_ChargeStrength == normalizedCharge)
            {
                return;
            }

            float chargeSterngth = normalizedCharge;

            if (!chargeSterngth.InRange(0f, 1f))
            {
                Mathf.Clamp01(chargeSterngth);
            }

            if (_ChargeStrength == normalizedCharge)
                return;

            _ChargeStrength = normalizedCharge;

            OnUpdatedStrength?.Invoke(ChargeStrength);

            if (ChargeStrength >= 1f)
            {
                OnReacthMaxStrength?.Invoke();
            }
        }
    }
}