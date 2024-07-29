using UnityEngine;
using UnityEngine.Events;
using static StudioScor.Utilities.ChargeableObject;

namespace StudioScor.Utilities
{
    public class ChargeableObject : BaseMonoBehaviour, IChargeable
    {
        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField] private UnityEvent _onStartedCharging;
            [SerializeField] private UnityEvent _onCancededCharging;
            [SerializeField] private UnityEvent<int> _onChangedChargeLevel;
            [SerializeField] private UnityEvent _onFulledCharging;
            [SerializeField] private UnityEvent _onFinishedCharging;
            [SerializeField] private UnityEvent _onEndedCharging;

            public void AddUnityEvent(IChargeable chargeable)
            {
                chargeable.OnStartedCharge += ChargeableObject_OnStartedCharging;
                chargeable.OnEndedCharge += ChargeableObject_OnEndedCharging;
                chargeable.OnCanceledCharge += ChargeableObject_OnCanceledCharging;
                chargeable.OnFinishedCharge += ChargeableObject_OnFinishedCharging;
                chargeable.OnFulledCharge += ChargeableObject_OnReachedCharging;
                chargeable.OnChangedChargeLevel += Chargeable_OnChangedChargeLevel;
            }

            public void RemoveUnityEvent(IChargeable chargeable)
            {
                chargeable.OnStartedCharge -= ChargeableObject_OnStartedCharging;
                chargeable.OnEndedCharge -= ChargeableObject_OnEndedCharging;
                chargeable.OnCanceledCharge -= ChargeableObject_OnCanceledCharging;
                chargeable.OnFinishedCharge -= ChargeableObject_OnFinishedCharging;
                chargeable.OnFulledCharge -= ChargeableObject_OnReachedCharging;
                chargeable.OnChangedChargeLevel -= Chargeable_OnChangedChargeLevel;
            }

            private void Chargeable_OnChangedChargeLevel(IChargeable chargeable, int currentLevel, int prevLevel)
            {
                _onChangedChargeLevel?.Invoke(currentLevel);
            }
            private void ChargeableObject_OnFinishedCharging(IChargeable chargeable)
            {
                _onFinishedCharging?.Invoke();
            }

            private void ChargeableObject_OnEndedCharging(IChargeable chargeable)
            {
                _onEndedCharging?.Invoke();
            }

            private void ChargeableObject_OnReachedCharging(IChargeable chargeable)
            {
                _onFulledCharging?.Invoke();
            }

            private void ChargeableObject_OnCanceledCharging(IChargeable chargeable)
            {
                _onCancededCharging?.Invoke();
            }

            private void ChargeableObject_OnStartedCharging(IChargeable chargeable)
            {
                _onStartedCharging?.Invoke();
            }
        }

        [Header(" [ Charageable Object ] ")]
        [SerializeField] private Chargeable _chargeable;

        [Header(" Events ")]
        [SerializeField] private bool _useUnityEvent = true;
        [SerializeField][SCondition(nameof(_useUnityEvent))] private UnityEvents _unityEvents;

        public event IChargeable.ChargingLevelStateHandler OnChangedChargeLevel { add => _chargeable.OnChangedChargeLevel += value; remove => _chargeable.OnChangedChargeLevel -= value; }
        public event IChargeable.ChargingStateHander OnStartedCharge { add { _chargeable.OnStartedCharge += value; } remove { _chargeable.OnStartedCharge -= value; } }
        public event IChargeable.ChargingStateHander OnEndedCharge { add { _chargeable.OnEndedCharge += value; } remove { _chargeable.OnEndedCharge -= value; } }
        public event IChargeable.ChargingStateHander OnCanceledCharge { add { _chargeable.OnCanceledCharge += value; } remove { _chargeable.OnCanceledCharge -= value; } }
        public event IChargeable.ChargingStateHander OnFinishedCharge { add { _chargeable.OnFinishedCharge += value; } remove { _chargeable.OnFinishedCharge -= value; } }
        public event IChargeable.ChargingStateHander OnFulledCharge { add { _chargeable.OnFulledCharge += value; } remove { _chargeable.OnFulledCharge -= value; } }

        public float Strength => _chargeable.Strength;

        public bool IsPlaying => _chargeable.IsPlaying;

        public bool IsFulled => _chargeable.IsFulled;

        public int MaxChargeLevel => _chargeable.MaxChargeLevel;

        public int CurrentChargeLevel => _chargeable.CurrentChargeLevel;

        private bool _wasInitialized;


        private void Awake()
        {
            Initialization();
        }
        private void OnDestroy()
        {
            if (_useUnityEvent)
            {
                _unityEvents.RemoveUnityEvent(this);
            }
        }

        private void Initialization()
        {
            if (_wasInitialized)
                return;

            _wasInitialized = true;

            if (_useUnityEvent)
            {
                _unityEvents.AddUnityEvent(this);
            }
        }
        public void OnCharging(float startOffset = -1f)
        {
            Initialization();

            _chargeable.OnCharging(startOffset);
        }

        public void FinishCharging()
        {
            _chargeable.FinishCharging();
        }

        public void CancelCharging()
        {
            _chargeable.CancelCharging();
        }

        public void SetStrength(float newStrenth)
        {
            _chargeable.SetStrength(newStrenth);
        }

        public void SetChargeLevel(int newLevel)
        {
            _chargeable.SetChargeLevel(newLevel);
        }
    }

    
}