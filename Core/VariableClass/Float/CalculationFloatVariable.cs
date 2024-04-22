using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class CalculationFloatVariable : FloatVariable
    {
        [Header(" [ Calculation Float Variable ] ")]
        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReferenceDropdown]
#endif
        private IFloatVariable _value = new DefaultFloatVariable();

        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReferenceDropdown]
#endif
        private ICalculation[] _calculations;


        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            _value.Setup(owner);

            foreach (var calculation in _calculations)
            {
                calculation.Setup(owner);
            }
        }
        public override IFloatVariable Clone()
        {
            var clone = new CalculationFloatVariable();

            clone._value = _value.Clone();
            clone._calculations = new Calculation[_calculations.Length];

            for (int i = 0; i < _calculations.Length; i++)
            {
                clone._calculations[i] = _calculations[i].Clone();
            }


            return clone;
        }

        public override float GetValue()
        {
            var value = _value.GetValue();

            foreach (var calculation in _calculations)
            {
                value = calculation.Calc(value);
            }

            return value;
        }
    }
}
