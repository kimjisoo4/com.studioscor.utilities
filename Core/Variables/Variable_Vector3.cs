using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new Vector3 Variable", fileName = "Variable_Vector3_")]
    public class Variable_Vector3 : VariableObject<Vector3>
    {
        public void AddValue(Vector3 value)
        {
            if (value.SafeEquals(Vector3.zero))
                return;

            Vector3 prevValue = runtimeValue;

            runtimeValue += value;

            Callback_OnChangeValue(prevValue);
        }

        public override void SetValue(Vector3 value)
        {
            if (value.SafeEquals(runtimeValue))
                return;

            Vector3 prevValue = runtimeValue;

            runtimeValue = value;

            Callback_OnChangeValue(prevValue);
        }

        public void SubtractValue(Vector3 value)
        {
            if (value.SafeEquals(Vector3.zero))
                return;

            Vector3 prevValue = runtimeValue;

            runtimeValue -= value;

            Callback_OnChangeValue(prevValue);
        }
    }

}
