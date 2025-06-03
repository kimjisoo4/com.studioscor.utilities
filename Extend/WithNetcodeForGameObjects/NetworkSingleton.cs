using Unity.Netcode;
using UnityEngine;

namespace StudioScor.Utilities.Netcode.GameObjects
{
    [DefaultExecutionOrder(-1)]
    public class NetworkSingleton<T> : BaseNetworkBehaviour where T : NetworkBehaviour
    {
        [Header(" [ Network Singleton ] ")]
        [SerializeField] private bool _dontDestroyOnLoad;

        private static T _singleton;
        public static T Singleton
        {
            get
            {
                if (!_singleton)
                {
                    _singleton = FindAnyObjectByType<T>();
                }

                return _singleton;
            }
        }

        private void Awake()
        {
            if (Singleton && Singleton != this)
            {
                Destroy(gameObject);
                return;
            }

            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            OnInit();
        }

        protected virtual void OnInit()
        {

        }
    }
}

