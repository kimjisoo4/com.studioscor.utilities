using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class GameEventListnerComponent : BaseMonoBehaviour
    {
        [Header(" [ GameEvent Listner Component ] ")]
        [SerializeField] private GameEvent _GameEvent;
        [SerializeField] private UnityEvent _Event;

        private GameEventListner _GameEventListener;

        private void Awake()
        {
            _GameEventListener = new GameEventListner(_GameEvent);

            _GameEventListener.OnEvent += GameEventListner_OnEvent;
        }

        private void GameEventListner_OnEvent()
        {
            Log($"Invoke - [ {_GameEvent.name} ]");

            _Event.Invoke();
        }

        private void OnEnable()
        {
            Log($" Add Listen - [ {_GameEvent.name} ] ");

            _GameEventListener.OnListner();
        }
        private void OnDisable()
        {
            Log($" End Listen - [ {_GameEvent.name} ] ");

            _GameEventListener.Endlistner();
        }
    }

}
