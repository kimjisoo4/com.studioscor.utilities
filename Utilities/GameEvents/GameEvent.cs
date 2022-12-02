using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

namespace StudioScor.Utilities
{

    [CreateAssetMenu(fileName = "Event_", menuName = "Utilities/Event/New Event")]
    public class GameEvent : ScriptableObject, ISerializationCallbackReceiver
    {
        private List<GameEventListner> _EventList = new List<GameEventListner>();
        public event Action Events;

        [SerializeField] private bool _UseDebug = false;

#if UNITY_EDITOR
        [TextArea]
        [SerializeField] private string _Explanation;
#endif

        public int GetEventListCount()
        {
            return _EventList.Count;
        }

        public void OnGameEvent()
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
                Log("'" + listner + "' 는 이미 존재하는 이벤트");
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
                Log("'"+ listner + "'] 는 이미 존재하지 않는 이벤트");
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void Log(string log)
        {
            if (_UseDebug)
                Utility.Debug.Log("GameEvent [" + name + "] :" + log, this);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _EventList = new();
            Events = null;
        }
    }

}
