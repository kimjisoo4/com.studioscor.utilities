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

        private bool _IsPlaying;
        public bool IsPlaying => _IsPlaying;

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
            return !IsPlaying;
        }

        public virtual bool CanExitState()
        {
            return IsPlaying;
        }

        public void ForceExitState()
        {
            _IsPlaying = false;

            OnExitState();

            enabled = false;

            Callbaco_OnExitedState();
        }

        public void ForceEnterState()
        {
            _IsPlaying = true;

            enabled = true;

            OnEnterState();

            Callback_OnEnteredState();
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

        protected virtual void OnEnterState()
        {

        }
        protected virtual void OnExitState()
        {

        }

        private void Callback_OnEnteredState()
        {
            Log("Entered State");

            OnEnteredState?.Invoke(this);
        }
        private void Callbaco_OnExitedState()
        {
            Log("Exited State");

            OnExitedState?.Invoke(this);
        }
    }

    public abstract class BaseStateClass : BaseClass, IState
    {       
        public event UnityAction<IState> OnEnteredState;
        public event UnityAction<IState> OnExitedState;

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