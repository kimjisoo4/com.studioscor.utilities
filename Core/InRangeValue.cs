namespace StudioScor.Utilities
{
    public class InRangeValue : BaseClass
    {
        public delegate void InRangeValueStateEventHandler(InRangeValue inRangeValue);

        public float MinValue { get; set; }
        public float MaxValue { get; set; }

        private bool _inRange = false;
        public bool InRange => _inRange;

        public event InRangeValueStateEventHandler OnChangedInRange;

        public InRangeValue()
        {

        }
        public InRangeValue(float minValue, float maxValue)
        {
            Setup(minValue, maxValue);
        }

        public void Setup(float minValue, float maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public void UpdateValue(float value)
        {
            if(InRange)
            {
                if(MaxValue <= value)
                {
                    _inRange = false;

                    Invoke_OnChangedInRange();
                }
            }
            else
            {
                if (MinValue <= value)
                {
                    _inRange = true;

                    Invoke_OnChangedInRange();
                }
            }
        }

        private void Invoke_OnChangedInRange()
        {
            Log($"{nameof(OnChangedInRange)} :: InRange - {InRange}");

            OnChangedInRange?.Invoke(this);
        }
    }
}