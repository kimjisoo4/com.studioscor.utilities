using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine.UI;
using System;

namespace StudioScor.Utilities
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class FloatingTextCanvas : BaseMonoBehaviour
    {
        [Header(" [ Floating Text ] ")]
        [SerializeField] private FloatingDamageText _FloatingText;
        [Space(5f)]
        [SerializeField] private bool _AutoSetup = true;
        [Space(5f)]
        [SerializeField] private int _Capacity = 10;
        [SerializeField] private int _MaxSize = 20;

        private IObjectPool<FloatingDamageText> _FloatingPool;

        private void Awake()
        {
            if(_AutoSetup && _FloatingPool is null)
                SetupPool();    
        }

        public void SetupFloatingText(FloatingDamageText floatingDamageText, int capacity = 10, int maxSize = 20)
        {
            _FloatingText = floatingDamageText;
            _Capacity = capacity;
            _MaxSize = maxSize;

            SetupPool();
        }

        public void SpawnFloatingDamage(Vector3 position, float damage)
        {
            if (_FloatingPool is null)
            {
                SetupPool();
            }

            var text = _FloatingPool.Get();

            text.transform.position = position;
            text.OnText(damage);
        }

        private void SetupPool()
        {
            _FloatingPool = new ObjectPool<FloatingDamageText>(CreatePool, actionOnGet:Getted, collectionCheck: true, defaultCapacity: _Capacity, maxSize: _MaxSize);

            var poolObjects = new List<FloatingDamageText>();

            for (int i = 0; i < _Capacity; i++)
            {
                poolObjects.Add(_FloatingPool.Get());
            }
            foreach (var poolObject in poolObjects)
            {
                poolObject.Release();
            }
        }

        private void Getted(FloatingDamageText floatingDamageText)
        {
            floatingDamageText.Activate();
        }

        private FloatingDamageText CreatePool()
        {
            var text = Instantiate(_FloatingText, transform);

            text.Create(_FloatingPool);

            text.gameObject.SetActive(false);

            return text;
        }
    }

}
