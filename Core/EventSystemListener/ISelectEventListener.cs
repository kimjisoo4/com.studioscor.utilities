using UnityEngine;
using UnityEngine.EventSystems;

namespace StudioScor.Utilities
{
    public interface ISelectEventListener
    {
        public delegate void SelectEventHandler(ISelectEventListener submitEventListener, BaseEventData eventData);
        public GameObject gameObject { get; }
        public Transform transform { get; }
        public bool CanSelect();

        public event SelectEventHandler OnSelected;
    }
}


