using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class ChargeableObject : BaseMonoBehaviour
    {
        [Header(" [ Charageable Object ] ")]
        [SerializeField] private float _Duration = 2f;
        [SerializeField][SRange(0f, 1f)] private float _Offset = 0f;
        [SerializeField] private bool _AutoPlaying = true;

        private readonly Chargeable _Chargeable = new();

        public UnityAction OnStartedCharging;
        public UnityAction OnFinishedCharging;
        public UnityAction OnReachedCharging;

        [SerializeField] private UnityEvent _OnStartedCharging;
        [SerializeField] private UnityEvent _OnFinishedCharging;
        [SerializeField] private UnityEvent _OnReachedCharging;

        public float ChargeStrength => _Chargeable.Charged;
        public bool IsFulled => _Chargeable.IsFulled;

        private void Awake()
        {
            _Chargeable.OnStartedCharging += Chargeable_OnStartedCharging;
            _Chargeable.OnFinishedCharging += Chargeable_OnFinishedCharging;
            _Chargeable.OnReachedCharging += Chargeable_OnReachedCharging;
        }

        private void OnEnable()
        {
            if (_AutoPlaying)
                OnCharging();
        }
        private void OnDisable()
        {
            EndCharging();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            UpdateCharging(deltaTime);
        }

        public void OnCharging()
        {
            _Chargeable.OnCharging(_Duration, _Offset);
        }
        public void EndCharging()
        {
            _Chargeable.EndCharging();
        }
        public void UpdateCharging(float deltaTime)
        {
            _Chargeable.UpdateCharging(deltaTime);
        }

        public void ResetChargeable()
        {

        }

        private void Chargeable_OnStartedCharging(Chargeable chargeable)
        {
            _OnStartedCharging?.Invoke();
            OnStartedCharging?.Invoke();
        }

        private void Chargeable_OnReachedCharging(Chargeable chargeable)
        {
            _OnReachedCharging?.Invoke();
            OnReachedCharging?.Invoke();
        }

        private void Chargeable_OnFinishedCharging(Chargeable chargeable)
        {
            _OnFinishedCharging?.Invoke();
            OnFinishedCharging?.Invoke();
        }
    }

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

    
}