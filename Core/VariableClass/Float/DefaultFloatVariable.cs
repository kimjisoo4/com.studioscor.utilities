using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class DefaultFloatVariable : FloatVariable
    {
        [Header(" [ Default Float Variable ] ")]
        [SerializeField] private float _value = 1f;

        private DefaultFloatVariable _original = null;

        public DefaultFloatVariable() { }
        public DefaultFloatVariable(float newValue)
        {
            _value = newValue;
        }

        public override IVariable<float> Clone()
        {
            var clone = new DefaultFloatVariable();

            clone._original = this;

            return clone;
        }

        public override float GetValue()
        {
            return _original is not null ? _original._value : _value;
        }
    }
}
