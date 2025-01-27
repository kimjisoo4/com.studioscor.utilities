using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public class SelectEventListener : BaseMonoBehaviour, ISelectHandler, IPointerEnterHandler, ISelectEventListener
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

            private void SubmitEventHandler_OnSubmited(ISelectEventListener selectEventListener, BaseEventData eventData)
            {
                _onSelected?.Invoke();
            }
        }

        [Header(" [ Select Event Listner ] ")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Selectable _selectable;
        [SerializeField] private EInputHandlerType _inputType = EInputHandlerType.Both;

        [Header(" Unity Event ")]
        [SerializeField] private bool _useUnityEvent = false;
        [SerializeField] private UnityEvents _unityEvents;

        public event ISelectEventListener.SelectEventHandler OnSelected;

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

            OnSelected = null;
        }

        public virtual bool CanSelect()
        {
            if ((_canvasGroup && !_canvasGroup.interactable)
                || (!_selectable.interactable))
                return false;

            return true;
        }
        public void OnSelect(BaseEventData eventData)
        {
            if (_inputType == EInputHandlerType.Pointer)
                return;

            Select(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_inputType == EInputHandlerType.Button)
                return;

            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        private void Select(BaseEventData eventData)
        {
            if (!CanSelect())
                return;

            Log($"{nameof(OnSelect)}");

            OnSelected?.Invoke(this, eventData);
        }
    }
}


