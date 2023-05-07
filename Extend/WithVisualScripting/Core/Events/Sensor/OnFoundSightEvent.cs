#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using System;
using UnityEngine;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("On Found Sight")]
    [UnitShortTitle("OnFoundSight")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\Sensor")]
    public class OnFoundSightEvent : CustomInterfaceEventUnit<ISight, Collider>
    {
        public override Type MessageListenerType => typeof(SightMessageListener);
        protected override string HookName => SightWithVisualScripting.ON_FOUND_SIGHT;

        [DoNotSerialize]
        [PortLabel("Sight")]
        public ValueOutput Sight;

        protected override void Definition()
        {
            base.Definition();

            Sight = ValueOutput<Collider>(nameof(Sight));
        }

        protected override void AssignArguments(Flow flow, Collider sight)
        {
            base.AssignArguments(flow, sight);

            flow.SetValue(Sight, sight);
        }
    }
}

#endif