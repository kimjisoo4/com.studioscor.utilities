using UnityEngine;
using UnityEngine.EventSystems;

namespace StudioScor.Utilities
{
    public interface IPressAnyKeyEventListener
    {
        public delegate void PressAnyKeyEventHandler(IPressAnyKeyEventListener anyKeyEventListener, BaseEventData eventData);
        public GameObject gameObject { get; }
        public Transform transform { get; }
        public bool CanPressAnyKey();

        public event PressAnyKeyEventHandler OnPressAnyKey;
        public event PressAnyKeyEventHandler OnPressAnyKeyFailed;
    }
}


