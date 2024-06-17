using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class DefaultGameObjectVariable : GameObjectVariable
    {
        [Header(" [ Default GameObject Variable ] ")]
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private bool _isInstantiate;
        private DefaultGameObjectVariable _original;

        public override IGameObjectVariable Clone()
        {
            var clone = new DefaultGameObjectVariable();

            clone._original = this;

            return clone;
        }

        public override GameObject GetValue()
        {
            bool isOriginal = _original is null;

            GameObject value = isOriginal ? _gameObject : _original._gameObject;
            bool isInstantiate = isOriginal ? _isInstantiate : _original._isInstantiate;


            if(isInstantiate)
            {
                return UnityEngine.Object.Instantiate(value);
            }
            else
            {
                return value;
            }
        }
    }

    [Serializable]
    public class SelfGameObjectVariable : GameObjectVariable
    {
        public override IGameObjectVariable Clone()
        {
            return new SelfGameObjectVariable();
        }

        public override GameObject GetValue()
        { 
            return Owner;
        }
    }
}
