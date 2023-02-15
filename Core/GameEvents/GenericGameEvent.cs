using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

using System;

namespace StudioScor.Utilities
{
    public class GenericGameEventSO<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private GenericGameEvent<T> _GameEvent;
        [SerializeField] private bool _UseDebug = false;

        public GenericGameEvent<T> GameEvent => _GameEvent;

#if UNITY_EDITOR
        [TextArea]
        [SerializeField] private string _Explanation;
#endif


        public void OnGameEvent(T data)
        {
            _GameEvent.OnGameEvent(data);
        }

        public void AddListner(GenericGameEventListner<T> listner)
        {
            _GameEvent.AddListner(listner);
        }

        public void RemoveListner(GenericGameEventListner<T> listner)
        {
            _GameEvent.RemoveListner(listner);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _GameEvent = new GenericGameEvent<T>(_UseDebug);
        }
    }

    [System.Serializable]
    public class GenericGameEvent<T>
    {
        [SerializeField] private List<GenericGameEventListner<T>> _EventList = new List<GenericGameEventListner<T>>();
        [SerializeField] private bool _UseDebug = false;

        public event UnityAction<T> GameEvent;

        public GenericGameEvent()
        {

        }
        public GenericGameEvent(bool useDebug = false)
        {
            _UseDebug = useDebug;
        }


        public int GetEventListCount()
        {
            return _EventList.Count;
        }

        public void OnGameEvent(T data)
        {
            Log(" On Game Event ");

            for (int i = 0; _EventList.Count > i; i++)
            {
                _EventList[i].TryActiveGameEvent(data);
            }

            GameEvent?.Invoke(data);
        }

        public void AddListner(GenericGameEventListner<T> listner)
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

        public void RemoveListner(GenericGameEventListner<T> listner)
        {
            if (_EventList.Contains(listner))
            {
                Log(" Remove Listner " + listner);

                _EventList.Remove(listner);
            }
            else
            {
                Log("'" + listner + "'] 는 이미 존재하지 않는 이벤트");
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void Log(string log)
        {
            if (_UseDebug)
                SUtility.Debug.Log("GameEvent :" + log);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _EventList = new();
        }
    }

}
