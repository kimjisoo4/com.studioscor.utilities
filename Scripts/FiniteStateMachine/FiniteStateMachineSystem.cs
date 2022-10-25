using UnityEngine;

namespace KimScor.Utilities
{
    [System.Serializable]
    public class FiniteStateMachineSystem<T> where T : class, IState
    {
        [Header(" [ Default State ] ")]
        [SerializeField] protected T _DefaultState;

        [SerializeField] protected T _CurrentState;
        [SerializeField] protected T _PrevState;
        [SerializeField] protected T _NextState;

        public T CurrentState => _CurrentState;
        public T PrevState => _PrevState;
        public T NextState => _NextState;

        public FiniteStateMachineSystem(T defaultState)
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
        public bool TrySetState(T state)
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


            if (_CurrentState is not null)
            {
                _CurrentState.OnExitState();
            }

            _PrevState = _CurrentState;
            _CurrentState = _NextState;

            _CurrentState.OnEnterState();

            _PrevState = null;
            _NextState = null;

            return true;
        }

        public void ForceSetState(T state)
        {
            _NextState = state;

            if (_CurrentState is not null)
            {
                _CurrentState.OnExitState();
            }

            _PrevState = _CurrentState;
            _CurrentState = _NextState;
            _NextState = null;

            _CurrentState.OnEnterState();

            _PrevState = null;
            _NextState = null;
        }
    }
}