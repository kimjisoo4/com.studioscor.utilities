using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{
    public class GenericGameEventListner<T>
    {
        [SerializeField] private GenericGameEvent<T> gameEvent;
        public event UnityAction<T> OnEvent;

        public GenericGameEventListner(GenericGameEvent<T> gameEvent)
        {
            this.gameEvent = gameEvent;
        }

        public void OnListner()
        {
            gameEvent.AddListner(this);
        }
        public void EndListner()
        {
            gameEvent.RemoveListner(this);
        }

        public void TryActiveGameEvent(T data)
        {
            OnEvent.Invoke(data);
        }
    }

}
