using UnityEngine;
using UnityEngine.Events;


namespace StudioScor.Utilities
{
    public class PooledObject : BaseMonoBehaviour
    {
        public delegate void PooledObjectEventHandler(PooledObject pooledObject);

        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField] private UnityEvent _onCreated;
            [SerializeField] private UnityEvent _onActivated;
            [SerializeField] private UnityEvent _onReleased;

            public void EnableUnityEvents(PooledObject pooledObject)
            {
                pooledObject.OnCreated += PooledObject_OnCreated;
                pooledObject.OnActivated += PooledObject_OnActivated;
                pooledObject.OnReleased += PooledObject_OnReleased;
            }

          

            public void DisableUnityEvents(PooledObject pooledObject)
            {
                if(pooledObject)
                {
                    pooledObject.OnCreated -= PooledObject_OnCreated;
                    pooledObject.OnActivated -= PooledObject_OnActivated;
                    pooledObject.OnReleased -= PooledObject_OnReleased;
                }
            }
            private void PooledObject_OnReleased(PooledObject pooledObject)
            {
                _onReleased?.Invoke();
            }
            private void PooledObject_OnActivated(PooledObject pooledObject)
            {
                _onActivated?.Invoke();
            }
            private void PooledObject_OnCreated(PooledObject pooledObject)
            {
                _onCreated?.Invoke();
            }
        }

        [Header(" [ Simple Pooled Object ] ")]
        [SerializeField] PoolContainer _poolContainer;
        [Space(5f)]
        [SerializeField] private bool _useUnityEvent = false;
        [SerializeField] private UnityEvents _unityEvents;

        private SimplePool _pool;
        private bool _isActivated = false;

        public event PooledObjectEventHandler OnCreated;
        public event PooledObjectEventHandler OnActivated;
        public event PooledObjectEventHandler OnReleased;

        private void Awake()
        {
            if (_useUnityEvent)
            {
                _unityEvents.EnableUnityEvents(this);
            }
        }
        private void OnDestroy()
        {
            if (_useUnityEvent)
            {
                _unityEvents.DisableUnityEvents(this);
            }
        }

        internal void Create(SimplePool poolContainer)
        {
            Log(nameof(Create));

            _pool = poolContainer;

            if (_pool is not null)
            {
                Invoke_OnCreated();
            }
        }

        internal void OnActivate()
        {
            if (_isActivated)
                return;

            Log($"{nameof(OnActivate)}");

            _isActivated = true;

            gameObject.SetActive(true);

            Invoke_OnActivated();
        }
        internal void OnRelease()
        {
            if (!_isActivated)
                return;

            Log($"{nameof(OnRelease)}");

            _isActivated = false;

            gameObject.SetActive(false);

            Invoke_OnReleased();
        }

        public void Release()
        {
            if (!_isActivated)
                return;

            if (_pool is not null)
            {
                _pool.Release(this);
            }
            else if (_poolContainer)
            {
                _poolContainer.Release(this);
            }
            else
            {
                Log(" Pool Container Is Null!");
            }
        }

        private void Invoke_OnCreated()
        {
            Log($"{nameof(OnCreated)}");
            
            OnCreated?.Invoke(this);
        }
        private void Invoke_OnActivated()
        {
            Log($"{nameof(OnActivated)}");

            OnActivated?.Invoke(this);
        }
        private void Invoke_OnReleased()
        {
            Log($"{nameof(OnReleased)}");

            OnReleased?.Invoke(this);
        }
    }
    
}