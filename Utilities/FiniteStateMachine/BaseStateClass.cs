using UnityEngine;
using System.Collections;

using System.Diagnostics;
using StudioScor.Utilities;
using UnityEngine.Events;

namespace StudioScor.Utilities 
{
    public abstract class BaseStateMono : MonoBehaviour, IState
    {
        [SerializeField] protected bool _UseDebug = false;

        public event UnityAction OnEnteredState;
        public event UnityAction OnExitedState;

        #region EDITOR ONLY
        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object massage)
        {
#if UNITY_EDITOR
            if (_UseDebug)
                Utility.Debug.Log(name + "[ " + GetType().Name + " ] :" + massage, this);
#endif
        }

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

            OnEnteredState?.Invoke();
        }
        private void OnExitState()
        {
            Log("Exit State");

            OnExitedState?.Invoke();
        }
    }

    public abstract class BaseStateClass : IState
    {       
        public event UnityAction OnEnteredState;
        public event UnityAction OnExitedState;

        [SerializeField] private bool _UseDebug;

        protected Transform _Owner;
        protected bool _IsActivate;
        public bool IsActivate => _IsActivate;
        public Transform Owner => _Owner;

        public BaseStateClass()
        {

        }
        public BaseStateClass(Transform owner)
        {
            _Owner = owner;
        }

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object massage)
        {
#if UNITY_EDITOR
            if (_UseDebug)
                Utility.Debug.Log(_Owner.name + "[ " + GetType().Name + " ] :" + massage, _Owner);
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

            OnEnteredState?.Invoke();
        }
        private void OnExitState()
        {
            Log("Exit State");

            OnExitedState?.Invoke();
        }
    }
}