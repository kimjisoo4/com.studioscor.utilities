using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new Color Variable", fileName = "Variable_Color_")]
    public class Variable_Color : VariableObject<Color>
    {
        public override void SetValue(Color value)
        {
            if (_runtimeValue == value)
                return;

            Color prevValue = _runtimeValue;

            _runtimeValue = value;

            Callback_OnChangeValue(prevValue);
        }
    }

}
