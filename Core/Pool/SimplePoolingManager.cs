using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    [System.Serializable]
    public struct FPooledObject
    {
        public PooledObject PooledObject;
        public Transform Container;
        public int StartSize;
        public int Capacity;
        public int MaxSize;

        public FPooledObject(PooledObject pooledObject = null, Transform container = null,int startSize = 5, int capacity = 10, int maxSize = 10)
        {
            PooledObject = pooledObject;
            Container = container;
            StartSize = startSize;
            Capacity = capacity;
            MaxSize = maxSize;
        }
    }
    
    public class SimplePoolingManager : Singleton<SimplePoolingManager>
    {
        [Header(" Simple Pooling Manager ")]
        [SerializeField] private List<FPooledObject> pooledObjects;
        private readonly Dictionary<PooledObject, SimplePool> pools = new();

        protected override void Setup()
        {
            base.Setup();

            foreach (var pooledObject in pooledObjects)
            {
                Add(pooledObject.PooledObject, pooledObject.Container, pooledObject.StartSize, pooledObject.Capacity, pooledObject.MaxSize);
            }
        }

        public bool HasPool(PooledObject pooledObject)
        {
            return pools.ContainsKey(pooledObject);
        }

        public PooledObject Get(PooledObject pooledObject)
        {
            if (!pooledObject)
                return null;

            if (pools.TryGetValue(pooledObject, out SimplePool spawner))
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
        public void Add(PooledObject pooledObject, Transform container = null, int startSize = 5, int capacity = 10, int maxSize = 10)
        {
            if(pools.ContainsKey(pooledObject))
            {
                Log("item Contained [ " + pooledObject + " ]");

                return;
            }

            SimplePool pool = new(pooledObject, container, startSize, capacity, maxSize);

            pools.Add(pooledObject, pool);

            Log("Add New Pool [ " + pooledObject + " ]");
        }
    }
    
}