using UnityEngine;
using UnityEngine.Pool;


namespace StudioScor.Utilities
{
    public class SimplePool
    {
        public SimplePool(PooledObject pooledObject, Transform container = null, int startSize = 5, int capacity = 10, int maxSize = 20)
        {
            _pooledObject = pooledObject;
            _container = container;

            _pool = new ObjectPool<PooledObject>(OnCreated, actionOnGet: OnGetted , actionOnRelease : OnReleased, actionOnDestroy: OnDestroyed, defaultCapacity : capacity, maxSize: maxSize);

            for(int i = 0; i < startSize; i++)
            {
                var startPooledObject = OnCreated();

                startPooledObject.gameObject.SetActive(false);

                _pool.Release(startPooledObject);
            }
        }

        private readonly PooledObject _pooledObject;
        private readonly Transform _container;
        private readonly ObjectPool<PooledObject> _pool;

        public PooledObject PooledObject => _pooledObject;
        public ObjectPool<PooledObject> Pool => _pool;
        public Transform Container => _container;

        protected virtual void OnDestroyed(PooledObject pooledObject)
        {
            if (!pooledObject)
                return;

            UnityEngine.Object.Destroy(pooledObject.gameObject);
        }

        protected virtual PooledObject OnCreated()
        {
            PooledObject pooledObject;

            if (_container)
            {
                pooledObject = Object.Instantiate(PooledObject, _container);
            }
            else
            {
                pooledObject = Object.Instantiate(PooledObject);
            }

            pooledObject.Create(this);

            return pooledObject;
        }

        public PooledObject Get()
        {
            return Pool.Get();
        }
        public void Release(PooledObject pooledObject)
        {
            Pool.Release(pooledObject);
        }

        private void OnGetted(PooledObject pooledObject)
        {
            pooledObject.OnActivate();
        }

        public void OnReleased(PooledObject pooledObject)
        {
            if (_container && pooledObject.transform.parent != _container)
            {
                pooledObject.transform.SetParent(_container, false);
            }

            pooledObject.OnRelease();
        }

        public void Clear()
        {
            if (_pool is null)
                return;

            _pool.Clear();
        }
    }
    
}