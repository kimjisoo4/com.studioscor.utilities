using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace KimScor.Utilities
{
    public class GameEventListnerComponent : MonoBehaviour
    {
        [SerializeField] private GameEvent _GameEvent;
        [SerializeField] private UnityEvent _Event;
        [SerializeField] private bool _UseDebug = false;

        private GameEventListner _GameEventListner;

        private void Awake()
        {
            _GameEventListner = new GameEventListner(_GameEvent);
            _GameEventListner.OnEvent += GameEventListner_OnEvent;
        }

        private void GameEventListner_OnEvent()
        {
            _Event.Invoke();
        }

        private void OnEnable()
        {
            _GameEventListner.OnListner();
        }
        private void OnDisable()
        {
            _GameEventListner.Endlistner();
        }

        [Conditional("UNITY_EDITOR")]
        private void Log(string log)
        {
            if (_UseDebug)
                Utilities.Debug.Log("GameEventListner [" + name + "] :" + log, this);
        }
    }

}
