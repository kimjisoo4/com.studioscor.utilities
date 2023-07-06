#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("On Changed Dilation")]
    [UnitShortTitle("OnChangedDilation")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\Dilation")]
    public class OnChangedDilationEvent : CustomInterfaceEventUnit<IDilationSystem, DilationEvent>
    {
        public override Type MessageListenerType => typeof(DilationMessageListener);
        protected override string HookName => DilationWithVisualScripting.ON_CHANGED_DILATION;

        [DoNotSerialize]
        [PortLabel("Current")]
        public ValueOutput Current;

        [DoNotSerialize]
        [PortLabel("Prev")]
        public ValueOutput Prev;

        protected override void Definition()
        {
            base.Definition();

            Current = ValueOutput<float>(nameof(Current));
            Prev = ValueOutput<float>(nameof(Prev));
        }

        protected override void AssignArguments(Flow flow, DilationEvent args)
        {
            base.AssignArguments(flow, args);

            flow.SetValue(Current, args.Current);
            flow.SetValue(Prev, args.Prev);
        }
    }
}

#endif