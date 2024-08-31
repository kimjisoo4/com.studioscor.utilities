using System.Collections.Generic;
using UnityEngine;
using static StudioScor.Utilities.IChargeable;

namespace StudioScor.Utilities
{
    public interface IChargeable
    {
        public delegate void ChargingStateHander(IChargeable chargeable);
        public delegate void ChargingLevelStateHandler(IChargeable chargeable, int currentLevel, int prevLevel);

        public float Strength { get; }
        public bool IsPlaying { get; }
        public bool IsFulled { get; }
        public int MaxChargeLevel { get; }
        public int CurrentChargeLevel { get; }

        public void OnCharging(float startOffset = 0f);
        public void FinishCharging();
        public void CancelCharging();
        public void SetChargeLevel(int newLevel);
        public void SetStrength(float newStrenth);

        public event ChargingStateHander OnStartedCharge;
        public event ChargingStateHander OnEndedCharge;
        public event ChargingStateHander OnCanceledCharge;
        public event ChargingStateHander OnFinishedCharge;
        public event ChargingStateHander OnFulledCharge;
        public event ChargingLevelStateHandler OnChangedChargeLevel;
    }

    [System.Serializable]
    public class Chargeable : BaseClass, IChargeable
    {
        [Header(" [ Chargeable ] ")]
        [SerializeField] private float[] _chargeValues = new float[] { 1f };
        [SerializeField][SReadOnly][SRange(0f,1f)] private float _strength;
        [SerializeField][SReadOnly] private bool _isPlaying;
        [SerializeField][SReadOnly] private bool _isFulled;
        [SerializeField][SReadOnly] private int _currentChargeLevel;
        public float Strength => _strength;
        public bool IsPlaying => _isPlaying;
        public bool IsFulled => _isFulled;
        public IReadOnlyCollection<float> ChargeValues => _chargeValues;
        public int CurrentChargeLevel => _currentChargeLevel;
        public int MaxChargeLevel => _chargeValues.Length;

        public event ChargingStateHander OnStartedCharge;
        public event ChargingStateHander OnEndedCharge;
        public event ChargingStateHander OnCanceledCharge;
        public event ChargingStateHander OnFinishedCharge;
        public event ChargingStateHander OnFulledCharge;
        public event ChargingLevelStateHandler OnChangedChargeLevel;

        public Chargeable()
        {
        }

        public void OnCharging(float startOffset = -1f)
        {
            if (_isPlaying)
                return;

            _isPlaying = true;
            _isFulled = false;
            _currentChargeLevel = 0;

            SetStrength(startOffset);

            Invoke_OnStartedCharging();
        }

        public void CancelCharging()
        {
            if (!IsPlaying)
                return;

            _isPlaying = false;

            Invoke_OnCanceledCharging();
            Invoke_OnEndedCharging();
        }

        public void FinishCharging()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;

            Invoke_OnFinishedCharging();
            Invoke_OnEndedCharging();
        }

        public void SetStrength(float charged)
        {
            if (charged < 0f)
                _strength = 0f;
            else
                _strength = charged;

            UpdateChargeLevel();
            UpdateFulledState();
        }
        public void SetChargeLevel(int newLevel)
        {
            if (_currentChargeLevel == newLevel)
                return;

            _currentChargeLevel = newLevel;

            if (_currentChargeLevel == 0)
                _strength = 0;
            else
                _strength = _chargeValues[_currentChargeLevel - 1];

            UpdateFulledState();
        }

        private void UpdateChargeLevel()
        {
            var prevLevel = _currentChargeLevel;

            if (_strength <= 0f)
            {
                _currentChargeLevel = 0;
            }
            else
            {
                for (int i = 0; i < _chargeValues.Length; i++)
                {
                    var chargeValue = _chargeValues[i];

                    if (_strength >= chargeValue)
                    {
                        _currentChargeLevel = i + 1;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (prevLevel != _currentChargeLevel)
            {
                Invoke_OnChangedChargeLevel(prevLevel);
            }
        }

        private void UpdateFulledState()
        {
            if (_strength >= 1f)
            {
                if(!_isFulled)
                {
                    _isFulled = true;

                    Invoke_OnFulledCharging();
                }
            }
            else
            {
                if(_isFulled)
                {
                    _isFulled = false;
                }
            }
        }

        
        
        protected virtual void Invoke_OnStartedCharging()
        {
            Log($"{nameof(OnStartedCharge)}");

            OnStartedCharge?.Invoke(this);
        }
        protected virtual void Invoke_OnFinishedCharging()
        {
            Log($"{nameof(OnFinishedCharge)}");

            OnFinishedCharge?.Invoke(this);
        }
        protected virtual void Invoke_OnFulledCharging()
        {
            Log($"{nameof(OnFulledCharge)}");

            OnFulledCharge?.Invoke(this);
        }
        protected virtual void Invoke_OnCanceledCharging()
        {
            Log($"{nameof(OnCanceledCharge)}");

            OnCanceledCharge?.Invoke(this);
        }
        protected virtual void Invoke_OnEndedCharging()
        {
            Log($"{nameof(OnEndedCharge)}");

            OnEndedCharge?.Invoke(this);
        }

        protected virtual void Invoke_OnChangedChargeLevel(int prevLevel)
        {
            Log($"{nameof(OnChangedChargeLevel)} :: Current Level - {_currentChargeLevel} ||  Prev Level - {prevLevel}");

            OnChangedChargeLevel?.Invoke(this, _currentChargeLevel, prevLevel);
        }
    }

    
}