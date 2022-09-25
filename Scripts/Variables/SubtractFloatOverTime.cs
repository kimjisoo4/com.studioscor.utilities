using UnityEngine;

namespace KimScor.Utilities
{

    [System.Serializable]
    public class SubtractFloatOverTime
    {
		#region
		public delegate void ChangeState(SubtractFloatOverTime subtractHealthOverTime);
		public delegate void ChangeStrength(SubtractFloatOverTime subtractHealthOverTime, float currentStrength, float prevStrength);
        #endregion

		[SerializeField] private bool _UseSubtract = true;
		[SerializeField] private float _Strength = 1f;

		private float _SubtractValue = 0f;

		public bool UseSubtract => _UseSubtract;
		public float Strength => _Strength;
		public float SubtractValue => _SubtractValue;

		public event ChangeState OnChangedState;
		public event ChangeStrength OnChangedStrength;

		public SubtractFloatOverTime(bool useSubtract = true, float strength = 1f)
        {
			_UseSubtract = useSubtract;
			_Strength = strength;
        }

		public void SetUseSubtract(bool useSubtract)
        {
			if (_UseSubtract == useSubtract)
				return;

			_UseSubtract = useSubtract;

			OnChangedState?.Invoke(this);

		}

		public void SetStrength(float newStrength)
        {
			if (_Strength == newStrength)
				return;

			float prevStrength = _Strength;
			_Strength = newStrength;

			OnChangedStrength?.Invoke(this, _Strength, prevStrength);
		}

        public float UpdateOverTime(float deltaTime)
        {
			if (!_UseSubtract)
            {
				_SubtractValue = 0f;
				
				return 0f;
			}
				

			float value = deltaTime * _Strength;

			_SubtractValue = value;

			return _SubtractValue;
		}
    }

}
