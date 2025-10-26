using UnityEngine;

namespace StudioScor.Utilities
{
    public class InRangeValue : BaseClass
    {
        public delegate void InRangeValueStateEventHandler(InRangeValue inRangeValue);
        [field: SerializeField] public Vector2 Range { get; set; }
        public bool InRange { get; private set; }
        public bool IsPlaying { get; private set; }

        public event InRangeValueStateEventHandler OnValueChanged;

        public void Dispose()
        {
            OnValueChanged = null;
        }

        public void Start()
        {
            if (IsPlaying)
                return;

            IsPlaying = true;

            InRange = false;
        }
        public void End()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;

            if (InRange)
            {
                InRange = false;
                RaiseOnValueChanged();
            }

        }

        public void UpdateValue(float value)
        {
            var prevValue = InRange;
            InRange = value.InRange(Range);

            if (InRange != prevValue)
            {
                RaiseOnValueChanged();
            }
        }

        private void RaiseOnValueChanged()
        {
            OnValueChanged?.Invoke(this);
        }
    }
}