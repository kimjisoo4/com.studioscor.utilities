using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimScor.Utilities
{

    [CreateAssetMenu(menuName = "Utilities/Variable/new Float Variable", fileName = "FloatVariable_")]
	public class FloatVariable : VariableObject<float>
	{
        public override void AddValue(float value)
        {
			if (value <= 0)
				return;

			float prevValue = _RuntimeValue;
			_RuntimeValue += value;

			OnChangeValue(prevValue);
		}

        public override void SubtractValue(float value)
        {
			if (value <= 0)
				return;

			float prevValue = _RuntimeValue;
			_RuntimeValue -= value;

			OnChangeValue(prevValue);
		}

        public override void SetValue(float value)
        {
			if (value == _RuntimeValue)
				return;

			float prevValue = _RuntimeValue;
			_RuntimeValue = value;

			OnChangeValue(prevValue);
		}
    }

}
