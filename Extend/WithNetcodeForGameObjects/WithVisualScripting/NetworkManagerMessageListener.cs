using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;



namespace StudioScor.Utilities.Netcode.GameObjects.VisualScripting
{
    [DisableAnnotation]
    [AddComponentMenu("")]
    [IncludeInSettings(false)]
    public class NetworkManagerMessageListener : MessageListener
    {
        public const string ON_CLIENT_CONNECTED = "OnClientConnected";
        public const string ON_CLIENT_DISCONNECT = "OnClientDisconnect";


        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;
        }
        private void OnDestroy()
        {
            if (NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback -= Singleton_OnClientDisconnectCallback;
            }
        }

        private void Singleton_OnClientConnectedCallback(ulong clientID)
        {
            EventBus.Trigger(new EventHook(ON_CLIENT_CONNECTED, gameObject), clientID);
        }
        private void Singleton_OnClientDisconnectCallback(ulong clientID)
        {
            EventBus.Trigger(new EventHook(ON_CLIENT_DISCONNECT, gameObject), clientID);
        }
    }
}