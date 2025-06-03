using UnityEngine;
using UnityEngine.Serialization;

namespace StudioScor.Utilities
{
    public delegate void OnChangedStateHandler<T>(FiniteStateMachineSystem<T> stateMachine, T currentState, T prevState) where T : class, IState;
    
    [System.Serializable]
    public class FiniteStateMachineSystem<T> where T : class, IState
    {
        [Header(" [ Default State ] ")]
        [SerializeField][FormerlySerializedAs("defaultState")] protected T _defaultState;
        [SerializeField][SReadonly] protected T _currentState;
        protected T _prevState;
        protected T _nextState;

        private bool _isPlaying;

        public bool IsPlaying => _isPlaying;
        public T CurrentState => _currentState;
        public T PrevState => _prevState;
        public T NextState => _nextState;

        public T DefaultState => _defaultState;

        public event OnChangedStateHandler<T> OnChangedState;

        public FiniteStateMachineSystem() { }
        public FiniteStateMachineSystem(T defaultState)
        {
            this._defaultState = defaultState;
        }

        public void SetDefaultState(T defaultState)
        {
            this._defaultState = defaultState;
        }

        public virtual void Start()
        {
            if (_isPlaying)
                return;

            _isPlaying = true;

            ForceSetDefaultState();
        }

        public virtual void End()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;

            ForceSetState(null);
        }

        public bool TrySetDefaultState()
        {
            return TrySetState(_defaultState);
        }
        public void ForceSetDefaultState()
        {
            ForceSetState(_defaultState);
        }

        public bool CanSetState(T state)
        {
            if (state == null)
                return false;

            _nextState = state;

            if (_currentState is not null)
            {
                if (!_currentState.CanExitState())
                {
                    _nextState = null;

                    return false;
                }
            }

            if (!_nextState.CanEnterState())
            {
                _nextState = null;

                return false;
            }

            _nextState = null;

            return true;
        }

        public bool TrySetState(T state)
        {
            if (!_isPlaying)
                return false;

            if (state is null)
                return false;

            _nextState = state;

            if (!CanSetState(state))
            {
                _nextState = null;

                return false;
            }

            ForceSetState(state);

            return true;
        }

        public virtual void ForceSetState(T state)
        {
            if(_isPlaying)
            {
                _isPlaying = true;
            }

            _prevState = _currentState;
            _currentState = state;

            if (_prevState is not null)
            {
                _prevState.ForceExitState();
            }
            if(_currentState is not null)
            {
                _currentState.ForceEnterState();
            }

            Invoke_OnChangedState(_prevState);
            
            _prevState = null;
            _nextState = null;
        }

        protected virtual void Invoke_OnChangedState(T prevState)
        {
            OnChangedState?.Invoke(this, _currentState, prevState);
        }
    }
}