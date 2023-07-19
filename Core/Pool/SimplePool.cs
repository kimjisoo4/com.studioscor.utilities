using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;


namespace StudioScor.Utilities
{

    public class SimplePool
    {
        public SimplePool(SimplePooledObject pooledObject, Transform container = null, int startSize = 5, int capacity = 10, int maxSize = 20)
        {
            this.pooledObject = pooledObject;
            this.container = container;

            pool = new ObjectPool<SimplePooledObject>(Create, actionOnGet: Getted , actionOnRelease : Released, actionOnDestroy: Destroyed, defaultCapacity : capacity, maxSize: maxSize);

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

        private readonly SimplePooledObject pooledObject;
        private readonly Transform container;
        private readonly ObjectPool<SimplePooledObject> pool;

        public SimplePooledObject PooledObject => pooledObject;
        public ObjectPool<SimplePooledObject> Pool => pool;
        public Transform Container => container;



        protected virtual void Destroyed(SimplePooledObject pooledObject)
        {
            UnityEngine.Object.Destroy(pooledObject.gameObject);
        }

        protected virtual SimplePooledObject Create()
        {
            SimplePooledObject pooledObject;

            if (container)
            {
                pooledObject = UnityEngine.Object.Instantiate(PooledObject, container);
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
            pooledObject.Activate();
        }

        public void Release(SimplePooledObject pooledObject)
        {
            Pool.Release(pooledObject);
        }
        public void Released(SimplePooledObject pooledObject)
        {
            if (container && pooledObject.transform.parent != container)
            {
                pooledObject.SetParent(container, false);
            }
        }
    }
    
}