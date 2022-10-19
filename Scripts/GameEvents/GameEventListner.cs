using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace KimScor.Utilities
{
    public class GameEventListner : MonoBehaviour
    {
        [SerializeField] private GameEvent _GameEvent;
        [SerializeField] private UnityEvent _Event;
        [SerializeField] private bool _UseDebug = false;
        private void OnEnable()
        {
            _GameEvent.AddListner(this);
        }
        private void OnDisable()
        {
            _GameEvent.RemoveListner(this);
        }

        public void OnGameEvent()
        {
            _Event.Invoke();
        }

        [Conditional("UNITY_EDITOR")]
        private void Log(string log)
        {
            if(_UseDebug)
                Utilities.Debug.Log("GameEventListner [" + name + "] :" + log, this);
        }
    }

}
