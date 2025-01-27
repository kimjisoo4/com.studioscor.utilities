using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new LayerMask Variable", fileName = "Variable_LayerMask_")]
    public class Variable_LayerMask : VariableObject<LayerMask>
    {
        public override void SetValue(LayerMask value)
        {
            if (value.value == _runtimeValue.value)
                return;

            var prev = value;
            _runtimeValue = value;

            Callback_OnChangeValue(prev);
        }
    }
}
