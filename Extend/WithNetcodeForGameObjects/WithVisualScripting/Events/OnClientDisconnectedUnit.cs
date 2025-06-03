using Unity.VisualScripting;
using System;
using StudioScor.Utilities.VisualScripting;

namespace StudioScor.Utilities.Netcode.GameObjects.VisualScripting
{
    [UnitTitle("On Client Disconnected")]
    [UnitShortTitle("OnClientDisconnected")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\NetcodeForGameObject")]
    public class OnClientDisconnectedUnit : GameObjectCustomEventUnit<ulong>
    {
        public override Type MessageListenerType => typeof(NetworkManagerMessageListener);
        protected override string HookName => NetworkManagerMessageListener.ON_CLIENT_DISCONNECT;

        [DoNotSerialize]
        [PortLabel(nameof(ClientID))]
        public ValueOutput ClientID;

        protected override void Definition()
        {
            base.Definition();

            ClientID = ValueOutput<ulong>(nameof(ClientID));
        }
        protected override void AssignArguments(Flow flow, ulong clientID)
        {
            base.AssignArguments(flow, clientID);

            flow.SetValue(ClientID, clientID);
        }
    }
}