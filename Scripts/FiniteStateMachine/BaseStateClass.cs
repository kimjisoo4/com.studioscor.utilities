using UnityEngine;
using System.Collections;

using System.Diagnostics;
using KimScor.Utilities;
using UnityEngine.Events;

namespace KimScor.Utilities 
{
    public abstract class BaseStateMono : MonoBehaviour, IState
    {
        [SerializeField] private bool _UseDebug = false;

        public event UnityAction OnStartedState;
        public event UnityAction OnFinishedState;

        [Conditional("UNITY_EDITOR")]
        protected virtual void Log(object massage)
        {
#if UNITY_EDITOR
            if (_UseDebug)
                Utilities.Debug.Log(this.GetType() + "[ " + transform + " ] :" + massage, this);
#endif
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            enabled = false;
        }
#endif

        public virtual bool CanEnterState()
        {
            return !enabled;
        }

        public virtual bool CanExitState()
        {
            return enabled;
        }

        public void OnExitState()
        {
            OnFinishedState?.Invoke();

            enabled = false;
        }

        public void OnEnterState()
        {
            OnStartedState?.Invoke();

            enabled = true;
        }

        public bool TryEnterState()
        {
            if (!CanEnterState())
                return false;

            OnEnterState();

            return true;
        }

        public bool TryExitState()
        {
            if (!CanExitState())
                return false;

            OnExitState();

            return true;
        }
    }

    public abstract class BaseStateClass : IState
    {       
        public event UnityAction OnStartedState;
        public event UnityAction OnFinishedState;

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
                Utilities.Debug.Log(this.GetType() + "[ " + _Owner + " ] :" + massage, _Owner);
#endif
        }

        public virtual bool TryEnterState()
        {
            if (!CanEnterState())
                return false;

            OnEnterState();

            return true;
        }
        public virtual bool TryExitState()
        {
            if (!CanExitState())
                return false;

            OnExitState();

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
        public void OnEnterState()
        {
            if (_IsActivate)
                return;

            _IsActivate = true;

            EnterState();

            OnStartedState?.Invoke();
        }
        public void OnExitState()
        {
            if (!_IsActivate)
                return;

            _IsActivate = false;

            ExitState();

            OnFinishedState?.Invoke();
        }

        protected abstract void EnterState();
        protected abstract void ExitState();
    }
}