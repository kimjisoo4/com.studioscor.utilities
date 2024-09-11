using UnityEngine;
using UnityEngine.EventSystems;

namespace StudioScor.Utilities
{
    public interface IDeselectEventListener
    {
        public delegate void DeselectEventHandler(IDeselectEventListener submitEventListener, BaseEventData eventData);
        public GameObject gameObject { get; }
        public Transform transform { get; }
        public bool CanDeselect();

        public event DeselectEventHandler OnDeselected;
    }
}


