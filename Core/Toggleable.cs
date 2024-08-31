using System;

namespace StudioScor.Utilities
{
    public class Toggleable : BaseClass
    {
        public delegate void ToggleFlowStateEventHandler(Toggleable toggleable, bool isOn);

        private bool _isOn = false;
        public bool IsOn => _isOn; 
        
        private readonly Action _onAction;
        private readonly Action _offAction;

        public event ToggleFlowStateEventHandler OnChangedToggleState;

        public Toggleable() { }
        public Toggleable(Action onAction, Action offAction)
        {
            _onAction = onAction;
            _offAction = offAction;
        }

        public bool TryOnToggle()
        {
            if (IsOn)
                return false;

            OnToggle();

            return true;
        }
        public bool TryOffToggle()
        {
            if (!IsOn)
                return false;

            OffToggle();

            return true;
        }
        public void OnToggle()
        {
            if (IsOn)
                return;

            _isOn = true;

            _onAction?.Invoke();
            Invoke_OnChangedGateState();
        }
        public void OffToggle()
        {
            if (!IsOn)
                return;

            _isOn = false;

            _offAction?.Invoke();
            Invoke_OnChangedGateState();
        }

        private void Invoke_OnChangedGateState()
        {
            Log($"{nameof(OnChangedToggleState)} - {(_isOn ? "On" : "Off")}");

            OnChangedToggleState?.Invoke(this, _isOn);
        }
    }
}