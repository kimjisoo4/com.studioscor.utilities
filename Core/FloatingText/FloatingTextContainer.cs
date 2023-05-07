using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/New FloatingTextContainer", fileName = "FloatingTextContainer_")]
    public class FloatingTextContainer : BaseScriptableObject
    {
        [Header(" [ Floating Text Container ] ")]
        [SerializeField] private FloatingTextComponent _FloatingDamageText;
        [SerializeField] private Canvas _Container;

        [SerializeField] private int _Capacity = 10;
        [SerializeField] private int _MaxSize = 100;

        private Canvas _InstContainer;
        private IObjectPool<FloatingTextComponent> _Pool;


        protected override void OnReset()
        {
            base.OnReset();

            _InstContainer = null;
            _Pool = null;
        }

        public void SpawnFloatingDamage(Vector3 position, float damage)
        {
            if (!_InstContainer || _Pool is null)
                SetupPool();

            var floatingText = _Pool.Get();

            floatingText.transform.position = position;
            floatingText.OnText(damage);
        }

        private void SetupPool()
        {
            if(!_InstContainer)
                _InstContainer = Instantiate(_Container);

            if(_Pool is null)
            {
                _Pool = new ObjectPool<FloatingTextComponent>(CreatePool, actionOnGet: Getted, collectionCheck: true, defaultCapacity: _Capacity, maxSize: _MaxSize);

                var poolObjects = new List<FloatingTextComponent>();

                for (int i = 0; i < _Capacity; i++)
                {
                    poolObjects.Add(_Pool.Get());
                }

                foreach (var poolObject in poolObjects)
                {
                    poolObject.Release();
                }
            }
        }

        private void Getted(FloatingTextComponent floatingDamageText)
        {
            floatingDamageText.Activate();
        }

        private FloatingTextComponent CreatePool()
        {
            var text = Instantiate(_FloatingDamageText, _InstContainer.transform);

            text.Create(_Pool);

            text.gameObject.SetActive(false);

            return text;
        }
    }

}
