using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{

    public class GameEventListner
    {
        [SerializeField] private GameEvent _GameEvent;

        public event UnityAction OnEvent;

        public GameEventListner(GameEvent gameEvent)
        {
            _GameEvent = gameEvent;
        }

        public void OnListner()
        {
            _GameEvent.AddListner(this);
        }
        public void Endlistner()
        {
            _GameEvent.RemoveListner(this);
        }

        public void OnGameEvent()
        {
            OnEvent.Invoke();
        }
    }

}
