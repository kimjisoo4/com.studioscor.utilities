using UnityEngine;

namespace StudioScor.Utilities
{
    public class InputBlockerRelay : BaseMonoBehaviour
    {
        [Header(" [ Input Blocker Relay ] ")]
        [SerializeField] private InputBlocker _inputBlocker;
        [SerializeField] private InputControlSystem _inputControlSystem;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if(!_inputBlocker)
            {
                _inputBlocker = SUtility.FindAssetByType<InputBlocker>();
            }

            if(!_inputControlSystem)
            {
                _inputControlSystem = gameObject.GetComponentInParentOrChildren<InputControlSystem>();
            }
#endif
        }
        private void OnEnable()
        {
            SyncInputState();

            if (_inputBlocker != null)
                _inputBlocker.OnInputStateChanged += InputBlocker_HandleInputStateChanged;
        }

        private void OnDisable()
        {
            if (_inputBlocker != null)
                _inputBlocker.OnInputStateChanged -= InputBlocker_HandleInputStateChanged;
        }

        private void SyncInputState()
        {
            // UI
            if (_inputBlocker.IsUIEnabled)
                _inputControlSystem.OnUIInput();
            else
                _inputControlSystem.EndUIInput();

            // Game
            if (_inputBlocker.IsGameEnabled)
                _inputControlSystem.OnGameInput();
            else
                _inputControlSystem.EndGameInput();
        }

        private void InputBlocker_HandleInputStateChanged(InputBlocker blocker, EBlockInputState current, EBlockInputState previous)
        {
            SyncInputState();
        }
    }
}
