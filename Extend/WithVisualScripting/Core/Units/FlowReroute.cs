#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using System;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitCategory("StudioScor\\Utilities")]
    public sealed class FlowReroute : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter;
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput Exit;

        protected override void Definition()
        {
            Enter = ControlInput(nameof(Enter), (flow) => { return Exit; });
            Exit = ControlOutput(nameof(Exit));

            Succession(Enter, Exit);
        }
    }

    [UnitCategory("StudioScor\\Utilities")]
    public sealed class ValueReroute : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Enter;
        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput Exit;
        [Serialize]
        public Type portType = typeof(object);

        protected override void Definition()
        {
            Enter = ValueInput(portType, nameof(Enter));
            Exit = ValueOutput(portType, nameof(Exit), (flow) => { return flow.GetValue(Enter); });
            Requirement(Enter, Exit);
        }
    }


}
#endif