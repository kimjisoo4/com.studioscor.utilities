#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("Released Pooled Object")]
    [UnitCategory("StudioScor\\Pool")]
    public class ReleasedPooledObject : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput Exit { get; private set; }

        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabelHidden]
        public ValueInput Target { get; private set; }


        protected override void Definition()
        {
            Target = ValueInput<GameObject>(nameof(Target), null).NullMeansSelf();

            Enter = ControlInput(nameof(Enter), OnRelease);
            Exit = ControlOutput(nameof(Exit));

            Succession(Enter, Exit);
            Requirement(Target, Enter);
        }

        private ControlOutput OnRelease(Flow flow)
        {
            var pool = flow.GetValue<PooledObject>(Target);

            pool.Release();

            return Exit;
        }
    }
}
#endif