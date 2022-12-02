using System;
using UnityEngine;
using UnityEngine.Pool;


namespace StudioScor.Utilities
{
    public class SimplePoolContainer
    {
        public SimplePoolContainer(SimplePooledObject pooledObject, int maxSize = 5)
        {
            _PooledObject = pooledObject;
            _MaxSize = maxSize;

            SetupPool();
        }

        private SimplePooledObject _PooledObject;
        private IObjectPool<SimplePooledObject> _Pool;
        private int _MaxSize;

        public SimplePooledObject PooledObject => _PooledObject;
        public IObjectPool<SimplePooledObject> Pool => _Pool;

        public void SetupPool()
        {
            _Pool = new ObjectPool<SimplePooledObject>(Create, actionOnDestroy : Destroyed, maxSize : _MaxSize);
        }

        private void Destroyed(SimplePooledObject pooledObject)
        {
            UnityEngine.Object.Destroy(pooledObject.gameObject);
        }

        private SimplePooledObject Create()
        {
            var pooledObject = UnityEngine.Object.Instantiate(PooledObject);

            pooledObject.Create(this);

            return pooledObject;
        }
        public SimplePooledObject Get()
        {
            return Pool.Get();
        }
        public void Release(SimplePooledObject pooledObject)
        {
            Pool.Release(pooledObject);
        }
    }
    
}