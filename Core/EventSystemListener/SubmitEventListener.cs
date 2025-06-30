using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public class SubmitEventListener : BaseMonoBehaviour, ISubmitHandler, IPointerClickHandler, ISubmitEventListener
    {
        [Header(" [ Submit Event Listner ] ")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Selectable _selectable;
        [SerializeField] private EInputHandlerType _inputType = EInputHandlerType.Both;

        [Header(" Unity Event ")]
        [SerializeField] private ToggleableUnityEvent _onSubmited;
        [SerializeField] private ToggleableUnityEvent _onSubmitedFailed;

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
        }
        protected virtual void OnDestroy()
        {
            _onSubmited.Dispose();
            _onSubmitedFailed.Dispose();

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
                
                _onSubmitedFailed.Invoke();
                OnFailedSubmited?.Invoke(this, eventData);
                return;
            }

            Log($"{nameof(OnSubmited)}");

            _onSubmited.Invoke();
            OnSubmited?.Invoke(this, eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_inputType == EInputHandlerType.Button)
                return;

            if (!CanSubmit())
            {
                Log($"{nameof(OnFailedSubmited)}");

                _onSubmitedFailed.Invoke();
                OnFailedSubmited?.Invoke(this, eventData);
                return;
            }

            Log($"{nameof(OnSubmited)}");

            _onSubmited.Invoke();
            OnSubmited?.Invoke(this, eventData);
        }
    }
}


