using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimScor.Utilities
{
	[CreateAssetMenu(menuName = "Utilities/Variable/new Float Variable", fileName = "FloatVariable_")]
	public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
	{
		#region
		public delegate void ValueHandler(FloatVariable floatVariable, float currentValue, float prevValue);
        #endregion

        [SerializeField] private float _InitialValue;

		[SerializeField] private float _RuntimeValue;

		public float InitialValue => _InitialValue;

		public float Value => _RuntimeValue;


		public event ValueHandler OnChangedValue;

		public void OnAfterDeserialize()
		{
			_RuntimeValue = _InitialValue;
		}

		public void OnBeforeSerialize()
		{

		}


		public void AddValue(float value)
		{
			if (value <= 0)
				return;

			float prevValue = _RuntimeValue;
			_RuntimeValue += value;

			OnChangedValue?.Invoke(this, _RuntimeValue, prevValue);
		}
		public void SubtractValue(float value)
		{
			if (value <= 0)
				return;

			float prevValue = _RuntimeValue;
			_RuntimeValue -= value;

			OnChangedValue?.Invoke(this, _RuntimeValue, prevValue);
		}
		public void SetValue(float value)
		{
			if (value == _RuntimeValue)
				return;

			float prevValue = _RuntimeValue;
			_RuntimeValue = value;

			OnChangedValue?.Invoke(this, _RuntimeValue, prevValue);
		}
	}

}
