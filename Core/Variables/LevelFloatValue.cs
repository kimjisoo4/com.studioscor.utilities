using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
	public class LevelFloatValue
    {
		[Header(" [ Level Float Value ] ")]
		[SerializeField] private float _Min = 0f;
		[SerializeField] private float _Max = 1f;
		[SerializeField] private int _Round = 1;
		[SerializeField] private int _MaxLevel = 10;
		[SerializeField] private bool _UseCurve = true;
		[SerializeField] private AnimationCurve _Curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		[SerializeField] private float[] _Values;

		public float Min => _Min;
		public float Max => _Max;

		public LevelFloatValue(float min, float max, int round = 1, int maxLevel = 10, bool useCurve = true, AnimationCurve curve = null)
        {
			_Min = min;
			_Max = max;
			_Round = round;
			_MaxLevel = maxLevel;
			_UseCurve = useCurve;

			if (curve is null)
				_Curve = AnimationCurve.Linear(0, 0, 1, 1);
			else
				_Curve = curve;

			UpdateValue();
		}

		public float Get(int level)
        {
			if(_Values is null)
            {
				UpdateValue();
            }

			if (level < 0)
				return _Min;

			if (level >= _MaxLevel)
				return _Max;

			return _Values[level];
		}

		public void UpdateValue()
        {
			if(_MaxLevel == 0 || !_UseCurve)
            {
				return;
            }

			_Values = new float[_MaxLevel];

			_Values[0] = _Min;
			float step = _MaxLevel - 1;

			for(int i = 1; i < _MaxLevel; i++)
            {
				float value = i / step;
				float lerp = _Curve.Evaluate(value);

				_Values[i] = System.MathF.Round(Mathf.Lerp(_Min, _Max, lerp), _Round);
			}
        }
    }

}
