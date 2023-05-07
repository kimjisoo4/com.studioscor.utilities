using UnityEngine;

namespace StudioScor.Utilities
{
    public delegate void OnChangedStateHandler<T>(FiniteStateMachineSystem<T> stateMachine, T currentState, T prevState) where T : class, IState;
    
    [System.Serializable]
    public class FiniteStateMachineSystem<T> where T : class, IState
    {
        [Header(" [ Default State ] ")]
        [SerializeField] protected T _DefaultState;

        protected T _CurrentState;
        protected T _PrevState;
        protected T _NextState;

        public T CurrentState => _CurrentState;
        public T PrevState => _PrevState;
        public T NextState => _NextState;

        public event OnChangedStateHandler<T> OnChangedState;

        public FiniteStateMachineSystem()
        {

        }
        public FiniteStateMachineSystem(T defaultState)
        {
            _DefaultState = defaultState;
        }

        public void SetDefaultState(T defaultState)
        {
            _DefaultState = defaultState;
        }

        public virtual void Setup()
        {
            ForceSetState(_DefaultState);
        }
        public bool TrySetDefaultState()
        {
            return TrySetState(_DefaultState);
        }
        public void ForceSetDefaultState()
        {
            ForceSetState(_DefaultState);
        }

        public bool CanSetState(T state)
        {
            if (state == null)
                return false;

            _NextState = state;

            if (_CurrentState is not null)
            {
                if (!_CurrentState.CanExitState())
                {
                    _NextState = null;

                    return false;
                }
            }

            if (!_NextState.CanEnterState())
            {
                _NextState = null;

                return false;
            }

            _NextState = null;

            return true;
        }

        public bool TrySetState(T state)
        {
            _NextState = state;

            if (!CanSetState(state))
            {
                _NextState = null;

                return false;
            }

            ForceSetState(state);

            return true;
        }

        public virtual void ForceSetState(T state)
        {
            _PrevState = _CurrentState;
            _CurrentState = state;

            if (_PrevState is not null)
            {
                _PrevState.ForceExitState();
            }
            if(_CurrentState is not null)
            {
                _CurrentState.ForceEnterState();
            }

            _PrevState = null;
            _NextState = null;
        }

        protected virtual void Callback_OnChangedState(T prevState)
        {
            OnChangedState?.Invoke(this, _CurrentState, prevState);
        }
    }
}