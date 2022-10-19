using UnityEngine;
using System.Collections;

using System.Diagnostics;
using KimScor.Utilities;
using UnityEngine.Events;

namespace KimScor.Utilities
{
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

            OnState();

            return true;
        }
        public virtual bool TryExitState()
        {
            if (!CanExitState())
                return false;

            EndState();

            return true;
        }
        public virtual bool CanEnterState()
        {
            return true;
        }
        public virtual bool CanExitState()
        {
            return true;
        }
        public void OnState()
        {
            if (_IsActivate)
                return;

            _IsActivate = true;

            EnterState();

            OnStartedState?.Invoke();
        }
        public void UpdateState(float deltaTime)
        {
            if (!_IsActivate)
                return;

            TickState(deltaTime);
        }
        public void EndState()
        {
            if (!_IsActivate)
                return;

            _IsActivate = false;

            ExitState();

            OnFinishedState?.Invoke();
        }

        protected abstract void EnterState();
        protected abstract void TickState(float deltaTime);
        protected abstract void ExitState();
    }
}