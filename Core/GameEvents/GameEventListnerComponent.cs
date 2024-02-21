using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class GameEventListnerComponent : BaseMonoBehaviour
    {
        [Header(" [ GameEvent Listner Component ] ")]
        [SerializeField] private GameEvent gameEvent;
        [SerializeField] private UnityEvent triggerEvent;

        private GameEventListner gameEventListener;

        private void Awake()
        {
            gameEventListener = new GameEventListner(gameEvent);

            gameEventListener.OnEvent += GameEventListner_OnEvent;
        }

        private void GameEventListner_OnEvent()
        {
            Log($"Invoke - [ {gameEvent.name} ]");

            triggerEvent.Invoke();
        }

        private void OnEnable()
        {
            Log($" Add Listen - [ {gameEvent.name} ] ");

            gameEventListener.OnListner();
        }

        private void OnDisable()
        {
            Log($" End Listen - [ {gameEvent.name} ] ");

            gameEventListener.Endlistner();
        }
    }

}
