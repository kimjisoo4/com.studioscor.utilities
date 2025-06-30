using UnityEngine;
using UnityEngine.EventSystems;

namespace StudioScor.Utilities
{
    public interface ICancelEventListener
    {
        public delegate void CancelEventHandler(ICancelEventListener cancelEventListener, BaseEventData eventData);
        public GameObject gameObject { get; }
        public Transform transform { get; }
        public bool CanCancel();

        public event CancelEventHandler OnCanceled;
        public event CancelEventHandler OnCanceledFailed;
    }
}


