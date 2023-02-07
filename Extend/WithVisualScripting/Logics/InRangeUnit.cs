using Unity.VisualScripting;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("InRange")]
    [UnitCategory("Logic\\StudioScor")]
    public sealed class InRangeUnit : Unit
    {
        [DoNotSerialize]
        [PortLabel("A")]
        public ValueInput A { get; private set; }

        [DoNotSerialize]
        [PortLabel("Value")]
        public ValueInput Value { get; private set; }

        [DoNotSerialize]
        [PortLabel("B")]
        public ValueInput B { get; private set; }

        [DoNotSerialize]
        [PortLabel("InRange")]
        public ValueOutput Result { get; private set; }

        [Serialize]
        [Inspectable]
        [InspectorToggleLeft]
        public bool Numeric { get; set; } = true;

        protected override void Definition()
        {
            if (Numeric)
            {
                Value = ValueInput<float>(nameof(Value));
                A = ValueInput<float>(nameof(A), 0.2f);
                B = ValueInput<float>(nameof(B), 0.8f);
                Result = ValueOutput<bool>(nameof(Result), NumericComparison).Predictable();
            }
            else
            {
                Value = ValueInput<object>(nameof(Value)).AllowsNull();
                A = ValueInput<object>(nameof(A)).AllowsNull();
                B = ValueInput<object>(nameof(B)).AllowsNull();
                Result = ValueOutput(nameof(Result), GenericComparison).Predictable();
            }

            Requirement(Value, Result);
            Requirement(A, Result);
            Requirement(B, Result);
        }

        private bool GenericComparison(Flow flow)
        {
            return GenericComparison(flow.GetValue(Value), flow.GetValue(A), flow.GetValue(B));
        }

        private bool NumericComparison(Flow flow)
        {
            return NumericComparison(flow.GetValue<float>(Value), flow.GetValue<float>(A), flow.GetValue<float>(B));
        }

        private bool NumericComparison(float value, float greater, float less)
        {
            return greater < value && value < less;
        }

        private bool GenericComparison(object value, object greater, object less)
        {
            return OperatorUtility.LessThan(greater, value) && OperatorUtility.GreaterThan(value, less);
        }
    }
}
