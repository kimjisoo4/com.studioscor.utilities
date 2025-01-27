using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new GameObject Variable", fileName = "Variable_GameObject_")]
    public class Variable_GameObject : VariableObject<GameObject>
    {
        public override void SetValue(GameObject value)
        {
            if (_runtimeValue == value)
                return;

            var prevValue = _runtimeValue;
            _runtimeValue = value;

            Callback_OnChangeValue(prevValue);
        }
    }

}
