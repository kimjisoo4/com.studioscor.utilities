using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using UnityEngine.InputSystem;

using StudioScor.Utilities;

namespace StudioScor.InputSystem
{
    public class PlayerInputSystem : BaseMonoBehaviour
    {
        [Header(" [ Player Input System ] ")]
        [SerializeField] private PlayerInput _PlayerInput;
        [SerializeField] private List<InputButton> _InputButtons;
        [Space(5f)]
        [Header("Mouse Cursor Settings")]
        [SerializeField] private bool _CursorLocked = true;
        public PlayerInput PlayerInput => _PlayerInput;

        private bool _IsCurrentDeviceMouse;
        private bool _IsPointerOverGameobject = false;

        private bool _NeedPointerOverCheck;
        public bool InPointerOverGameobject
        {
            get
            {
                if (_NeedPointerOverCheck)
                {
                    _IsPointerOverGameobject = IsPointerOverGameObject();

                    _NeedPointerOverCheck = false;
                }

                return _IsPointerOverGameobject;
            }
        }
        public bool IsCurrentDeviceMouse => _IsCurrentDeviceMouse;

        private const string KEYBOARD_MOUSE= "Keyboard&Mouse";


#if UNITY_EDITOR
        private void Reset()
        {
            TryGetComponent(out _PlayerInput);
        }
#endif

        private void Awake()
        {
            foreach (var input in _InputButtons)
            {
                input.Setup(this);
            }

            if (PlayerInput.currentControlScheme == KEYBOARD_MOUSE)
            {
                _IsCurrentDeviceMouse = true;
            }

            PlayerInput.onControlsChanged += PlayerInput_onControlsChanged;

            SetCursorState(_CursorLocked);
        }

        private void PlayerInput_onControlsChanged(PlayerInput obj)
        {
            if (PlayerInput.currentControlScheme == KEYBOARD_MOUSE)
            {
                _IsCurrentDeviceMouse = true;
            }
        }

        private void LateUpdate()
        {
            if (!_NeedPointerOverCheck)
                _NeedPointerOverCheck = true;
        }

        protected bool IsPointerOverGameObject()
        {
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };

            var raycastResultsList = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);

            return raycastResultsList.Count > 0;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(_CursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Confined : CursorLockMode.None;
        }
    }
}