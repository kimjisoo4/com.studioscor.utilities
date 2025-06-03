using UnityEngine;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/New Pool Container", fileName = "Pool_")]
    public class PoolContainer : BaseScriptableObject
    {
        [Header(" [ Simple Pool Container ] ")]
        [SerializeField] private PooledObject _pooledObject;
        [SerializeField] private GameObject _container;
        [SerializeField] private int _startSize = 5;
        [SerializeField] private int _capacity = 10;
        [SerializeField] private int _maxSize = 20;

        private GameObject _instContainer;
        private SimplePool _simplePool;


        public void Initialization(Transform newContainer = null)
        {
            if (_instContainer)
                return;

            if(newContainer)
            {
                SetContainer(newContainer.gameObject);
            }
            else if(_container)
            {
                SetContainer(Instantiate(_container));
            }
            else
            {
                SetContainer(new GameObject($"[Pool]{name}"));
            }
        }

        private void SetContainer(GameObject newContainer)
        {
            Log($"[Pool]{nameof(SetContainer)} - {newContainer}");

            _instContainer = newContainer;

            if(_simplePool is not null)
            {
                Log($"Clear Pool");

                _simplePool.Clear();
                _simplePool = null;
            }

            _simplePool = new SimplePool(_pooledObject, _instContainer.transform, _startSize, _capacity, _maxSize);
        }

        public PooledObject Get()
        {
            if (!_instContainer)
                Initialization();

            return _simplePool.Get();
        }
        public void Release(PooledObject pooledObject)
        {
            if (!_instContainer)
                Initialization();

            _simplePool.Release(pooledObject);
        }

        protected override void OnReset()
        {
            _instContainer = null;
            _simplePool = null;
        }
    }
    
}