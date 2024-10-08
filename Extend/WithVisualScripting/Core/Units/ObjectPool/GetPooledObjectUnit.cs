#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;
using UnityEngine;


namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("Get Pooled Object")]
    [UnitCategory("StudioScor\\Pool")]
    public class GetPooledObjectUnit : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput Exit { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput Target { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput Result { get; private set; }


        protected override void Definition()
        {
            Enter = ControlInput(nameof(Enter), GetPooledObject);
            Exit = ControlOutput(nameof(Exit));

            Target = ValueInput<PoolContainer>(nameof(Target), null);
            Result = ValueOutput<GameObject>(nameof(Result));
            
            Succession(Enter, Exit);
            Assignment(Enter, Result);
            Requirement(Target, Result);
        }

        private ControlOutput GetPooledObject(Flow flow)
        {
            var target = flow.GetValue<PoolContainer>(Target);

            flow.SetValue(Result, target.Get().gameObject);

            return Exit;
        }
    }
}
#endif