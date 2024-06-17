using System;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public interface IVariable<T>
    {
        public GameObject Owner { get; }
        public void Setup(GameObject owner);
        public T GetValue();
    }

    [CreateAssetMenu(fileName = "VariableKey_", menuName = "StudioScor/Utilities/new VariableeKey")]
    public class VariableBlackboardKey : BaseScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private string _description;
#endif
    }

    public interface IVariableBlackBoard
    {
        public IReadOnlyDictionary<VariableBlackboardKey, object> Blackboard { get; }
        public TValue GetValue<TValue>(VariableBlackboardKey ket);
        public bool TryGetValue<TValue>(VariableBlackboardKey key, out TValue value);
        public void SetValue(VariableBlackboardKey key, object value);
        public bool HasValue(VariableBlackboardKey key);
    }

    public class VariableBlackBoard : MonoBehaviour, IVariableBlackBoard
    {
        private readonly Dictionary<VariableBlackboardKey, object> _blackboard = new();
        public IReadOnlyDictionary<VariableBlackboardKey, object> Blackboard => _blackboard;

        public TValue GetValue<TValue>(VariableBlackboardKey key)
        {
            return HasValue(key) ? (TValue)_blackboard[key] : default;
        }
        public bool TryGetValue<TValue>(VariableBlackboardKey key, out TValue value)
        {
            bool hasValue = _blackboard.TryGetValue(key, out object blackboardValue);

            value = (TValue)blackboardValue;

            return hasValue;
        }

        public bool HasValue(VariableBlackboardKey key)
        {
            return _blackboard.ContainsKey(key);
        }

        public void SetValue(VariableBlackboardKey key, object value)
        {
            if(HasValue(key))
            {
                _blackboard[key] = value;
            }
            else
            {
                _blackboard.Add(key, value);
            }
        }
        public void RemoveValue(VariableBlackboardKey key)
        {
            _blackboard.Remove(key);
        }
    }
}
