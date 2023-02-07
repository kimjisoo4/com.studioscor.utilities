using Unity.VisualScripting;

namespace StudioScor.Utilities.VisualScripting
{
    [UnitTitle("On Draw Gizmos Selected Activate")]
    [UnitShortTitle("OnDrawGizomsSelectedActivate")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\Editor")]
    [TypeIcon(typeof(OnDrawGizmosSelected))]
    public sealed class OnDrawGizmosSelectedActivatedEventUnit : ManualEventUnit<EmptyEventArgs>
    {
        protected override string hookName => EventHooks.OnDrawGizmosSelected;

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
