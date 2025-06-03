using Unity.VisualScripting;

namespace StudioScor.Utilities.Netcode.GameObjects.VisualScripting
{
    [UnitTitle("On Network Despawn")]
    [UnitShortTitle("OnNetworkDespawn")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\NetcodeForGameObject")]
    public class OnNetworkDespawnUnit : NetworkEventUnit<EmptyEventArgs>
    {
        protected override string HookName => NetworkMessageListener.ON_NETWORK_DESPAWN;
    }
}