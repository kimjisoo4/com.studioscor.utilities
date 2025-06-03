using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace StudioScor.Utilities.Netcode.GameObjects
{
    public class BaseNetworkBehaviour : NetworkBehaviour
    {
        public delegate void NetworkEventHandler();
        public delegate void ChangedOwnershipEventHandler(ulong prevOwnerID, ulong currentOwnerID);

        public bool ActiveSelf { get; private set; } = true;

        private HashSet<ulong> _spawnedClients;

        public event ChangedOwnershipEventHandler OnChangedOwnership;


        #region EDITOR ONLY
#if UNITY_EDITOR
        [Header(" [ Base Network Behaviour ] ")]
        [Header(" Use Debug ")]
        public bool UseDebug;
#else
        [HideInInspector] public bool UseDebug = false;
#endif

        [System.Diagnostics.Conditional(SUtility.DEFINE_UNITY_EDITOR)]
        protected virtual void Log(object log, string color = SUtility.STRING_COLOR_GREY)
        {
#if UNITY_EDITOR
            if (UseDebug)
                this.NetworkStateLog($"{GetType().Name} [{name}] : {log}", color);
#endif
        }

        [System.Diagnostics.Conditional(SUtility.DEFINE_UNITY_EDITOR)]
        protected virtual void LogError(object log, string color = SUtility.STRING_COLOR_GREY)
        {
#if UNITY_EDITOR
            this.NetworkStateLogError($"{GetType().Name} [{name}] : {log}", color);
#endif
        }
        #endregion


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if(IsServer)
            {
                NetworkManager.OnClientConnectedCallback += OnClientConnected;
            }
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            if (IsServer)
            {
                NetworkManager.OnClientConnectedCallback -= OnClientConnected;
            }
        }

        protected override void OnNetworkPostSpawn()
        {
            base.OnNetworkPostSpawn();

            if (SNetworkUtility.IsInNetwork())
            {
                if (IsServer)
                {
                    if (_spawnedClients is null)
                        _spawnedClients = new();

                    _spawnedClients.Add(NetworkManager.LocalClientId);

                    if (_spawnedClients.Count == NetworkManager.ConnectedClients.Count)
                    {
                        SpawnComplated();
                        OnComplatedSpawnNotServerRpc();

                        _spawnedClients = null;
                    }
                }
                else
                {
                    NotifyComplatedSpawnServerRpc(NetworkManager.LocalClientId);
                }
            }
            else
            {
                SpawnComplated();
            }
        }

        [Rpc(SendTo.Server)]
        private void NotifyComplatedSpawnServerRpc(ulong clientID)
        {
            if (_spawnedClients is null)
                _spawnedClients = new();

            if (_spawnedClients.Contains(clientID))
                return;

            _spawnedClients.Add(clientID);

            if (_spawnedClients.Count == NetworkManager.ConnectedClients.Count)
            {
                SpawnComplated();
                OnComplatedSpawnNotServerRpc();

                _spawnedClients = null;
            }
        }

        [Rpc(SendTo.NotServer)]
        private void OnComplatedSpawnNotServerRpc()
        {
            SpawnComplated();
        }

        private void SpawnComplated()
        {
            Log($"{nameof(SpawnComplated)}");

            OnSpawnComplated();
        }
        protected virtual void OnSpawnComplated()
        {

        }


        public void SetActive(bool enabled)
        {
            if(SNetworkUtility.IsInNetwork())
            {
                if (IsServer)
                {
                    ChangedActive(enabled);
                    ChangedActiveNotServerRpc(enabled);
                }
                else
                {
                    ChangedActiveServerRpc(enabled);
                }
            }
            else
            {
                ChangedActive(enabled);
            }
        }

        private void ChangedActive(bool enabled)
        {
            ActiveSelf = enabled;

#if UNITY_EDITOR
            const string enableText = "[Enable]";
            const string disableText = "[Disable]";

            if (name.Contains(enableText))
            {
                if(!enabled)
                {
                    name = name.Replace(enableText, disableText);
                }
            }
            else if (name.Contains(disableText))
            {
                if (enabled)
                {
                    name = name.Replace(disableText, enableText);
                }
            }
            else
            {
                name = (enabled ? enableText : disableText) + name;
            }
#endif

            OnChangedActivate(enabled);
        }

        protected virtual void OnChangedActivate(bool activeSelf)
        {

        }

        [Rpc(SendTo.Server)]
        private void ChangedActiveServerRpc(bool enabled)
        {
            ChangedActive(enabled);

            if (SNetworkUtility.IsInNetwork())
                ChangedActiveNotServerRpc(enabled);
        }
        [Rpc(SendTo.NotServer)]
        private void ChangedActiveNotServerRpc(bool enabled)
        {
            ChangedActive(enabled);
        }

        [Rpc(SendTo.SpecifiedInParams)]
        private void SetActiveTargetRpc(bool enabled, RpcParams rpcParams = default)
        {
            ChangedActive(enabled);
        }


        protected override void OnOwnershipChanged(ulong previous, ulong current)
        {
            base.OnOwnershipChanged(previous, current);

            OnChangedOwnership?.Invoke(previous, current);
        }


        private void OnClientConnected(ulong clientID)
        {
            SetActiveTargetRpc(ActiveSelf, RpcTarget.Single(clientID, RpcTargetUse.Temp));
        }

    }
}
