using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    public enum ECalcType
    {
        Add,
        Subtract,
        Multiply,
        Divide,
    }

    public interface ICalculation
    {
        public void Setup(GameObject owner);
        public ICalculation Clone();
        public float Calc(float value);
    }
   
    [Serializable]
    public class Calculation : ICalculation
    {
        [SerializeField] private ECalcType _type;
        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReferenceDropdown]
#endif
        private IFloatVariable _value;
        private Calculation _original;

        public GameObject Owner { get; protected set; }

        public void Setup(GameObject owner)
        {
            Owner = owner;

            _value.Setup(Owner);
        }

        public ICalculation Clone()
        {
            var clone = new Calculation();

            clone._original = this;
            clone._value = _value.Clone();

            return clone;
        }

        public float Calc(float value)
        {
            var rhs = _value.GetValue();
            var type = _original is null ? _type : _original._type;

            switch (type)
            {
                case ECalcType.Add:
                    return value + rhs;
                case ECalcType.Subtract:
                    return value - rhs;
                case ECalcType.Multiply:
                    return value * rhs;
                case ECalcType.Divide:
                    return value.SafeDivide(rhs);
                default:
                    return value;
            }
        }
    }
}
