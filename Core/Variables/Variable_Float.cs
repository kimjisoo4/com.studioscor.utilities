using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
			if (value == _RuntimeValue)
				return;

			float prevValue = _RuntimeValue;
			_RuntimeValue = value;

			Callback_OnChangeValue(prevValue);
		}
    }

}
