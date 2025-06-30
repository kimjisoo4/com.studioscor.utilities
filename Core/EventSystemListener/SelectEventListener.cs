using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public class SelectEventListener : BaseMonoBehaviour, ISelectHandler, IPointerEnterHandler, ISelectEventListener
    {

        [Header(" [ Select Event Listner ] ")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Selectable _selectable;
        [SerializeField] private EInputHandlerType _inputType = EInputHandlerType.Both;

        [Header(" Unity Event ")]
        [SerializeField] private ToggleableUnityEvent _onSelected;

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
        }
        protected virtual void OnDestroy()
        {
            _onSelected.Dispose();
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

            _onSelected.Invoke();
            OnSelected?.Invoke(this, eventData);
        }
    }
}


