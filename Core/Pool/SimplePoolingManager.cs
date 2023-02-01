using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public struct FPooledObject
    {
        public SimplePooledObject PooledObject;
        public Transform Container;
        public int MaxSize;

        public FPooledObject(SimplePooledObject pooledObject = null, Transform container = null, int maxSize = 5)
        {
            PooledObject = pooledObject;
            Container = container;
            MaxSize = maxSize;
        }
    }
    
    public class SimplePoolingManager : Singleton<SimplePoolingManager>
    {
        [SerializeField] private List<FPooledObject> _PooledObjects;
        private Dictionary<SimplePooledObject, SimplePool> _Pools;

        protected override void Setup()
        {
            base.Setup();

            _Pools = new();

            foreach (var pooledObject in _PooledObjects)
            {
                Add(pooledObject.PooledObject, pooledObject.Container, pooledObject.MaxSize);
            }
        }

        public bool HasPool(SimplePooledObject pooledObject)
        {
            return _Pools.ContainsKey(pooledObject);
        }

        public SimplePooledObject Get(SimplePooledObject pooledObject)
        {
            if (!pooledObject)
                return null;

            if (_Pools.TryGetValue(pooledObject, out SimplePool spawner))
            {
                Log(" Get - " + pooledObject);

                return spawner.Get();
            }
            else
            {
                Add(pooledObject);

                return Get(pooledObject);
            }
        }
        public void Add(SimplePooledObject pooledObject, Transform container = null, int size = 5)
        {
            if(_Pools.ContainsKey(pooledObject))
            {
                Log("item Contained [ " + pooledObject + " ]");

                return;
            }

            SimplePool pool = new(pooledObject, container, size);

            _Pools.Add(pooledObject, pool);

            Log("Add New Pool [ " + pooledObject + " ]");
        }
    }
    
}