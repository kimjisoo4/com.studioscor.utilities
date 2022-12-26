using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System.Diagnostics;

using UnityEngine.InputSystem;


namespace StudioScor.InputSystem
{


    [CreateAssetMenu(menuName = "StudioScor/Input/new Input Button Event", fileName = "InputButton_")]
    public class InputButton : ScriptableObject
    {
        #region Events
        public delegate void ButtonInputHandler(InputButton inputButton);
        public delegate void ButtonStateHandler(InputButton inputButton, bool isPressed);
        public delegate void IgnoreInputHandler(InputButton inputButton, bool isIgnore);
        #endregion

        [Header(" [ Input Button ] ")]

        [SerializeField] private string _InputName;
        [SerializeField] private bool _IgnoreOnUi = false;
        [SerializeField] private bool _IsIngnoreInput = false;

        [Header(" [ Use Debug ] ")]
        [SerializeField] private bool _UseDebug;

        public string InputName => _InputName;

        private bool _IsPressed = false;
        private bool _PrevPressed = false;

        private PlayerInputSystem _PlayerInputSystem;

        private InputAction _InputAction;
        public InputAction InputAction => _InputAction;
        public bool IsIgnoreInput => _IsIngnoreInput;
        public bool IsPressed => _IsPressed;
        public bool IsCurrentDeviceMouse => _PlayerInputSystem.IsCurrentDeviceMouse;

        public event ButtonInputHandler OnPressedInput;
        public event ButtonInputHandler OnReleasedInput;
        public event ButtonStateHandler OnChangedInput;
        public event IgnoreInputHandler OnChangedIgnoreInput;


        #region EDITOR ONLY

        [Conditional("UNITY_EDITOR")]
        protected void Log(string log)
        {
#if UNITY_EDITOR
            if (!_UseDebug)
                return;

            UnityEngine.Debug.Log("Input [ " + name + " ] : " + log);
#endif
        }
        #endregion


        public virtual void Setup(PlayerInputSystem playerInputSystem)
        {
            ResetInput();

            _PlayerInputSystem = playerInputSystem;

            _InputAction = _PlayerInputSystem.PlayerInput.currentActionMap.FindAction(_InputName);

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

        public virtual void ResetInput()
        {
            _IsPressed = false;
            _PrevPressed = false;
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
            if (_IgnoreOnUi && _PlayerInputSystem.InPointerOverGameobject)
                return;
                
            CheckPressInput();
        }
        protected virtual void InputAction_performed(InputAction.CallbackContext obj)
        {
        }

        protected virtual void InputAction_canceled(InputAction.CallbackContext obj)
        {
            if (_IgnoreOnUi && _PlayerInputSystem.InPointerOverGameobject)
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
        

        #region CallBack
        protected void OnChangeIgnoreInput()
        {
            Log(IsIgnoreInput ? "Is Ignore Input" : "Is Not Ignore Input");

            OnChangedIgnoreInput?.Invoke(this, IsIgnoreInput);
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


    }

}