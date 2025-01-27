using UnityEngine;
using System.Collections.Generic;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public class FiniteStateMachineSystemWithKey<TKey, TState> : FiniteStateMachineSystem<TState> where TState : class,IState
    {
        [SerializeField] private TKey defaultStateKey;
        
        private TKey currentStateKey;
        
        private readonly Dictionary<TKey, TState> states = new();
        public IReadOnlyDictionary<TKey, TState> States => states;
        public TKey CurrentStateKey
        {
            get
            {
                if (needUpdateKey)
                {
                    foreach (var state in states)
                    {
                        if (state.Value == _currentState)
                        {
                            currentStateKey = state.Key;

                            needUpdateKey = false;

                            break;
                        }
                    }
                }

                return currentStateKey;
            }
        }

        private bool needUpdateKey = false;

        public FiniteStateMachineSystemWithKey() { }
        public FiniteStateMachineSystemWithKey(TKey key, TState defaultState) : base(defaultState)
        {
            states = new()
            {
                { key, defaultState }
            };
        }

        public void SetDefaultState(TKey key, TState state)
        {
            defaultStateKey = key;
            _defaultState = state;

            if(!states.ContainsKey(key))
            {
                AddState(key, state);
            }
        }

        public bool HasState(TKey key)
        {
            return states.ContainsKey(key);
        }
        public bool HasState(TState state)
        {
            return states.ContainsValue(state);
        }

        public bool CanSetState(TKey key)
        {
            return states.TryGetValue(key, out TState state) && CanSetState(state);
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

            needUpdateKey = true;
        }

        public void ForceSetState(TKey key)
        {
            if (states.TryGetValue(key, out TState state))
            {
                ForceSetState(state);
            }
        }
        public void ClearState()
        {
            states.Clear();
        }
        public bool AddState(TKey key, TState state)
        {
            return states.TryAdd(key, state);
        }
        public bool RemoveState(TKey key)
        {
            return states.Remove(key);
        }
    }
}