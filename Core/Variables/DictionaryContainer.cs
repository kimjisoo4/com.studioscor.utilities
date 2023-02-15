using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace StudioScor.Utilities
{

    public abstract class DictionaryContainer<TKey, TValue> : BaseScriptableObject, ISerializationCallbackReceiver
    {
        private Dictionary<TKey, TValue> _Container;
        public IReadOnlyDictionary<TKey, TValue> Container => _Container;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _Container = new();
        }

        public void SetValue(TKey key, TValue value)
        {
            if (_Container is null)
                _Container = new();

            if (!_Container.ContainsKey(key))
            {
                _Container.Add(key, value);
            }
            else
            {
                _Container[key] = value;
            }
        }

        public bool Contain(TKey key)
        {
            if (_Container is null)
                _Container = new();

            return _Container.ContainsKey(key);
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_Container is null)
                _Container = new();

            return _Container.TryGetValue(key, out value);
        }

        public TValue GetValue(TKey key)
        {
            if (_Container is null)
                _Container = new();

            return _Container[key];
        }

        
    }

}
