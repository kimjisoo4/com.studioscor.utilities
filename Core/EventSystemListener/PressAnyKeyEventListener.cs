using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace StudioScor.Utilities
{
    public class PressAnyKeyEventListener : BaseMonoBehaviour, IPressAnyKeyEventListener
    {
        [Header(" [ Submit Event Listner ] ")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private EInputHandlerType _inputType = EInputHandlerType.Both;

        [Header(" Unity Event ")]
        [SerializeField] private ToggleableUnityEvent _onPressAnyKey;
        [SerializeField] private ToggleableUnityEvent _onPressAnyKeyFailed;

        public event IPressAnyKeyEventListener.PressAnyKeyEventHandler OnPressAnyKey;
        public event IPressAnyKeyEventListener.PressAnyKeyEventHandler OnPressAnyKeyFailed;

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
            if (!_canvasGroup)
            {
                _canvasGroup = GetComponentInParent<CanvasGroup>();
            }

        }
        protected virtual void OnDestroy()
        {
            _onPressAnyKey.Dispose();
            _onPressAnyKeyFailed.Dispose();

            OnPressAnyKey = null;
            OnPressAnyKeyFailed = null;
        }

        void Update()
        {
            switch (_inputType)
            {
                case EInputHandlerType.Both:
                    if (CheckPressedButton() || CheckPressedPointer())
                        OnPress();
                    break;
                case EInputHandlerType.Button:
                    if(CheckPressedButton())
                        OnPress();
                    break;
                case EInputHandlerType.Pointer:
                    if (CheckPressedPointer())
                        OnPress();
                    break;
                default:
                    break;
            }
        }

        private bool CheckPressedButton()
        {
            return Keyboard.current.anyKey.wasPressedThisFrame;
        }
        private bool CheckPressedPointer()
        {
            if (!Pointer.current.press.wasPressedThisFrame)
            {
                var mouse = Mouse.current;

                if (mouse is not null && (mouse.rightButton.wasPressedThisFrame || mouse.middleButton.wasPressedThisFrame || mouse.backButton.wasPressedThisFrame || mouse.forwardButton.wasPressedThisFrame))
                    return true;
            }
            else
            {
                return true;
            }

            return false;
        }
        public virtual bool CanPressAnyKey()
        {
            if (!enabled)
                return false;

            if (_canvasGroup && !_canvasGroup.interactable)
                return false;

            return true;
        }
        private void OnPress()
        {
            BaseEventData eventData = new BaseEventData(EventSystem.current);

            if (!CanPressAnyKey())
            {
                Log($"{nameof(OnPressAnyKeyFailed)}");
                OnPressAnyKeyFailed?.Invoke(this, eventData);
                return;
            }
            else
            {
                Log($"{nameof(OnPressAnyKey)}");

                OnPressAnyKey?.Invoke(this, eventData);
            }
        }
    }
}


