using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StudioScor.Utilities
{
    public class SelectEventListener : BaseMonoBehaviour, ISelectHandler, ISelectEventListener
    {
        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField] private UnityEvent _onSelected;
            public void AddUnityEvent(ISelectEventListener selectEventHandler)
            {
                selectEventHandler.OnSelected += SubmitEventHandler_OnSubmited;
            }
            public void RemoveUnityEvent(ISelectEventListener submitEventHandler)
            {
                submitEventHandler.OnSelected -= SubmitEventHandler_OnSubmited;
            }

            private void SubmitEventHandler_OnSubmited(BaseEventData obj)
            {
                _onSelected?.Invoke();
            }
        }

        [Header(" [ Select Event Listner ] ")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header(" Unity Event ")]
        [SerializeField] private bool _useUnityEvent = false;
        [SerializeField] private UnityEvents _unityEvents;

        public event Action<BaseEventData> OnSelected;

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

        public virtual bool CanSelect()
        {
            if (_canvasGroup && !_canvasGroup.interactable)
                return false;

            return true;
        }
        public void OnSelect(BaseEventData eventData)
        {
            if (!CanSelect())
                return;

            Log($"{nameof(OnSelect)}");

            OnSelected?.Invoke(eventData);
        }
    }
}


