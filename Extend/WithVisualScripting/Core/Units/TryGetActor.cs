#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("Try Get Actor")]
    [UnitCategory("StudioScor\\Actor")]
    public class TryGetActor : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        [NullMeansSelf]
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
            Enter = ControlInput(nameof(Enter), GetActor);
            Target = ValueInput<GameObject>(nameof(Target), null).NullMeansSelf();
            IsNotNull = ControlOutput(nameof(IsNotNull));
            IsNull = ControlOutput(nameof(IsNull));

            Result = ValueOutput(typeof(IActor), nameof(Result), (flow) => flow.GetValue(Target));

            Succession(Enter, IsNull);
            Succession(Enter, IsNotNull);
        }


        private ControlOutput GetActor(Flow flow)
        {
            var obj = flow.GetValue<GameObject>(Target);


            if (obj is null || !obj.TryGetActor(out IActor actor))
            {
                flow.SetValue(Result, null);
                return IsNull;
            }
            else
            {
                flow.SetValue(Result, actor);
                return IsNotNull;
            }
        }
    }

}
#endif