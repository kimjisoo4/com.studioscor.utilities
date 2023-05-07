#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("On Draw Gizmos Activate")]
    [UnitShortTitle("OnDrawGizomsActivate")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\Editor")]
    [TypeIcon(typeof(OnDrawGizmos))]
    public sealed class OnDrawGizmosActivatedEventUnit : ManualEventUnit<EmptyEventArgs>
    {
        protected override string hookName => EventHooks.OnDrawGizmos;

        private bool _IsListning = false;
        public override void StartListening(GraphStack stack)
        {
            base.StartListening(stack);

            _IsListning = true;
        }
        public override void StopListening(GraphStack stack)
        {
            base.StopListening(stack);

            _IsListning = false;
        }
        protected override bool ShouldTrigger(Flow flow, EmptyEventArgs args)
        {
            return _IsListning || flow.isInspected;
        }
    }
}
#endif