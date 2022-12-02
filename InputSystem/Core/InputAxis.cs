using UnityEngine;
using System.Collections;

using UnityEngine.InputSystem;


namespace StudioScor.InputSystem
{

    [CreateAssetMenu(menuName = "Event/Input/new Input Axis Event", fileName = "InputAxis_")]
    public class InputAxis : InputButton
    {
        #region Events
        public delegate void ValueChangeHandler(InputAxis inputButton, Vector2 direction, float strength);
        #endregion

        private Vector2 _Axis;
        public Vector2 Axis => _Axis;

        private Vector2 _PrevAxis;
        private float _PrevStrength;

        private float _Strength;
        public float Strength => _Strength;

        public event ValueChangeHandler OnChangedValue;

        protected override void OnIgnoreInput()
        {
            base.OnIgnoreInput();

            _PrevAxis = _Axis;
            _PrevStrength = _Strength;
        }
        protected override void EndIgnoreInput()
        {
            base.EndIgnoreInput();

            if (_Axis != _PrevAxis || _Strength != _PrevStrength)
            {
                _Axis = _PrevAxis;
                _Strength = _PrevStrength;

                OnChangeValue();
            }
        }
        protected override void InputAction_performed(InputAction.CallbackContext obj)
        {
            if (IsIgnoreInput)
            {
                _PrevAxis = obj.ReadValue<Vector2>();
                _PrevStrength = _PrevAxis.magnitude;

                return;
            }

            _Axis = obj.ReadValue<Vector2>();
            _Strength = _Axis.magnitude;

            OnChangeValue();
        }
        protected override void InputAction_canceled(InputAction.CallbackContext obj)
        {
            base.InputAction_canceled(obj);

            if (IsIgnoreInput)
            {
                _PrevAxis = default;
                _PrevStrength = default;

                return;
            }

            _Axis = default;
            _Strength = default;

            OnChangeValue();
        }

        protected void OnChangeValue()
        {
            Log("On Changed Value - Direction : " + Axis + " Strength :" + Strength);

            OnChangedValue?.Invoke(this, Axis, Strength);
        }
    }

}