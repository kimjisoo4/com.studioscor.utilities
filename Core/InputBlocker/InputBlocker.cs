using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utility/new InputBlocker", fileName = "InputBlocker")]
    public class InputBlocker : BaseScriptableObject
    {
        public delegate void InputBlockerEventHandler(InputBlocker inputBlocker, EBlockInputState newInputState, EBlockInputState prevInputState);

        private readonly Dictionary<object, EBlockInputState> _inputBlocks = new();

        [SerializeField] private EBlockInputState _inputState;
        
        private EBlockInputState _runtimeInputState;
        public EBlockInputState InputState
        {
            get => _runtimeInputState;
            private set
            {
                if (_runtimeInputState == value)
                    return;

                var prevValue = _runtimeInputState;
                _runtimeInputState = value;

                RaiseOnInputStateChanged(prevValue);
            }
        }

#if UNITY_EDITOR
        [SerializeField]private List<string> EDITOR_Requests = new();
#endif

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void UpdateDebug()
        {
#if UNITY_EDITOR
            EDITOR_Requests.Clear();
            EDITOR_Requests.AddRange(_inputBlocks.Keys.ToList().ConvertAll( x => x.ToString()));
#endif
        }

        public bool IsUIEnabled => _runtimeInputState.HasFlag(EBlockInputState.UI);
        public bool IsGameEnabled => _runtimeInputState.HasFlag(EBlockInputState.Game);

        public event InputBlockerEventHandler OnInputStateChanged;

        protected override void OnReset()
        {
            base.OnReset();

            _inputBlocks.Clear();
            _runtimeInputState = _inputState;
            OnInputStateChanged = null;
        }

        public void BlockInput(object key, EBlockInputState disableFlags)
        {
            if (key == null) 
                return;
            
            if (_inputBlocks.TryGetValue(key, out var current) && current == disableFlags)
                return;

            Log($"{nameof(BlockInput)} - {key.ToString()} : {disableFlags.ToString()}");
            _inputBlocks[key] = disableFlags;
            
            UpdateState();
        }

        public void UnblockInput(object key)
        {
            if (key == null) 
                return;

            Log($"{nameof(UnblockInput)} - {key.ToString()}");
            _inputBlocks.Remove(key);
            
            UpdateState();
        }

        private EBlockInputState CalculateEffectiveInputState()
        {
            var state = EBlockInputState.UI | EBlockInputState.Game;

            foreach (var pair in _inputBlocks.Values)
            {
                state &= ~pair;
            }

            return state;
        }

        private void UpdateState()
        {
            InputState = CalculateEffectiveInputState();
            UpdateDebug();
        }

        private void RaiseOnInputStateChanged(EBlockInputState prevState)
        {
            Log($"{nameof(OnInputStateChanged)} - Current : {_runtimeInputState} || Prev : {prevState}");

            OnInputStateChanged?.Invoke(this, _runtimeInputState, prevState);
        }
    }
}
