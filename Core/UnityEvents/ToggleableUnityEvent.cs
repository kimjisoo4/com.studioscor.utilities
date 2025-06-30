using UnityEngine;
using UnityEngine.Events;

namespace StudioScor.Utilities
{

    [System.Serializable]
    public class ToggleableUnityEvent
    {
        [SerializeField] private bool _useUnityEvent;
        [SerializeField] private UnityEvent _unityEvent;

        public ToggleableUnityEvent()
        {

        }
        public ToggleableUnityEvent(bool useUnityEvent)
        {
            _useUnityEvent = useUnityEvent;
        }

        public void Dispose()
        {
            if (!_useUnityEvent)
                return;

            _unityEvent.RemoveAllListeners();
        }

        public void Invoke()
        {
            if (!_useUnityEvent)
                return;

            _unityEvent?.Invoke();
        }
    }
}
