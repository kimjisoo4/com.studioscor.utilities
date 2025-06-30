using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StudioScor.Utilities
{
    public class CancelEventListener : BaseMonoBehaviour, ICancelHandler, ICancelEventListener
    {
        [Header(" [ Cancel Event Listner ] ")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Selectable _selectable;

        [Header(" Unity Event ")]
        [SerializeField] private ToggleableUnityEvent _onCanceled;
        [SerializeField] private ToggleableUnityEvent _onCanceledFailed;

        public event ICancelEventListener.CancelEventHandler OnCanceled;
        public event ICancelEventListener.CancelEventHandler OnCanceledFailed;

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
            _onCanceled.Dispose();
            _onCanceledFailed.Dispose();

            OnCanceled = null;
            OnCanceledFailed = null;
        }

        public virtual bool CanCancel()
        {
            if ((_canvasGroup && !_canvasGroup.interactable)
                || (!_selectable.interactable))
                return false;

            return true;
        }

        public void OnCancel(BaseEventData eventData)
        {
            if (!CanCancel())
            {
                Log($"{nameof(OnCanceledFailed)}");

                _onCanceledFailed.Invoke();
                OnCanceledFailed?.Invoke(this, eventData);
                return;
            }

            Log($"{nameof(OnCanceled)}");

            _onCanceled.Invoke();
            OnCanceled?.Invoke(this, eventData);
        }
    }
}


