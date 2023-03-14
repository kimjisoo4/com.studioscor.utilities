using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(fileName = "Event_", menuName = "StudioScor/Utilities/Event/New Event")]
    public class GameEvent : BaseScriptableObject, ISerializationCallbackReceiver
    {
        [Header(" [ GameEvent ] ")]
        [SerializeField, TextArea] protected string _Description;

        [SerializeField][SReadOnly] private List<GameEventListner> _EventList = new List<GameEventListner>();
        public event Action Events;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            OnReset();
        }

        protected override void OnReset()
        {
            _EventList = new();
            Events = null;
        }

        public int GetEventListCount()
        {
            return _EventList.Count;
        }

        public void Invoke()
        {
            Log(" On Game Event ");

            for (int i = 0; _EventList.Count > i; i++)
            {
                _EventList[i].OnGameEvent();
            }

            Events?.Invoke();
        }

        public void AddListner(GameEventListner listner)
        {
            if (_EventList.Contains(listner))
            {
                Log($"{listner}] 는 이미 보유한 이벤트");
            }
            else
            {
                Log(" Add Listner " + listner);

                _EventList.Add(listner);
            }
        }

        public void RemoveListner(GameEventListner listner)
        {
            if (_EventList.Contains(listner))
            {
                Log(" Remove Listner " + listner);

                _EventList.Remove(listner);
            }
            else
            {
                Log($"{listner}] 는 보유하지 않는 이벤트");
            }
        }
    }
}
