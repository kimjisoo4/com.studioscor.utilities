﻿using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new Int Variable", fileName = "Variable_Int_")]
    public class Variable_Int : VariableObject<int>
    {
        public override void SetValue(int value)
        {
			if (Value == value)
				return;

			int prevValue = Value;
			_runtimeValue = value;

			Callback_OnChangeValue(prevValue);
        }

		public void AddValue(int value)
        {
			if (value <= 0)
				return;

			SetValue(Value + value);
        }
		public void SubtractValue(int value)
        {
			if (value <= 0)
				return;

			SetValue(Value - value);
		}

		/// <summary>
		/// Value++;
		/// </summary>
		public void OnIncrease()
        {
			SetValue(Value + 1);
        }
		/// <summary>
		/// Value--;
		/// </summary>
		public void OnDecrease()
        {
			SetValue(Value - 1);
		}

	}

}
