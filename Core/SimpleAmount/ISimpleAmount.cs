namespace StudioScor.Utilities
{
    public interface ISimpleAmount
    {
        public void AddModifier(ISimpleAmountModifier modifier);
        public void RemoveModifier(ISimpleAmountModifier modifier);

        public void SetMaxValue(float maxValue);
        public void SetCurrentValue(float currentValue);
        public void SetValue(float currentValue, float maxValue);
        public void UpdateAmount();

        public float PrevValue { get; }
        public float CurrentValue { get; }
        public float MaxValue { get; }
        public float NormalizedValue { get; }
    }

}