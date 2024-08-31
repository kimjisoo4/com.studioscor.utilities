using UnityEngine;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/New Simple Pool Container", fileName = "SimplePool_")]
    public class SimplePoolContainer : BaseScriptableObject
    {
        [Header(" [ Simple Pool Container ] ")]
        [SerializeField] private SimplePooledObject simplePoolObject;
        [SerializeField] private GameObject container;
        [SerializeField] private int startSize = 5;
        [SerializeField] private int capacity = 10;
        [SerializeField] private int maxSize = 20;

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
            else if(container)
            {
                SetContainer(Instantiate(container));
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

            _simplePool = new SimplePool(simplePoolObject, _instContainer.transform, startSize, capacity, maxSize);
        }

        public SimplePooledObject Get()
        {
            if (!_instContainer)
            {
                if (container)
                    SetContainer(Instantiate(container));
                else
                    SetContainer(new GameObject($"[Pool]{name}"));
            }

            return _simplePool.Get();
        }
        public void Release(SimplePooledObject pooledObject)
        {
            if (!_instContainer)
            {
                if (container)
                    SetContainer(Instantiate(container));
                else
                    SetContainer(new GameObject($"[Pool]{name}"));
            }

            _simplePool.Release(pooledObject);
        }

        protected override void OnReset()
        {
            _instContainer = null;
            _simplePool = null;
        }
    }
    
}