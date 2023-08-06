using System;
using TMPro;
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

        public void Initialization(Transform newContainer = null)
        {
            if (instContainer)
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
                SetContainer(new GameObject(name));
            }
        }

        private void SetContainer(GameObject newContainer)
        {
            instContainer = newContainer;

            simplePool = new SimplePool(simplePoolObject, instContainer.transform, startSize, capacity, maxSize);
        }

        public SimplePooledObject Get()
        {
            if (!instContainer)
            {
                if (container)
                    SetContainer(Instantiate(container));
                else
                    SetContainer(new GameObject(name));
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