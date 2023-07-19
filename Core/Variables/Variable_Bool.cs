using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new Bool Variable", fileName = "Variable_Bool_")]
    public class Variable_Bool : VariableObject<bool>
    {
        public override void SetValue(bool value)
        {
            if (value == runtimeValue)
                return;

            runtimeValue = value;

            Callback_OnChangeValue(!runtimeValue);
        }
    }
}
