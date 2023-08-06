using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class Chargeable : BaseClass
    {
        public delegate void ChargingStateHander(Chargeable chargeable);

        [Header(" [ Chargeable ] ")]
        [SerializeField] private float duration;
        [SerializeField][SReadOnly][SRange(0f,1f)] private float chargeStrength;
        [SerializeField][SReadOnly] private bool isPlaying;
        [SerializeField][SReadOnly] private bool isFulled;
        
        
        private float elapsedTime;
        public float ChargeStrength => chargeStrength;
        public float Duration => duration;
        public float ElapsedTime => elapsedTime;
        public bool IsPlaying => isPlaying;
        public bool IsFulled => isFulled;

        public event ChargingStateHander OnStartedCharging;
        public event ChargingStateHander OnFinishedCharging;
        public event ChargingStateHander OnReachedCharging;

        public Chargeable()
        {
        }
        public Chargeable(float duration)
        {
            this.duration = duration;
        }
        public void OnCharging(float duration = -1f, float chargedOffset = -1f)
        {
            if (isPlaying)
                return;

            isPlaying = true;
            isFulled = false;

            SetDuration(duration);
            SetCharged(chargedOffset);

            Callback_OnStartedCharging();
        }

        public void SetDuration(float duration)
        {
            if (duration < 0f)
                return;

            this.duration = duration;

            chargeStrength = Mathf.Min(1f, elapsedTime.SafeDivide(this.duration));
        }

        public void SetCharged(float charged)
        {
            if (charged < 0f)
            {
                this.chargeStrength = 0f;
                elapsedTime = 0f;

                return;
            }

            this.chargeStrength = charged;
            elapsedTime = this.chargeStrength * duration;
        }

        public void EndCharging()
        {
            if (!isPlaying)
                return;

            isPlaying = false;

            Callback_OnFinishedCharging();
        }

        public void UpdateCharging(float deltaTime) 
        {
            if (!isPlaying || isFulled)
                return;

            elapsedTime += deltaTime;

            chargeStrength = Mathf.Min(1, elapsedTime.SafeDivide(duration));

            if (chargeStrength >= 1f)
            {
                isFulled = true;

                Callback_OnReachedCharging();
            }
        }

        protected virtual void Callback_OnStartedCharging()
        {
            Log("On Started Charging ");

            OnStartedCharging?.Invoke(this);
        }
        protected virtual void Callback_OnFinishedCharging()
        {
            Log("On Finished Charging");

            OnFinishedCharging?.Invoke(this);
        }
        protected virtual void Callback_OnReachedCharging()
        {
            Log("On Reached Charging");

            OnReachedCharging?.Invoke(this);
        }
    }

    
}