using System;
using UnityEngine;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/New Simple Pool Container", fileName ="SimplePool_")]
    public class SimplePoolContainer : BaseScriptableObject
    {
        [Header(" [ Simple Pool Container ] ")]
        [SerializeField] private SimplePooledObject simplePoolObject;
        [SerializeField] private GameObject container;
        [SerializeField] private int startSize = 5;
        [SerializeField] private int capacity = 10;
        [SerializeField] private int maxSize = 20;

        private GameObject instContainer;
        private SimplePool simplePool;

        public SimplePooledObject Get()
        {
            if (!instContainer)
            {
                if (container)
                {
                    instContainer = Instantiate(container);
                }
                else
                {
                    instContainer = new GameObject(name);
                }

                simplePool = new SimplePool(simplePoolObject, instContainer.transform, startSize, capacity, maxSize);
            }


            return simplePool.Get();
        }

        protected override void OnReset()
        {
            instContainer = null;
            simplePool = null;
        }
    }
    
}