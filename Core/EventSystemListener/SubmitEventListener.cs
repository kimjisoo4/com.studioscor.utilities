using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public class SubmitEventListener : BaseMonoBehaviour, ISubmitHandler, IPointerClickHandler, ISubmitEventListener
    {

        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField] private UnityEvent _onSubmited;
            [SerializeField] private UnityEvent _onFailedSubmited;
            public void AddUnityEvent(ISubmitEventListener submitEventHandler)
            {
                submitEventHandler.OnSubmited += SubmitEventHandler_OnSubmited;
                submitEventHandler.OnFailedSubmited += SubmitEventHandler_OnFailedSubmited;
            }
            public void RemoveUnityEvent(ISubmitEventListener submitEventHandler)
            {
                submitEventHandler.OnSubmited -= SubmitEventHandler_OnSubmited;
                submitEventHandler.OnFailedSubmited -= SubmitEventHandler_OnFailedSubmited;
            }

            private void SubmitEventHandler_OnSubmited(ISubmitEventListener submitEventListener,BaseEventData obj)
            {
                _onSubmited?.Invoke();
            }
            private void SubmitEventHandler_OnFailedSubmited(ISubmitEventListener submitEventListener, BaseEventData eventData)
            {
                _onFailedSubmited?.Invoke();
            }
        }

        [Header(" [ Submit Event Listner ] ")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Selectable _selectable;
        [SerializeField] private EInputHandlerType _inputType = EInputHandlerType.Both;

        [Header(" Unity Event ")]
        [SerializeField] private bool _useUnityEvent = false;
        [SerializeField] private UnityEvents _unityEvents;

        public event ISubmitEventListener.SubmitEventHandler OnSubmited;
        public event ISubmitEventListener.SubmitEventHandler OnFailedSubmited;

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (!_canvasGroup)
            {
                _canvasGroup = GetComponentInParent<CanvasGroup>();
            }
            if(!_selectable)
            {
                _selectable = GetComponent<Selectable>();
            }
#endif
        }

        protected virtual void Awake()
        {
            if (!_canvasGroup)
            {
                _canvasGroup = GetComponentInParent<CanvasGroup>();
            }
            if (!_selectable)
            {
                _selectable = GetComponent<Selectable>();
            }
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

            OnSubmited = null;
            OnFailedSubmited = null;
        }
        public virtual bool CanSubmit()
        {
            if ((_canvasGroup && !_canvasGroup.interactable)
                || (!_selectable.interactable))
                return false;

            return true;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (_inputType == EInputHandlerType.Pointer)
                return;

            if (!CanSubmit())
            {
                Log($"{nameof(OnFailedSubmited)}");
                OnFailedSubmited?.Invoke(this, eventData);
                return;
            }

            Log($"{nameof(OnSubmited)}");

            OnSubmited?.Invoke(this, eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_inputType == EInputHandlerType.Button)
                return;

            if (!CanSubmit())
            {
                Log($"{nameof(OnFailedSubmited)}");
                OnFailedSubmited?.Invoke(this, eventData);
                return;
            }

            Log($"{nameof(OnSubmited)}");

            OnSubmited?.Invoke(this,eventData);
        }
    }
}


