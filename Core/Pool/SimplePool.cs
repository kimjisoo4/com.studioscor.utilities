using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;


namespace StudioScor.Utilities
{
    public class SimplePool
    {
        public SimplePool(SimplePooledObject pooledObject, Transform container = null, int startSize = 5, int capacity = 10, int maxSize = 20)
        {
            this._pooledObject = pooledObject;
            this._container = container;

            _pool = new ObjectPool<SimplePooledObject>(Create, actionOnGet: Getted , actionOnRelease : Released, actionOnDestroy: Destroyed, defaultCapacity : capacity, maxSize: maxSize);

            var poolObjects = new List<SimplePooledObject>();

            for (int i = 0; i < startSize; i++)
            {
                poolObjects.Add(Get());
            }

            foreach (var poolObject in poolObjects)
            {
                poolObject.Release();
            }
        }

        private readonly SimplePooledObject _pooledObject;
        private readonly Transform _container;
        private readonly ObjectPool<SimplePooledObject> _pool;

        public SimplePooledObject PooledObject => _pooledObject;
        public ObjectPool<SimplePooledObject> Pool => _pool;
        public Transform Container => _container;

        protected virtual void Destroyed(SimplePooledObject pooledObject)
        {
            if (!pooledObject)
                return;

            UnityEngine.Object.Destroy(pooledObject.gameObject);
        }

        protected virtual SimplePooledObject Create()
        {
            SimplePooledObject pooledObject;

            if (_container)
            {
                pooledObject = UnityEngine.Object.Instantiate(PooledObject, _container);
            }
            else
            {
                pooledObject = UnityEngine.Object.Instantiate(PooledObject);
            }

            pooledObject.Create(this);

            return pooledObject;
        }

        public SimplePooledObject Get()
        {
            return Pool.Get();
        }

        private void Getted(SimplePooledObject pooledObject)
        {
        }

        public void Release(SimplePooledObject pooledObject)
        {
            Pool.Release(pooledObject);
        }

        public void Released(SimplePooledObject pooledObject)
        {
            if (_container && pooledObject.transform.parent != _container)
            {
                pooledObject.SetParent(_container, false);
            }
        }

        public void Clear()
        {
            if (_pool is null)
                return;

            _pool.Clear();
        }
    }
    
}