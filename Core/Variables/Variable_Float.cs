using UnityEngine;

namespace StudioScor.Utilities
{

    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new Float Variable", fileName = "Variable_Float_")]
	public class Variable_Float : VariableObject<float>
	{
        public void AddValue(float value)
        {
			if (value <= 0)
				return;

			SetValue(Value + value);
		}

        public void SubtractValue(float value)
        {
			if (value <= 0)
				return;

			SetValue(Value - value);
		}

        public override void SetValue(float value)
        {
			if (value == runtimeValue)
				return;

			float prevValue = runtimeValue;
			runtimeValue = value;

			Callback_OnChangeValue(prevValue);
		}
    }

}
