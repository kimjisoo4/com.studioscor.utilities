using StudioScor.Utilities;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StudioScor.Utilities
{

    public class InputControlSystem : BaseMonoBehaviour
    {
        [Header(" [ Input Control System ] ")]
        [SerializeField] private EBlockInputState _state;

        public bool UseInput => _state.HasFlag(EBlockInputState.None);
        public bool UseUIInput => _state.HasFlag(EBlockInputState.UI);
        public bool UseGameInput => _state.HasFlag(EBlockInputState.Game);


#if UNITY_EDITOR
        [SerializeField]private bool _cursorVisible;
        [SerializeField]private CursorLockMode _cursorLockMode;
#endif

        private void Start()
        {
            UpdateUIControl();
        }

        private void UpdateUIControl()
        {
            UpdateUIInput();
            UpdateGameInput();
        }

        public void OnUIInput()
        {
            if (UseUIInput)
                return;

            _state |= EBlockInputState.UI;

            Log(nameof(OnUIInput));

            UpdateUIInput();
        }

        public void EndUIInput()
        {
            if (!UseUIInput)
                return;

            _state &= ~EBlockInputState.UI;

            Log(nameof(EndUIInput));

            UpdateUIInput();
        }


        public void OnGameInput()
        {
            if (UseGameInput)
                return;

            _state |= EBlockInputState.Game;

            Log(nameof(OnGameInput));

            UpdateGameInput();
        }

        public void EndGameInput()
        {
            if (!UseGameInput)
                return;

            _state &= ~EBlockInputState.Game;

            Log(nameof(EndGameInput));

            UpdateGameInput();
        }

        private void UpdateUIInput()
        {
            SetCursorState(UseUIInput);

            var playerInput = PlayerInput.all.ElementAtOrDefault(0);

            if (!playerInput)
                return;

            if (UseUIInput)
            {
                playerInput.actions.FindActionMap("UI").Enable();
            }
            else
            {
                playerInput.actions.FindActionMap("UI").Disable();
            }
        }
        private void UpdateGameInput()
        {
            var playerInput = PlayerInput.all.ElementAtOrDefault(0);

            if (!playerInput)
                return;

            if (UseGameInput)
            {
                playerInput.actions.FindActionMap("Player").Enable();
            }
            else
            {
                playerInput.actions.FindActionMap("Player").Disable();
            }
        }


        private void SetCursorState(bool visible)
        {
            if(visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

#if UNITY_EDITOR
            _cursorVisible = Cursor.visible;
            _cursorLockMode = Cursor.lockState;
#endif
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                SetCursorState(UseUIInput);
            }
            else
            {
                SetCursorState(true);
            }
        }
    }
}
