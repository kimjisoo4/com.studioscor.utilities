using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class VariableChangedEventListener<T> : BaseMonoBehaviour
    {
        [Header(" [ Variable Changed Event Listner ] ")]
        [SerializeField] private VariableObject<T> _Variable;
        [SerializeField] private UnityEvent<T> _OnChangedVariable;
        public event UnityAction<T> OnChangedVariable;

        private void OnEnable()
        {
            _OnChangedVariable?.Invoke(_Variable.Value);
            OnChangedVariable?.Invoke(_Variable.Value);

            _Variable.OnChangedValue += Variable_OnChangedValue;
        }

        private void OnDisable()
        {
            _Variable.OnChangedValue -= Variable_OnChangedValue;
        }

        private void Variable_OnChangedValue(VariableObject<T> variable, T currentValue, T prevValue)
        {
            _OnChangedVariable?.Invoke(currentValue);
            OnChangedVariable?.Invoke(currentValue);
        }

    }
}
