using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace StudioScor.Utilities.Netcode.GameObjects.VisualScripting
{
    [DisableAnnotation]
    [AddComponentMenu("")]
    [IncludeInSettings(false)]
    public class NetworkMessageListener : BaseNetworkBehaviour
    {
        public struct FOwnershipChanged
        {
            public ulong Previous;
            public ulong Current;

            public FOwnershipChanged(ulong previous, ulong current)
            {
                Previous = previous;
                Current = current;
            }
        }

        public const string ON_NETWORK_SPAWN = "OnNetworkSpawn";
        public const string ON_NETWORK_POST_SPAWN = "OnNetworkPostSpawn";
        public const string ON_COMPLATED_SPAWN = "OnComplatedSpawn";
        public const string ON_CHANGED_ACTIVATE = "OnChangedActivate";
        public const string ON_NETWORK_DESPAWN = "OnNetworkDespawn";
        public const string ON_OWNERSHIP_CHANGED = "OnOwnershipChanged";
        public const string ON_CUSTOM_RPC_EVENT = "OnCustomRpcEvent";

        private NetworkObject _networkObject;

        private void Awake()
        {
            _networkObject = GetComponent<NetworkObject>();
        }
        public static NetworkMessageListener AddTo(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out NetworkMessageListener networkMessageListener))
                return gameObject.AddComponent<NetworkMessageListener>();

            return networkMessageListener;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            EventBus.Trigger(new EventHook(ON_NETWORK_SPAWN, _networkObject));
        }
        protected override void OnNetworkPostSpawn()
        {
            base.OnNetworkPostSpawn();

            EventBus.Trigger(new EventHook(ON_NETWORK_POST_SPAWN, _networkObject));
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            EventBus.Trigger(new EventHook(ON_NETWORK_DESPAWN, _networkObject));
        }
        protected override void OnOwnershipChanged(ulong previous, ulong current)
        {
            base.OnOwnershipChanged(previous, current);

            EventBus.Trigger(new EventHook(ON_OWNERSHIP_CHANGED, _networkObject), new FOwnershipChanged(previous, current));
        }
        protected override void OnSpawnComplated()
        {
            base.OnSpawnComplated();

            EventBus.Trigger(new EventHook(ON_COMPLATED_SPAWN, _networkObject));
        }

        private void TriggerEvent(string name)
        {
            EventBus.Trigger(new EventHook(ON_CUSTOM_RPC_EVENT, _networkObject), name);
        }


        public void TriggerRpcEvent(string name, SendTo rpc, RpcParams rpcParams = default)
        {
            if (!SNetworkUtility.IsInNetwork())
                return;

            switch (rpc)
            {
                case SendTo.Owner:
                    OwnerRpc(name);
                    break;
                case SendTo.NotOwner:
                    NotOwnerRpc(name);
                    break;
                case SendTo.Server:
                    ServerRpc(name);
                    break;
                case SendTo.NotServer:
                    NotServerRpc(name);
                    break;
                case SendTo.Me:
                    MeRpc(name);
                    break;
                case SendTo.NotMe:
                    NotMeRpc(name);
                    break;
                case SendTo.Everyone:
                    EveryoneRpc(name);
                    break;
                case SendTo.ClientsAndHost:
                    ClientRpc(name);
                    break;
                case SendTo.Authority:
                    AuthorityRpc(name);
                    break;
                case SendTo.NotAuthority:
                    NotAuthorityRpc(name);
                    break;
                case SendTo.SpecifiedInParams:
                    TargetRpc(name, rpcParams);
                    break;
                default:
                    break;
            }
        }


        [Rpc(SendTo.Server)]
        private void ServerRpc(string name) => TriggerEvent(name);
        [Rpc(SendTo.NotServer)]
        private void NotServerRpc(string name) => TriggerEvent(name);
        [Rpc(SendTo.Owner)]
        private void OwnerRpc(string name) => TriggerEvent(name);
        [Rpc(SendTo.NotOwner)]
        private void NotOwnerRpc(string name) => TriggerEvent(name);
        [Rpc(SendTo.ClientsAndHost)]
        private void ClientRpc(string name) => TriggerEvent(name);
        [Rpc(SendTo.Everyone)]
        private void EveryoneRpc(string name) => TriggerEvent(name);
        [Rpc(SendTo.SpecifiedInParams)]
        private void TargetRpc(string name, RpcParams rpcParams = default) => TriggerEvent(name);
        [Rpc(SendTo.Me)]
        private void MeRpc(string name) => TriggerEvent(name);
        [Rpc(SendTo.NotMe)]
        private void NotMeRpc(string name) => TriggerEvent(name);
        [Rpc(SendTo.Authority)]
        private void AuthorityRpc(string name) => TriggerEvent(name);
        [Rpc(SendTo.NotAuthority)]
        private void NotAuthorityRpc(string name) => TriggerEvent(name);

    }
}