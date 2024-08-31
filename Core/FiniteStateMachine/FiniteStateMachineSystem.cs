using UnityEngine;

namespace StudioScor.Utilities
{
    public delegate void OnChangedStateHandler<T>(FiniteStateMachineSystem<T> stateMachine, T currentState, T prevState) where T : class, IState;
    
    [System.Serializable]
    public class FiniteStateMachineSystem<T> where T : class, IState
    {
        [Header(" [ Default State ] ")]
        [SerializeField] protected T defaultState;

        private bool _isPlaying;
        protected T currentState;
        protected T prevState;
        protected T nextState;

        public bool IsPlaying => _isPlaying;
        public T CurrentState => currentState;
        public T PrevState => prevState;
        public T NextState => nextState;

        public T DefaultState => defaultState;

        public event OnChangedStateHandler<T> OnChangedState;

        public FiniteStateMachineSystem() { }
        public FiniteStateMachineSystem(T defaultState)
        {
            this.defaultState = defaultState;
        }

        
        public void SetDefaultState(T defaultState)
        {
            this.defaultState = defaultState;
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
            return TrySetState(defaultState);
        }
        public void ForceSetDefaultState()
        {
            ForceSetState(defaultState);
        }

        public bool CanSetState(T state)
        {
            if (state == null)
                return false;

            nextState = state;

            if (currentState is not null)
            {
                if (!currentState.CanExitState())
                {
                    nextState = null;

                    return false;
                }
            }

            if (!nextState.CanEnterState())
            {
                nextState = null;

                return false;
            }

            nextState = null;

            return true;
        }

        public bool TrySetState(T state)
        {
            nextState = state;

            if (!CanSetState(state))
            {
                nextState = null;

                return false;
            }

            ForceSetState(state);

            return true;
        }

        public virtual void ForceSetState(T state)
        {
            prevState = currentState;
            currentState = state;

            if (prevState is not null)
            {
                prevState.ForceExitState();
            }
            if(currentState is not null)
            {
                currentState.ForceEnterState();
            }

            prevState = null;
            nextState = null;
        }

        protected virtual void Callback_OnChangedState(T prevState)
        {
            OnChangedState?.Invoke(this, currentState, prevState);
        }
    }
}