using UnityEngine;
using System.Collections;

using System.Diagnostics;
using StudioScor.Utilities;
using UnityEngine.Events;

namespace StudioScor.Utilities 
{
    public abstract class BaseStateMono : BaseMonoBehaviour, IState
    {
        public event UnityAction<IState> OnEnteredState;
        public event UnityAction<IState> OnExitedState;

        #region EDITOR ONLY

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            enabled = false;
        }
#endif
        #endregion

        public virtual bool CanEnterState()
        {
            return !enabled;
        }

        public virtual bool CanExitState()
        {
            return enabled;
        }

        public void ForceExitState()
        {
            enabled = false;

            OnExitState();
        }

        public void ForceEnterState()
        {
            enabled = true;

            OnEnterState();
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

        private void OnEnterState()
        {
            Log("Enter State");

            OnEnteredState?.Invoke(this);
        }
        private void OnExitState()
        {
            Log("Exit State");

            OnExitedState?.Invoke(this);
        }
    }

    public abstract class BaseStateClass : IState
    {       
        public event UnityAction<IState> OnEnteredState;
        public event UnityAction<IState> OnExitedState;

        protected virtual bool UseDebug { get; } 

        protected bool _IsActivate;
        public bool IsActivate => _IsActivate;

        public BaseStateClass()
        {

        }

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object massage)
        {
#if UNITY_EDITOR
            if (UseDebug)
                Utility.Debug.Log("[ " + GetType().Name + " ] :" + massage);
#endif
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

            OnEnterState();
        }
        public void ForceExitState()
        {
            if (!_IsActivate)
                return;

            _IsActivate = false;

            ExitState();

            OnExitState();
        }

        protected abstract void EnterState();
        protected virtual void ExitState() { }

        private void OnEnterState()
        {
            Log("Enter State");

            OnEnteredState?.Invoke(this);
        }
        private void OnExitState()
        {
            Log("Exit State");

            OnExitedState?.Invoke(this);
        }
    }
}