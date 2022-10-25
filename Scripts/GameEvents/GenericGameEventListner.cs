using UnityEngine;
using UnityEngine.Events;

namespace KimScor.Utilities
{
    public class GenericGameEventListner<T>
    {
        [SerializeField] private GenericGameEvent<T> _GameEvent;
        public event UnityAction<T> OnEvent;

        public GenericGameEventListner(GenericGameEvent<T> gameEvent)
        {
            _GameEvent = gameEvent;
        }

        public void OnListner()
        {
            _GameEvent.AddListner(this);
        }
        public void EndListner()
        {
            _GameEvent.RemoveListner(this);
        }

        public void TryActiveGameEvent(T data)
        {
            OnEvent.Invoke(data);
        }
    }

}
