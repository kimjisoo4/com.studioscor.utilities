using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

using System;

namespace StudioScor.Utilities
{
    public class GenericGameEvent<T> : GameEvent
    {
        [SerializeField] private List<GenericGameEventListner<T>> _GenericEventList = new List<GenericGameEventListner<T>>();

        public event UnityAction<T> OnGenericEvent;

        public int GetGenericEventListCount()
        {
            return _GenericEventList.Count;
        }

        public void Invoke(T data)
        {
            Invoke();

            Log(" On Generic Game Event ");

            for (int i = 0; _GenericEventList.Count > i; i++)
            {
                _GenericEventList[i].TryActiveGameEvent(data);
            }

            OnGenericEvent?.Invoke(data);

        }
        public void AddListner(GenericGameEventListner<T> listner)
        {
            if (_GenericEventList.Contains(listner))
            {
                Log($"{listner}] 는 이미 보유한 이벤트");
            }
            else
            {
                Log(" Add Listner " + listner);

                _GenericEventList.Add(listner);
            }
        }

        public void RemoveListner(GenericGameEventListner<T> listner)
        {
            if (_GenericEventList.Contains(listner))
            {
                Log(" Remove Listner " + listner);

                _GenericEventList.Remove(listner);
            }
            else
            {
                Log($"{listner}] 는 보유하지 않는 이벤트");
            }
        }

        protected override void OnReset()
        {
            base.OnReset();

            _GenericEventList = new();
        }
    }

}
