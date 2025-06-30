using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public class DeselectEventListener : BaseMonoBehaviour, IDeselectHandler, IPointerExitHandler, IDeselectEventListener
    {
        [Header(" [ Deselect Event Listner ] ")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Selectable _selectable;
        [SerializeField] private EInputHandlerType _inputType = EInputHandlerType.Both;
        [SerializeField] private bool _isDeselectOnBackground = false;

        [Header(" Unity Event ")]
        [SerializeField] private ToggleableUnityEvent _onDeselected;

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
            if (!_canvasGroup)
            {
                _canvasGroup = GetComponentInParent<CanvasGroup>();
            }
            if (!_selectable)
            {
                _selectable = GetComponent<Selectable>();
            }
        }

        protected virtual void OnDestroy()
        {
            _onDeselected.Dispose();

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
            if (_inputType == EInputHandlerType.Button)
                return;

            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                if(_isDeselectOnBackground)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }

        private void Deselect(BaseEventData eventData)
        {
            if (!CanDeselect())
                return;

            Log($"{nameof(OnDeselected)}");

            _onDeselected.Invoke();
            OnDeselected?.Invoke(this, eventData);
        }
    }
}


