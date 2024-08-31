using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StudioScor.Utilities
{
    public class SubmitEventListener : BaseMonoBehaviour, ISubmitHandler, IPointerClickHandler, ISubmitEventListener
    {

        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField] private UnityEvent _onSubmited;
            public void AddUnityEvent(ISubmitEventListener submitEventHandler)
            {
                submitEventHandler.OnSubmited += SubmitEventHandler_OnSubmited;
            }
            public void RemoveUnityEvent(ISubmitEventListener submitEventHandler)
            {
                submitEventHandler.OnSubmited -= SubmitEventHandler_OnSubmited;
            }

            private void SubmitEventHandler_OnSubmited(ISubmitEventListener submitEventListener,BaseEventData obj)
            {
                _onSubmited?.Invoke();
            }
        }

        [Header(" [ Submit Event Listner ] ")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private EInputHandlerType _inputType = EInputHandlerType.Both;

        [Header(" Unity Event ")]
        [SerializeField] private bool _useUnityEvent = false;
        [SerializeField] private UnityEvents _unityEvents;

        public event ISubmitEventListener.SubmitEventHandler OnSubmited;

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (!_canvasGroup)
            {
                _canvasGroup = GetComponentInParent<CanvasGroup>();
            }
#endif
        }

        protected virtual void Awake()
        {
            if (_useUnityEvent)
            {
                _unityEvents.AddUnityEvent(this);
            }
        }
        protected virtual void OnDestroy()
        {
            if (_useUnityEvent && ReferenceEquals(_unityEvents, null))
            {
                _unityEvents.RemoveUnityEvent(this);
            }
        }
        public virtual bool CanSubmit()
        {
            if (_canvasGroup && !_canvasGroup.interactable)
                return false;

            return true;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (_inputType == EInputHandlerType.Pointer)
                return;

            if (!CanSubmit())
                return;

            Log($"{nameof(OnSubmited)}");

            OnSubmited?.Invoke(this, eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_inputType == EInputHandlerType.Button)
                return;

            if (!CanSubmit())
                return;

            Log($"{nameof(OnSubmited)}");

            OnSubmited?.Invoke(this,eventData);
        }
    }
}


