using UnityEngine;
using System.Collections.Generic;

namespace KimScor.Utilities
{
    [System.Serializable]
    public class FiniteStateMachineSystemWithKey<TKey, T> : FiniteStateMachineSystem<T> where T : class,IState
    {
        [SerializeField] private TKey _DefaultStateKey;

        private Dictionary<TKey, T> _States;

        public FiniteStateMachineSystemWithKey(TKey key, T defaultState) : base(defaultState)
        {
            _States = new();

            _States.Add(key, defaultState);
        }

        public override void Setup()
        {
            if (_States is null)
            {
                _States = new();

                AddState(_DefaultStateKey, _DefaultState);
            }

            base.Setup();
        }

        public bool TrySetState(TKey key)
        {
            if (_States.TryGetValue(key, out T state))
            {
                return TrySetState(state);
            }

            return false;
        }
        public void ForceSetState(TKey key)
        {
            if (_States.TryGetValue(key, out T state))
            {
                ForceSetState(state);
            }
        }
        public void ClearState()
        {
            _States.Clear();
        }
        public bool AddState(TKey key, T state)
        {
            return _States.TryAdd(key, state);
        }
        public bool RemoveState(TKey key)
        {
            return _States.Remove(key);
        }
    }
}