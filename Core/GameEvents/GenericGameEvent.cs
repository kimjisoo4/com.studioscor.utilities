using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

using System;

namespace StudioScor.Utilities
{

    public class GenericGameEvent<T> : GameEvent
    {
        [SerializeField][SReadOnly] private List<GenericGameEventListner<T>> genericEventList = new List<GenericGameEventListner<T>>();

        public event UnityAction<T> OnGenericEvent;

        public int GetGenericEventListCount()
        {
            return genericEventList.Count;
        }

        public void Invoke(T data)
        {
            Invoke();

            Log(" On Generic Game Event ");

            for (int i = 0; genericEventList.Count > i; i++)
            {
                genericEventList[i].TryActiveGameEvent(data);
            }

            OnGenericEvent?.Invoke(data);

        }
        public void AddListner(GenericGameEventListner<T> listner)
        {
            if (genericEventList.Contains(listner))
            {
                Log($"{listner}] 는 이미 보유한 이벤트");
            }
            else
            {
                Log(" Add Listner " + listner);

                genericEventList.Add(listner);
            }
        }

        public void RemoveListner(GenericGameEventListner<T> listner)
        {
            if (genericEventList.Contains(listner))
            {
                Log(" Remove Listner " + listner);

                genericEventList.Remove(listner);
            }
            else
            {
                Log($"{listner}] 는 보유하지 않는 이벤트");
            }
        }

        protected override void OnReset()
        {
            base.OnReset();

            genericEventList = new();
        }
    }

}
