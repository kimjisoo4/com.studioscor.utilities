using Unity.VisualScripting;

namespace StudioScor.Utilities.Netcode.GameObjects.VisualScripting
{
    [UnitTitle("On Network Post Spawn")]
    [UnitShortTitle("OnNetworkPostSpawn")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\NetcodeForGameObject")]
    public class OnNetworkPostSpawnUnit : NetworkEventUnit<EmptyEventArgs>
    {
        protected override string HookName => NetworkMessageListener.ON_NETWORK_POST_SPAWN;
    }
}