using UnityEngine;

namespace StudioScor.Utilities
{
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
            _NextState = state;

            if (_CurrentState is not null)
            {
                _CurrentState.ForceExitState();
            }

            _PrevState = _CurrentState;
            _CurrentState = _NextState;
            _NextState = null;

            _CurrentState.ForceEnterState();

            _PrevState = null;
            _NextState = null;
        }
    }
}