using System.Collections.Generic;
using UnityEngine;
using System;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(fileName = "Event_", menuName = "StudioScor/Utilities/Event/New Event")]
    public class GameEvent : BaseScriptableObject, ISerializationCallbackReceiver
    {
        [Header(" [ GameEvent ] ")]
        [SerializeField, TextArea] protected string description;

        [SerializeField][SReadOnly] private List<GameEventListner> eventList = new List<GameEventListner>();
        public event Action OnTriggerEvent;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            OnReset();
        }

        protected override void OnReset()
        {
            eventList = new();
            OnTriggerEvent = null;
        }

        public int GetEventListCount()
        {
            return eventList.Count;
        }

        public void Invoke()
        {
            Log(" On Game Event ");

            for (int i = 0; eventList.Count > i; i++)
            {
                eventList[i].OnGameEvent();
            }

            OnTriggerEvent?.Invoke();
        }

        public void AddListner(GameEventListner listner)
        {
            if (eventList.Contains(listner))
            {
                Log($"{listner}] 는 이미 보유한 이벤트");
            }
            else
            {
                Log(" Add Listner " + listner);

                eventList.Add(listner);
            }
        }

        public void RemoveListner(GameEventListner listner)
        {
            if (eventList.Contains(listner))
            {
                Log(" Remove Listner " + listner);

                eventList.Remove(listner);
            }
            else
            {
                Log($"{listner}] 는 보유하지 않는 이벤트");
            }
        }
    }
}
