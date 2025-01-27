using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{

    [System.Serializable]
    public class GenericToggleableUnityEvent<T>
    {
        [SerializeField] private bool _useUnityEvent;
        [SerializeField] private UnityEvent<T> _unityEvent;

        public GenericToggleableUnityEvent()
        {

        }
        public GenericToggleableUnityEvent(bool useUnityEvent)
        {
            _useUnityEvent = useUnityEvent;
        }

        public void Invoke(T value)
        {
            if (!_useUnityEvent)
                return;

            _unityEvent?.Invoke(value);
        }
    }
}
