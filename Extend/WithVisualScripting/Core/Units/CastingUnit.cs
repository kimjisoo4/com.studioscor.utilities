#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using System;

namespace StudioScor.Utilities.VisualScripting
{

    [UnitTitle("Cast")]
    [UnitCategory("Nulls\\StudioScor")]
    public class CastingUnit : Unit
    {
        [Serialize]
        [Inspectable]
        [UnitHeaderInspectable]
        public Type CastType = typeof(object);

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Target { get; private set; }


        [DoNotSerialize]
        [PortLabel("IsNotNull")]
        public ControlOutput IsNotNull { get; private set; }

        [DoNotSerialize]
        [PortLabel("IsNull")]
        public ControlOutput IsNull { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput Result { get; private set; }


        protected override void Definition()
        {
            Enter = ControlInput(nameof(Enter), TryCasting);
            Target = ValueInput<object>(nameof(Target));
            IsNotNull = ControlOutput(nameof(IsNotNull));
            IsNull = ControlOutput(nameof(IsNull));

            Result = ValueOutput(CastType, nameof(Result), (flow) => flow.GetValue(Target));

            Succession(Enter, IsNull);
            Succession(Enter, IsNotNull);
        }


        private ControlOutput TryCasting(Flow flow)
        {
            var obj = flow.GetValue(Target).ConvertTo(CastType);

            if(obj is null)
            {
                return IsNull;
            }
            else
            {
                return IsNotNull;
            }
        }
    }


}
#endif