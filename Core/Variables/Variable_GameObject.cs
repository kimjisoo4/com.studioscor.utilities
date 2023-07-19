using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new GameObject Variable", fileName = "Variable_GameObject_")]
    public class Variable_GameObject : VariableObject<GameObject>
    {
        public override void SetValue(GameObject value)
        {
            if (runtimeValue == value)
                return;

            var prevValue = runtimeValue;
            runtimeValue = value;

            Callback_OnChangeValue(prevValue);
        }
    }

}
