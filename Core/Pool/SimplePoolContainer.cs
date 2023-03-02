using System;
using UnityEngine;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/New Simple Pool Container", fileName ="SimplePool_")]
    public class SimplePoolContainer : BaseScriptableObject
    {
        [Header(" [ Simple Pool Container ] ")]
        [SerializeField] private SimplePooledObject _SimplePoolObject;
        [SerializeField] private GameObject _Container;
        [SerializeField] private int _StartSize = 5;
        [SerializeField] private int _Capacity = 10;
        [SerializeField] private int _MaxSize = 20;

        private GameObject _InstContainer;
        private SimplePool _SimplePool;

        public SimplePooledObject GetItem()
        {
            if (!_InstContainer)
            {
                if (_Container)
                {
                    _InstContainer = Instantiate(_Container);
                }
                else
                {
                    _InstContainer = new GameObject(name);
                }

                _SimplePool = new SimplePool(_SimplePoolObject, _InstContainer.transform, _StartSize, _Capacity, _MaxSize);
            }

            return _SimplePool.Get();
        }

        protected override void OnReset()
        {
            _InstContainer = null;
            _SimplePool = null;
        }
    }
    
}