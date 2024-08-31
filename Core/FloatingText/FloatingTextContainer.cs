using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/New FloatingTextContainer", fileName = "FloatingTextContainer_")]
    public class FloatingTextContainer : BaseScriptableObject
    {
        [Header(" [ Floating Text Container ] ")]
        [SerializeField] private FloatingTextComponent _floatingDamageText;
        [SerializeField] private Canvas _container;
        [SerializeField] private int _capacity = 10;
        [SerializeField] private int _maxSize = 100;

        private Canvas _instContainer;
        private IObjectPool<FloatingTextComponent> _pool;
        protected override void OnReset()
        {
            base.OnReset();

            _instContainer = null;
            _pool = null;
        }

        public void SpawnFloatingDamage(Vector3 position, float damage)
        {
            if (!_instContainer || _pool is null)
                SetupPool();

            var floatingText = _pool.Get();

            floatingText.transform.position = position;

            floatingText.OnText(damage);
        }

        public void SetupPool(Canvas container = null)
        {
            if(!_instContainer)
            {
                if(_pool is not null)
                {
                    _pool.Clear();
                    _pool = null;
                }

                if (container)
                    _instContainer = container;
                else
                    _instContainer = Instantiate(_container);
            }

            _pool = new ObjectPool<FloatingTextComponent>(CreatePool, actionOnGet: Getted, collectionCheck: true, defaultCapacity: _capacity, maxSize: _maxSize);

            var poolObjects = new List<FloatingTextComponent>();

            for (int i = 0; i < _capacity; i++)
            {
                poolObjects.Add(_pool.Get());
            }

            foreach (var poolObject in poolObjects)
            {
                poolObject.Release();
            }
        }

        public void Clear()
        {
            _pool.Clear();
        }

        private void Getted(FloatingTextComponent floatingDamageText)
        {
            floatingDamageText.Activate();
        }

        private FloatingTextComponent CreatePool()
        {
            var text = Instantiate(_floatingDamageText, _instContainer.transform);

            text.Create(_pool);

            text.gameObject.SetActive(false);

            return text;
        }
    }

}
