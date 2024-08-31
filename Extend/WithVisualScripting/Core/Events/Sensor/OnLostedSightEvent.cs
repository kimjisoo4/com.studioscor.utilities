#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using System;
using UnityEngine;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("On Losted Sight")]
    [UnitShortTitle("OnLostedSight")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\Sensor")]
    public class OnLostedSightEvent : CustomInterfaceEventUnit<ISightSensor, ISightTarget>
    {
        public override Type MessageListenerType => typeof(SightMessageListener);
        protected override string HookName => SightWithVisualScripting.ON_LOSTED_SIGHT;

        [DoNotSerialize]
        [PortLabel("Sight")]
        public ValueOutput Sight;

        protected override void Definition()
        {
            base.Definition();

            Sight = ValueOutput<Collider>(nameof(Sight));
        }

        protected override void AssignArguments(Flow flow, ISightTarget sight)
        {
            base.AssignArguments(flow, sight);

            flow.SetValue(Sight, sight);
        }
    }
}

#endif