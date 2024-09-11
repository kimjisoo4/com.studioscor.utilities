using UnityEngine;
using UnityEngine.EventSystems;

namespace StudioScor.Utilities
{
    public interface ISubmitEventListener
    {
        public delegate void SubmitEventHandler(ISubmitEventListener submitEventListener, BaseEventData eventData);
        public GameObject gameObject { get; }
        public Transform transform { get; }
        public bool CanSubmit();

        public event SubmitEventHandler OnSubmited;
        public event SubmitEventHandler OnFailedSubmited;
    }
}


