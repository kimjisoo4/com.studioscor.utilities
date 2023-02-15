using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class GenericGameEventListnerComponent<T> : MonoBehaviour
    {
        [SerializeField] private GenericGameEventSO<T> _GameEvent;
        [SerializeField] private UnityEvent<T> _Event;
        [SerializeField] private bool _UseDebug = false;

        private GenericGameEventListner<T> _GameEventListner;

        private void Awake()
        {
            _GameEventListner = new GenericGameEventListner<T>(_GameEvent.GameEvent);
            _GameEventListner.OnEvent += GameEventListner_OnEvent;
        }

        protected virtual void GameEventListner_OnEvent(T data)
        {
            Log("On Game Event - " + data);

            _Event.Invoke(data);
        }

        private void OnEnable()
        {
            _GameEvent.AddListner(_GameEventListner);
        }
        private void OnDisable()
        {
            _GameEvent.RemoveListner(_GameEventListner);
        }

        [Conditional("UNITY_EDITOR")]
        private void Log(string log)
        {
            if (_UseDebug)
                SUtility.Debug.Log("GameEventListner [" + name + "] :" + log, this);
        }
    }

}
