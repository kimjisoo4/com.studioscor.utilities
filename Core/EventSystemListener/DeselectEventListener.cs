using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public class DeselectEventListener : BaseMonoBehaviour, IDeselectHandler, IPointerExitHandler, IDeselectEventListener
    {
        [System.Serializable]
        public class UnityEvents
        {
            [SerializeField] private UnityEvent _onDeselected;
            public void AddUnityEvent(IDeselectEventListener selectEventHandler)
            {
                selectEventHandler.OnDeselected += DeselectEventHandler_OnDeselect;
            }
            public void RemoveUnityEvent(IDeselectEventListener submitEventHandler)
            {
                submitEventHandler.OnDeselected -= DeselectEventHandler_OnDeselect;
            }

            private void DeselectEventHandler_OnDeselect(IDeselectEventListener selectEventListener, BaseEventData eventData)
            {
                _onDeselected?.Invoke();
            }
        }

        [Header(" [ Select Event Listner ] ")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Selectable _selectable;
        [SerializeField] private EInputHandlerType _inputType = EInputHandlerType.Both;

        [Header(" Unity Event ")]
        [SerializeField] private bool _useUnityEvent = false;
        [SerializeField] private UnityEvents _unityEvents;

        public event IDeselectEventListener.DeselectEventHandler OnDeselected;

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (!_canvasGroup)
            {
                _canvasGroup = GetComponentInParent<CanvasGroup>();
            }
            if (!_selectable)
            {
                _selectable = GetComponent<Selectable>();
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

            OnDeselected = null;
        }

        public virtual bool CanDeselect()
        {
            if ((_canvasGroup && !_canvasGroup.interactable)
                || (!_selectable.interactable))
                return false;

            return true;
        }
        public void OnDeselect(BaseEventData eventData)
        {
            if (_inputType == EInputHandlerType.Pointer)
                return;

            Deselect(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            switch (_inputType)
            {
                case EInputHandlerType.Both:
                    Deselect(eventData);
                    break;
                case EInputHandlerType.Button:
                    break;
                case EInputHandlerType.Pointer:
                    Deselect(eventData);
                    break;
                default:
                    break;
            }
        }

        private void Deselect(BaseEventData eventData)
        {
            if (!CanDeselect())
                return;

            Log($"{nameof(OnDeselected)}");

            OnDeselected?.Invoke(this, eventData);
        }
    }
}


