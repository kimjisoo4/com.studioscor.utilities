using System;
using System.Text;
using Unity.Netcode;
using UnityEngine;


namespace StudioScor.Utilities.Netcode.GameObjects
{
    public static class SNetworkUtility
    {
        public const string LOCAL_ADDRESS = "127.0.0.1";

        private static string NetworkLogText(this NetworkBehaviour networkBehavior, object message)
        {
            StringBuilder sb = SUtility.GetStringBuilder();

            sb.Append(message);

            sb.Append(" [ ");

            if (networkBehavior.IsServer)
            {
                sb.Append(" In Server");
            }

            if (networkBehavior.IsClient)
            {
                if (networkBehavior.IsServer)
                {
                    sb.Append(" & Client");
                }
                else
                {
                    sb.Append(" In Client");
                }
            }

            if (networkBehavior.IsOwner)
            {
                sb.Append(" by Owner");
            }
            else
            {
                sb.Append(" by Other");
            }

            sb.Append(" ] ( Owner Client ID : ");
            sb.Append(networkBehavior.OwnerClientId);
            sb.Append(" )");

            return sb.ToString();
        }
        
        [System.Diagnostics.Conditional(SUtility.DEFINE_UNITY_EDITOR)]        
        public static void NetworkStateLog(this NetworkBehaviour networkBehavior, object message, string color = SUtility.STRING_COLOR_DEFAULT)
        {
            SUtility.Debug.Log(networkBehavior.NetworkLogText(message), networkBehavior, color);
        }
        
        [System.Diagnostics.Conditional(SUtility.DEFINE_UNITY_EDITOR)]
        public static void NetworkStateLogError(this NetworkBehaviour networkBehavior, object message, string color = SUtility.STRING_COLOR_ERROR)
        {
            SUtility.Debug.LogError(networkBehavior.NetworkLogText(message), networkBehavior, color);
        }
       
        public static bool IsInNetwork()
        {
            return NetworkManager.Singleton && NetworkManager.Singleton.RpcTarget is not null;
        }
    }
}

