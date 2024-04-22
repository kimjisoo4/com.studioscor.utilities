using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class DefaultIntegerVariable : IntegerVariable
    {
        [Header(" [ Default Integer Variable ] ")]
        [SerializeField] private int _value;

        private DefaultIntegerVariable _original;

        public override IIntegerVariable Clone()
        {
            var clone = new DefaultIntegerVariable();

            clone._original = this;

            return clone;
        }

        public override int GetValue()
        {
            return _original is null ? _value : _original._value;
        }
    }
}
