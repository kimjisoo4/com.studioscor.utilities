using UnityEngine;

namespace StudioScor.Utilities
{

    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new Vector2 Variable", fileName = "Variable_Vector2_")]
    public class Variable_Vector2 : VariableObject<Vector2>
    {
        public void AddValue(Vector2 value)
        {
            if (value == Vector2.zero)
                return;

            Vector2 prevValue = _runtimeValue;

            _runtimeValue += value;

            Callback_OnChangeValue(prevValue);
        }
        public void SubtractValue(Vector2 value)
        {
            if (value == Vector2.zero)
                return;

            Vector2 prevValue = _runtimeValue;

            _runtimeValue -= value;

            Callback_OnChangeValue(prevValue);
        }

        public override void SetValue(Vector2 value)
        {
            if (_runtimeValue.SafeEqauls(value))
                return;

            Vector2 prevValue = _runtimeValue;

            _runtimeValue = value;

            Callback_OnChangeValue(prevValue);
        }
    }

}
