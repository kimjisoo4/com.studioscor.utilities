using UnityEngine;
using UnityEngine.Events;


namespace KimScor.Utilities
{
    public class SimplePooledObject : MonoBehaviour
    {
        public UnityEvent OnCreatedEvent;
        public UnityEvent OnReleaseEvent;

        private SimplePoolContainer _PoolContainer;

        public void Create(SimplePoolContainer poolContainer)
        {
            _PoolContainer = poolContainer;

            OnCreatedEvent?.Invoke();
        }
        public void Release()
        {
            _PoolContainer.Release(this);

            OnReleaseEvent?.Invoke();
        }
    }
    
}