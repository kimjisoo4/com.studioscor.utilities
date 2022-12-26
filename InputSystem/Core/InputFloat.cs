using UnityEngine;

using UnityEngine.InputSystem;


namespace StudioScor.InputSystem
{
    [CreateAssetMenu(menuName = "StudioScor/Input/new Input Float Event", fileName = "InputFloat_")]
    public class InputFloat : InputButton
    {
        #region Events
        public delegate void InputFloatHandler(InputFloat inputFloat, float value);
        #endregion
        [Header(" [ Input Float ] ")]
        [SerializeField] private float _Speed = 1f;
        [Space(5f)]
        [SerializeField] private bool _InverseValue = false;

        private float _Value;
        private float _PrevValue;

        public float Value => _Value;

        public event InputFloatHandler OnChangedFloatValue;

        public override void Setup(PlayerInputSystem playerInputSystem)
        {
            base.Setup(playerInputSystem);

            _Value = default;
            _PrevValue = default;
        }

        protected override void OnIgnoreInput()
        {
            base.OnIgnoreInput();

            _Value = _PrevValue;
        }

        protected override void InputAction_performed(InputAction.CallbackContext obj)
        {
            if (IsIgnoreInput)
                return;

            _PrevValue = _Value;

            float deviceTypeMultiplier = IsCurrentDeviceMouse? 1f : Time.deltaTime;
            float inverseValue = _InverseValue ? -1f : 1f;

            _Value = obj.ReadValue<Vector2>().y * inverseValue * deviceTypeMultiplier * _Speed;

            OnChangeFloatValue();
        }
        #region Callback
        private void OnChangeFloatValue()
        {
            Log("On Changed Float Value : " + _Value);

            OnChangedFloatValue?.Invoke(this, _Value);
        }
        #endregion
    }

}