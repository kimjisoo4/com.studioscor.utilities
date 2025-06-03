using Unity.VisualScripting;

namespace StudioScor.Utilities.Netcode.GameObjects.VisualScripting
{
    [UnitTitle("Custom Rpc Event")]
    [UnitShortTitle("CustomRpcEvent")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\NetcodeForGameObject")]
    public class CustomRpcEventUnit : NetworkEventUnit<string>
    {
        protected override string HookName => NetworkMessageListener.ON_CUSTOM_RPC_EVENT;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput EventName;

        protected override bool ShouldTrigger(Flow flow, string eventName)
        {
            return CompareNames(flow, EventName, eventName);
        }

        protected override void Definition()
        {
            base.Definition();

            EventName = ValueInput<string>(nameof(EventName), null);
        }
    }

}