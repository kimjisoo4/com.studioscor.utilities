using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public abstract class BaseStateMono : BaseMonoBehaviour, IState
    {
        [Header(" [ State MonoBehaviour ] ")]
        [SerializeField] private ToggleableUnityEvent _onEnteredState;
        [SerializeField] private ToggleableUnityEvent _onExitedState;

        public event IState.StateEventHandler OnEnteredState;
        public event IState.StateEventHandler OnExitedState;

        private bool _isPlaying;
        public bool IsPlaying => _isPlaying;

        #region EDITOR ONLY

        protected virtual void Reset()
        {
#if UNITY_EDITOR
            enabled = false;
#endif
        }
        #endregion


        public virtual bool CanEnterState()
        {
            return !IsPlaying;
        }

        public virtual bool CanExitState()
        {
            return IsPlaying;
        }


        public void ForceEnterState()
        {
            _isPlaying = true;

            enabled = true;

            EnterState();

            Invoke_OnEnteredState();
        }

        public void ForceExitState()
        {
            _isPlaying = false;

            ExitState();

            enabled = false;

            Invoke_OnExitedState();
        }

        public bool TryEnterState()
        {
            if (!CanEnterState())
                return false;

            ForceEnterState();

            return true;
        }

        public bool TryExitState()
        {
            if (!CanExitState())
                return false;

            ForceExitState();

            return true;
        }

        protected virtual void EnterState() { }
        protected virtual void ExitState() { }

        private void Invoke_OnEnteredState()
        {
            Log(nameof(OnEnteredState));

            _onEnteredState.Invoke();
            OnEnteredState?.Invoke(this);
        }
        private void Invoke_OnExitedState()
        {
            Log(nameof(OnExitedState));

            _onExitedState.Invoke();    
            OnExitedState?.Invoke(this);
        }
    }

    public abstract class BaseStateClass : BaseClass, IState
    {       
        public event IState.StateEventHandler OnEnteredState;
        public event IState.StateEventHandler OnExitedState;

        protected bool _IsActivate;
        public bool IsActivate => _IsActivate;

        public BaseStateClass()
        {

        }

        public virtual bool TryEnterState()
        {
            if (!CanEnterState())
                return false;

            ForceEnterState();

            return true;
        }
        public virtual bool TryExitState()
        {
            if (!CanExitState())
                return false;

            ForceExitState();

            return true;
        }
        public virtual bool CanEnterState()
        {
            return !_IsActivate;
        }
        public virtual bool CanExitState()
        {
            return _IsActivate;
        }
        public void ForceEnterState()
        {
            if (_IsActivate)
                return;

            _IsActivate = true;

            EnterState();

            Invoke_OnEnteredState();
        }
        public void ForceExitState()
        {
            if (!_IsActivate)
                return;

            _IsActivate = false;

            ExitState();

            Invoke_OnExitedState();
        }

        protected abstract void EnterState();
        protected virtual void ExitState() { }

        private void Invoke_OnEnteredState()
        {
            Log($"{nameof(OnEnteredState)}");

            OnEnteredState?.Invoke(this);
        }
        private void Invoke_OnExitedState()
        {
            Log($"{nameof(OnExitedState)}");

            OnExitedState?.Invoke(this);
        }
    }
}