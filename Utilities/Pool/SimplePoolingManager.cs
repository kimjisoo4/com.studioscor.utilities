using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public class SimplePoolingManager : Singleton<SimplePoolingManager>
    {
        [SerializeField] private List<SimplePooledObject> _PooledObjects;
        private Dictionary<SimplePooledObject, SimplePoolContainer> _PooledContainers;

        protected override void Setup()
        {
            base.Setup();

            _PooledContainers = new();

            foreach (var pooledObject in _PooledObjects)
            {
                Add(pooledObject);
            }
        }

        public SimplePooledObject Get(SimplePooledObject pooledObject)
        {
            if (!pooledObject)
                return null;

            if (_PooledContainers.TryGetValue(pooledObject, out SimplePoolContainer spawner))
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
        private void Add(SimplePooledObject pooledObject)
        {
            if(_PooledContainers.ContainsKey(pooledObject))
            {
                Log("item Contained [ " + pooledObject + " ]");

                return;
            }

            SimplePoolContainer poolContainer = new(pooledObject);

            _PooledContainers.Add(pooledObject, poolContainer);

            Log("Add New Pool [ " + pooledObject + " ]");
        }
    }
    
}