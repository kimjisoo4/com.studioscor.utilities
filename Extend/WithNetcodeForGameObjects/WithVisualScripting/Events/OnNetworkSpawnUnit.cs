using Unity.VisualScripting;

namespace StudioScor.Utilities.Netcode.GameObjects.VisualScripting
{
    [UnitTitle("On Network Spawn")]
    [UnitShortTitle("OnNetworkSpawn")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\NetcodeForGameObject")]
    public class OnNetworkSpawnUnit : NetworkEventUnit<EmptyEventArgs>
    {
        protected override string HookName => NetworkMessageListener.ON_NETWORK_SPAWN;
    }
}