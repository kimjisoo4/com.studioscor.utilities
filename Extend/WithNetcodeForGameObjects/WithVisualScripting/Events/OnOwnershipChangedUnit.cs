using Unity.VisualScripting;

namespace StudioScor.Utilities.Netcode.GameObjects.VisualScripting
{
    [UnitTitle("On Ownership Changed")]
    [UnitShortTitle("OnOwnershipChanged")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\NetcodeForGameObject")]
    public class OnOwnershipChangedUnit : NetworkEventUnit<NetworkMessageListener.FOwnershipChanged>
    {
        protected override string HookName => NetworkMessageListener.ON_OWNERSHIP_CHANGED;

        [DoNotSerialize]
        [PortLabel("Previous")]
        public ValueOutput Previous;

        [DoNotSerialize]
        [PortLabel("Current")]
        public ValueOutput Current;

        protected override void Definition()
        {
            base.Definition();

            Previous = ValueOutput<ulong>(nameof(Previous));
            Current = ValueOutput<ulong>(nameof(Current));
        }

        protected override void AssignArguments(Flow flow, NetworkMessageListener.FOwnershipChanged ownershipChanged)
        {
            base.AssignArguments(flow, ownershipChanged);

            flow.SetValue(Previous, ownershipChanged.Previous);
            flow.SetValue(Current, ownershipChanged.Current);
        }
    }
}