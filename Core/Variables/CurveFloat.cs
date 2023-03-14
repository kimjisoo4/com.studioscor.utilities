using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
	public class CurveFloat
    {
		[Header(" [ Curve Float ] ")]
		[SerializeField] private float _MinValue = 0f;
		[SerializeField] private float _MaxValue = 1f;
		[SerializeField] private int _Round = 1;
		[SerializeField] private int _Step = 10;
		[SerializeField] private AnimationCurve _Curve;
		[SerializeField] private float[] _Values;

		public CurveFloat(float minValue, float maxValue, int round = 1, int step = 10, AnimationCurve curve = null)
        {
			_MinValue = minValue;
			_MaxValue = maxValue;
			_Round = round;
			_Step = step;
			_Curve = curve;

			if (_Curve is null)
				_Curve = AnimationCurve.Linear(0, 0, 1, 1);
		
			UpdateValue();
		}

		public float GetValue(int step)
        {
			if(_Values is null)
            {
				UpdateValue();
            }

			if (step < 0)
				return _MinValue;

			if (step >= _Step)
				return _MaxValue;

			return _Values[step];
		}

		public void UpdateValue()
        {
			_Values = new float[_Step];

			_Values[0] = _MinValue;
			float step = _Step - 1;

			for(int i = 1; i < _Step; i++)
            {
				float value = i / step;
				float lerp = _Curve.Evaluate(value);

				_Values[i] = System.MathF.Round(Mathf.Lerp(_MinValue, _MaxValue, lerp), _Round);
			}
        }
    }

}
