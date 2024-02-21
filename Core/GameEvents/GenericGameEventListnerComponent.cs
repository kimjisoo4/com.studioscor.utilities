using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class GenericGameEventListnerComponent<T> : BaseMonoBehaviour
    {
        [SerializeField] private GenericGameEvent<T> gameEvent;
        [SerializeField] private UnityEvent<T> onEvent;

        private GenericGameEventListner<T> gameEventListner;

        private void Awake()
        {
            gameEventListner = new GenericGameEventListner<T>(gameEvent);
            gameEventListner.OnEvent += GameEventListner_OnEvent;
        }

        protected virtual void GameEventListner_OnEvent(T data)
        {
            Log("On Game Event - " + data);

            onEvent.Invoke(data);
        }

        private void OnEnable()
        {
            gameEvent.AddListner(gameEventListner);
        }
        private void OnDisable()
        {
            gameEvent.RemoveListner(gameEventListner);
        }
    }
}
