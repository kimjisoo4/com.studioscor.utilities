using System.Collections.Generic;
using UnityEngine;

namespace KimScor.Utilities
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
            if (!_PooledContainers.TryAdd(pooledObject, new SimplePoolContainer(pooledObject)))
            {
                Log("[ " + pooledObject + " ] item Contained ! ");
            }
        }
    }
    
}