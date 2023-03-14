using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class Chargeable : BaseClass
    {
        public delegate void ChargingStateHander(Chargeable chargeable);

        private float _Charged;
        private float _Duration;
        private float _ElapsedTime;

        private bool _IsPlaying;
        private bool _IsFulled;

        public float ElapsedTime => _ElapsedTime;
        public float Duration => _Duration;
        public float Charged => _Charged;
        public bool IsPlaying => _IsPlaying;
        public bool IsFulled => _IsFulled;

        public event ChargingStateHander OnStartedCharging;
        public event ChargingStateHander OnFinishedCharging;
        public event ChargingStateHander OnReachedCharging;

        public Chargeable()
        {
        }
        public Chargeable(float duration)
        {
            _Duration = duration;
        }
        public void OnCharging(float duration = -1f, float chargedOffset = -1f)
        {
            if (_IsPlaying)
                return;

            _IsPlaying = true;
            _IsFulled = false;

            SetDuration(duration);
            SetCharged(chargedOffset);

            Callback_OnStartedCharging();
        }

        public void SetDuration(float duration)
        {
            if (duration < 0f)
                return;

            _Duration = duration;

            _Charged = Mathf.Min(1f, _ElapsedTime.SafeDivide(_Duration));
        }

        public void SetCharged(float charged)
        {
            if (charged < 0f)
            {
                _Charged = 0f;
                _ElapsedTime = 0f;

                return;
            }

            _Charged = charged;
            _ElapsedTime = _Charged * _Duration;
        }

        public void EndCharging()
        {
            if (!_IsPlaying)
                return;

            _IsPlaying = false;

            Callback_OnFinishedCharging();
        }

        public void UpdateCharging(float deltaTime) 
        {
            if (!_IsPlaying || _IsFulled)
                return;

            _ElapsedTime += deltaTime;

            _Charged = Mathf.Min(1, _ElapsedTime.SafeDivide(_Duration));

            if (_Charged >= 1f)
            {
                _IsFulled = true;

                Callback_OnReachedCharging();
            }
        }

        private void Callback_OnStartedCharging()
        {
            Log("On Started Charging ");

            OnStartedCharging?.Invoke(this);
        }
        private void Callback_OnFinishedCharging()
        {
            Log("On Finished Charging");

            OnFinishedCharging?.Invoke(this);
        }
        private void Callback_OnReachedCharging()
        {
            Log("On Reached Charging");

            OnReachedCharging?.Invoke(this);
        }
    }

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