using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new Bool Variable", fileName = "Variable_Bool_")]
    public class Variable_Bool : VariableObject<bool>
    {
        public override void SetValue(bool value)
        {
            if (value == _RuntimeValue)
                return;

            _RuntimeValue = value;

            Callback_OnChangeValue(!_RuntimeValue);
        }
    }
}
