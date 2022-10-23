using UnityEngine.Pool;


namespace KimScor.Utilities
{
    public class SimplePoolContainer
    {
        public SimplePoolContainer(SimplePooledObject pooledObject)
        {
            _PooledObject = pooledObject;

            SetupPool();
        }

        private SimplePooledObject _PooledObject;
        private IObjectPool<SimplePooledObject> _Pool;

        public SimplePooledObject PooledObject => _PooledObject;
        public IObjectPool<SimplePooledObject> Pool => _Pool;

        public void SetupPool()
        {
            _Pool = new ObjectPool<SimplePooledObject>(Create);
        }

        private SimplePooledObject Create()
        {
            var monster = UnityEngine.Object.Instantiate(_PooledObject);

            return monster;
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