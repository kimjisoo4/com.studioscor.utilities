using System;
using Unity.VisualScripting;
using StudioScor.Utilities.VisualScripting;

namespace StudioScor.Utilities.Netcode.GameObjects.VisualScripting
{
    [UnitTitle("On Client Connected")]
    [UnitShortTitle("OnClientConnected")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\NetcodeForGameObject")]
    public class OnClientConnectedUnit : GameObjectCustomEventUnit<ulong>
    {
        protected override string HookName => NetworkManagerMessageListener.ON_CLIENT_CONNECTED;
        public override Type MessageListenerType => typeof(NetworkManagerMessageListener);


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