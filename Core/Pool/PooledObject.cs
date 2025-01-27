using UnityEngine;


namespace StudioScor.Utilities
{
    public class PooledObject : BaseMonoBehaviour
    {
        public enum EPooledState
        {
            None,
            Activated,
            Released,
        }
        public delegate void PooledObjectEventHandler(PooledObject pooledObject);

        [Header(" [ Simple Pooled Object ] ")]
        [SerializeField] PoolContainer _poolContainer;
        [SerializeField] private ToggleableUnityEvent _onCreated;
        [SerializeField] private ToggleableUnityEvent _onActivated;
        [SerializeField] private ToggleableUnityEvent _onReleased;

        private SimplePool _pool;
        private EPooledState _state;

        public event PooledObjectEventHandler OnCreated;
        public event PooledObjectEventHandler OnActivated;
        public event PooledObjectEventHandler OnReleased;


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
            if (_state == EPooledState.Activated)
                return;

            Log($"{nameof(OnActivate)}");

            _state = EPooledState.Activated;

            gameObject.SetActive(true);

            Invoke_OnActivated();
        }
        internal void OnRelease()
        {
            if (_state == EPooledState.Released)
                return;

            Log($"{nameof(OnRelease)}");

            _state = EPooledState.Released;

            gameObject.SetActive(false);

            Invoke_OnReleased();
        }

        public void Release()
        {
            if (_state == EPooledState.Released)
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

            _onCreated.Invoke();
            OnCreated?.Invoke(this);
        }
        private void Invoke_OnActivated()
        {
            Log($"{nameof(OnActivated)}");

            _onActivated.Invoke();
            OnActivated?.Invoke(this);
        }
        private void Invoke_OnReleased()
        {
            Log($"{nameof(OnReleased)}");

            _onReleased.Invoke();
            OnReleased?.Invoke(this);
        }
    }
    
}