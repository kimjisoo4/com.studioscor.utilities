using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "Utilities/Variable/new Vector3 Variable", fileName = "Vector3Variable_")]
    public class Vector3Variable : VariableObject<Vector3>
    {
        public override void AddValue(Vector3 value)
        {
            if (value == Vector3.zero)
                return;

            Vector3 prevValue = _RuntimeValue;
            _RuntimeValue += value;

            OnChangeValue(prevValue);
        }

        public override void SetValue(Vector3 value)
        {
            if (value == Vector3.zero)
                return;

            Vector3 prevValue = _RuntimeValue;
            _RuntimeValue -= value;

            OnChangeValue(prevValue);
        }

        public override void SubtractValue(Vector3 value)
        {
            if (value == _RuntimeValue)
                return;

            Vector3 prevValue = _RuntimeValue;
            _RuntimeValue = value;

            OnChangeValue(prevValue);
        }
    }

}
