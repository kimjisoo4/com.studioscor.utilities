using UnityEngine;
using System.Collections.Generic;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class FiniteStateMachineSystemWithKey<TKey, TState> : FiniteStateMachineSystem<TState> where TState : class,IState
    {
        [SerializeField] private TKey _DefaultStateKey;
        
        private TKey _CurrentStateKey;
        
        private Dictionary<TKey, TState> _States;
        public TKey CurrentStateKey
        {
            get
            {
                if (_NeedUpdateKey)
                {
                    foreach (var state in _States)
                    {
                        if (state.Value == _CurrentState)
                        {
                            _CurrentStateKey = state.Key;

                            _NeedUpdateKey = false;

                            break;
                        }
                    }
                }

                return _CurrentStateKey;
            }
        }

        private bool _NeedUpdateKey = false;
        
        public FiniteStateMachineSystemWithKey(TKey key, TState defaultState) : base(defaultState)
        {
            _States = new();

            _States.Add(key, defaultState);
        }
        public FiniteStateMachineSystemWithKey() : base()
        {
            _States = new();
        }

        public override void Setup()
        {
            if (_States is null)
            {
                _States = new();

                if(_DefaultStateKey != null && _DefaultState != null)
                    AddState(_DefaultStateKey, _DefaultState);
            }

            base.Setup();
        }

        public void SetDefaultState(TKey key, TState state)
        {
            _DefaultStateKey = key;
            _DefaultState = state;

            if(!_States.ContainsKey(key))
            {
                AddState(key, state);
            }
        }

        public bool HasState(TKey key)
        {
            return _States.ContainsKey(key);
        }
        public bool HasState(TState state)
        {
            return _States.ContainsValue(state);
        }

        public bool CanSetState(TKey key)
        {
            return _States.TryGetValue(key, out TState state) && CanSetState(state);
        }
        public bool TrySetState(TKey key)
        {
            if (!CanSetState(key))
            {
                return false;
            }

            ForceSetState(key);

            return true;
        }

        public override void ForceSetState(TState state)
        {
            base.ForceSetState(state);

            _NeedUpdateKey = true;
        }

        public void ForceSetState(TKey key)
        {
            if (_States.TryGetValue(key, out TState state))
            {
                ForceSetState(state);
            }
        }
        public void ClearState()
        {
            _States.Clear();
        }
        public bool AddState(TKey key, TState state)
        {
            return _States.TryAdd(key, state);
        }
        public bool RemoveState(TKey key)
        {
            return _States.Remove(key);
        }
    }
}