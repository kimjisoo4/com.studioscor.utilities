using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System.Diagnostics;

using UnityEngine.InputSystem;


namespace StudioScor.InputSystem
{
    [CreateAssetMenu(menuName = "Event/Input/new Input Button Event", fileName = "InputButton_")]
    public class InputButton : ScriptableObject
    {
        #region Events
        public delegate void ButtonInputHandler(InputButton inputButton);
        public delegate void ButtonStateHandler(InputButton inputButton, bool isPressed);
        public delegate void IgnoreInputHandler(InputButton inputButton, bool isIgnore);
        #endregion

        [SerializeField] private string _InputName;
        [SerializeField] private bool _IgnoreOnUi = false;
        [SerializeField] private bool _IsIngnoreInput = false;
        [SerializeField] private bool _UseDebugMode;

        public string InputName => _InputName;

        private bool _IsPressed = false;
        private bool _PrevPressed = false;

        private InputAction _InputAction;
        public InputAction InputAction => _InputAction;
        public bool IsIgnoreInput => _IsIngnoreInput;
        public bool IsPressed => _IsPressed;

        public event ButtonInputHandler OnPressedInput;
        public event ButtonInputHandler OnReleasedInput;
        public event ButtonStateHandler OnChangedInput;
        public event IgnoreInputHandler OnChangedIgnoreInput;

        public virtual void Setup(PlayerInput playerInput)
        {
            _InputAction = playerInput.currentActionMap.FindAction(_InputName);

            if (_InputAction is null)
            {
                Log("Failed Setup - " + _InputAction);

                return;
            }

            _InputAction.canceled += InputAction_canceled;
            _InputAction.performed += InputAction_performed;
            _InputAction.started += InputAction_started;
            Log("Success Setup - " + _InputAction);
        }

       

        public void SetIgnoreInput(bool ignore)
        {
            if (_IsIngnoreInput == ignore)
                return;

            _IsIngnoreInput = ignore;

            if (IsIgnoreInput)
            {
                OnIgnoreInput();
            }
            else
            {
                EndIgnoreInput();
            }

            OnChangeIgnoreInput();
        }

        protected virtual void OnIgnoreInput()
        {
            _PrevPressed = _IsPressed;
        }
        protected virtual void EndIgnoreInput()
        {
            if (_IsPressed != _PrevPressed)
            {
                _IsPressed = _PrevPressed;

                if (_IsPressed)
                {
                    OnPressInput();
                }
                else
                {
                    OnReleaseInput();
                }
            }
        }

        protected virtual void InputAction_started(InputAction.CallbackContext obj)
        {
            if (_IgnoreOnUi && IsPointerOverGameObject())
                return;
                
            CheckPressInput();
        }
        protected virtual void InputAction_performed(InputAction.CallbackContext obj)
        {
        }

        protected virtual void InputAction_canceled(InputAction.CallbackContext obj)
        {
            if (_IgnoreOnUi && IsPointerOverGameObject())
                return;

            CheckReleaseInput();
        }

        protected void CheckPressInput()
        {
            if (IsIgnoreInput)
            {
                _PrevPressed = true;

                return;
            }
            
            if (!_IsPressed)
            {
                _IsPressed = true;

                OnPressInput();
            }
        }
        protected void CheckReleaseInput()
        {
            if (IsIgnoreInput)
            {
                _PrevPressed = false;

                return;
            }

            if (_IsPressed)
            {
                _IsPressed = false;

                OnReleaseInput();
            }
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

        #region CallBack
        protected void OnChangeIgnoreInput()
        {
            Log(IsIgnoreInput ? "Is Ignore Input" : "Is Not Ignore Input");

            OnChangedIgnoreInput(this, IsIgnoreInput);
        }
        protected void OnPressInput()
        {
            Log("On Press Input");

            OnPressedInput?.Invoke(this);
            OnChangedInput?.Invoke(this, true);
        }
        protected void OnReleaseInput()
        {
            Log("On Release Input");

            OnReleasedInput?.Invoke(this);
            OnChangedInput?.Invoke(this, false);
        }
        #endregion

        #region DEBUG
        [Conditional("UNITY_EDITOR")]
        protected void Log(string log)
        {
            if (!_UseDebugMode)
                return;

            UnityEngine.Debug.Log("Input [ "+ name + " ] : " + log);
        }
        #endregion
    }

}