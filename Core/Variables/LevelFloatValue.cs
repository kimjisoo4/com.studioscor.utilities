using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
	public class LevelFloatValue
    {
		[Header(" [ Level Float Value ] ")]
		[SerializeField] private float _min = 0f;
		[SerializeField] private float _max = 1f;
		[SerializeField] private int _round = 1;
		[SerializeField] private int _maxLevel = 10;
		[SerializeField] private bool _useCurve = true;
		[SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		[SerializeField] private float[] _values;

		public float Min => _min;
		public float Max => _max;

		public LevelFloatValue(float min, float max, int round = 1, int maxLevel = 10, bool useCurve = true, AnimationCurve curve = null)
        {
			_min = min;
			_max = max;
			_round = round;
			_maxLevel = maxLevel;
			_useCurve = useCurve;

			if (curve is null)
				_curve = AnimationCurve.Linear(0, 0, 1, 1);
			else
				_curve = curve;

			UpdateValue();
		}

		public float Get(int level)
        {
			if(_values is null)
            {
				UpdateValue();
            }

			if (level < 0)
				return _min;

			if (level >= _maxLevel)
				return _max;

			return _values[level];
		}

		public void UpdateValue()
        {
			if(_maxLevel == 0)
            {
				return;
            }

			if(_useCurve)
			{
                _values = new float[_maxLevel];

                _values[0] = _min;
                float step = _maxLevel - 1;

                for (int i = 1; i < _maxLevel; i++)
                {
                    float value = i / step;
                    float lerp = _curve.Evaluate(value);

                    _values[i] = System.MathF.Round(Mathf.Lerp(_min, _max, lerp), _round);
                }
            }
			else
			{
                _values = new float[_maxLevel];

                _values[0] = _min;
                float step = _maxLevel - 1;

                for (int i = 1; i < _maxLevel; i++)
                {
                    float value = i / step;

                    _values[i] = System.MathF.Round(Mathf.Lerp(_min, _max, value), _round);
                }
            }
        }
    }

}
