using Unity.VisualScripting;

namespace StudioScor.Utilities.Netcode.GameObjects.VisualScripting
{
    [UnitTitle("On Complated Spawn")]
    [UnitShortTitle("OnComplatedSpawn")]
    [UnitSubtitle("Events")]
    [UnitCategory("Events\\StudioScor\\NetcodeForGameObject")]
    public class OnComplatedSpawnUnit : NetworkEventUnit<EmptyEventArgs>
    {
        protected override string HookName => NetworkMessageListener.ON_COMPLATED_SPAWN;
    }
}
