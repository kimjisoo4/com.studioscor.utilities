using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class DefualtConditionVariable : ConditionVariable
    {
        [Header(" [ Default Condition Variable ] ")]
        [SerializeField] private bool _condition = false;

        private DefualtConditionVariable _original;

        public override IVariable<bool> Clone()
        {
            var clone = new DefualtConditionVariable();

            _original = clone;

            return clone;
        }

        public override bool GetValue()
        {
            return _original is null ? _condition : _original._condition;
        }
    }
}
