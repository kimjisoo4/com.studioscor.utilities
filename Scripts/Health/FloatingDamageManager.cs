using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace KimScor.Utilities
{
    public class FloatingDamageManager : Singleton<FloatingDamageManager>
    {
        [SerializeField] private Transform _TextContainer;
		[SerializeField] private FloatingDamageText _FloatingDamageText;

        [Header(" [ Use Object Pool ] ")]
        [SerializeField] private int _Capacity = 10;
        [SerializeField] private int _MaxSize = 100;

        private IObjectPool<FloatingDamageText> _FloatingDamagePool;

        protected override void Setup()
        {
            base.Setup();

            SetupPool();
        }

        public void SpawnFloatingDamage(Vector3 position, float damage)
        {
            var text = _FloatingDamagePool.Get();

            text.transform.position = position;
			text.OnText(damage);
        }

        #region ObjectPool
        public void SetupPool()
        {
            _FloatingDamagePool = new ObjectPool<FloatingDamageText>(CreatePool, collectionCheck : true, defaultCapacity : _Capacity, maxSize : _MaxSize);

            List<FloatingDamageText> texts = new();

            for(int i = 0; i < _Capacity; i++)
            {
                texts.Add(_FloatingDamagePool.Get());
            }
            foreach (var text in texts)
            {
                text.Release();
            }
        }

        private FloatingDamageText CreatePool()
        {
            var text = Instantiate(_FloatingDamageText, _TextContainer);

            text.Create(_FloatingDamagePool);

            return text;
        }

        #endregion
    }

}
