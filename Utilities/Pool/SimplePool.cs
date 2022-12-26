using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;


namespace StudioScor.Utilities
{

    public class SimplePool
    {
        public SimplePool(SimplePooledObject pooledObject, Transform container = null, int startSize = 5, int capacity = 10, int maxSize = 20)
        {
            _PooledObject = pooledObject;
            _Container = container;

            _Pool = new ObjectPool<SimplePooledObject>(Create, actionOnGet: Getted , actionOnDestroy: Destroyed, defaultCapacity : capacity, maxSize: maxSize);

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

        private readonly SimplePooledObject _PooledObject;
        private readonly Transform _Container;
        private readonly IObjectPool<SimplePooledObject> _Pool;
        public SimplePooledObject PooledObject => _PooledObject;
        public IObjectPool<SimplePooledObject> Pool => _Pool;


        protected virtual void Destroyed(SimplePooledObject pooledObject)
        {
            UnityEngine.Object.Destroy(pooledObject.gameObject);
        }

        protected virtual SimplePooledObject Create()
        {
            var pooledObject = UnityEngine.Object.Instantiate(PooledObject, _Container);

            pooledObject.Create(this);

            return pooledObject;
        }

        public SimplePooledObject Get()
        {
            return Pool.Get();
        }
        private void Getted(SimplePooledObject pooledObject)
        {
            pooledObject.Activate();
        }
        public void Released(SimplePooledObject pooledObject)
        {
            if (pooledObject.transform.parent != _Container)
            {
                pooledObject.SetParent(_Container, false);
            }

            Pool.Release(pooledObject);
        }
    }
    
}