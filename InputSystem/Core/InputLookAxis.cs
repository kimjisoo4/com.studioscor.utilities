using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using StudioScor.Utilities;

namespace StudioScor.InputSystem
{
    [CreateAssetMenu(menuName = "StudioScor/Input/new Input Look Axis Event", fileName = "InputLookAxis_")]
    public class InputLookAxis : InputButton
    {
        #region Events
        public delegate void OnChangedLookHandler(InputLookAxis lookAxis, float picth, float yaw);
        #endregion

        [Header(" [ Look Axis ] ")]
        [SerializeField] private float _Threshold = 0.01f;

        [Space(5f)]
        [SerializeField] private float _Speed = 1f;
        [SerializeField] private float _PitchSpeed = 1f;
        [SerializeField] private float _YawSpeed = 1f;

        [Space(5f)]
        [SerializeField, Range(-89f, 89f)] private float _UpClamp = 80f;
        [SerializeField, Range(-89f, 89f)] private float _DownClamp = -80f;
        [Space(5f)]
        [SerializeField] private bool _IgnorePitch;
        [SerializeField] private bool _IgnoreYaw;
        [Space(5f)]
        [SerializeField] private bool _InversePicth;
        [SerializeField] private bool _InverseYaw;

        public float Yaw => _Yaw;
        public float Pitch => _Picth;

        private float _Yaw;
        private float _Picth;

        private float _PrevYaw;
        private float _PrevPicth;

        public event OnChangedLookHandler OnChangedLookValue;

        public override void ResetInput()
        {
            base.ResetInput();

            _Yaw = default;
            _Picth = default;
            _PrevYaw = default;
            _PrevPicth = default;

            OnChangeLookValue();
        }

        protected override void OnIgnoreInput()
        {
            base.OnIgnoreInput();

            _Yaw = _PrevYaw;
            _Picth = _PrevPicth;
        }

        public void SetIgnoreYaw(bool isIgnore)
        {
            _IgnoreYaw = isIgnore;
        }
        public void SetIgnorePitch(bool isIgnore)
        {
            _IgnorePitch = isIgnore;
        }

        protected override void InputAction_performed(InputAction.CallbackContext obj)
        {
            if (IsIgnoreInput)
            {
                return;
            }

            Vector3 axis = obj.ReadValue<Vector2>();

            if (axis.sqrMagnitude >= _Threshold)
            {
                float deviceTypeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                if (!_IgnorePitch)
                {
                    _PrevPicth = _Picth;

                    float inversePicth = _InversePicth ? -1f : 1f;

                    _Picth += axis.y * deviceTypeMultiplier * _Speed * _PitchSpeed * inversePicth;

                    _Picth = Utility.ClampAngle(_Picth, _DownClamp, _UpClamp);
                }

                if (!_IgnoreYaw)
                {
                    _PrevYaw = _Yaw;

                    float inverseYaw = _InverseYaw ? -1f : 1f;

                    _Yaw += axis.x * deviceTypeMultiplier * _Speed * _YawSpeed * inverseYaw;

                    _Yaw = Utility.ClampAngle(_Yaw, float.MinValue, float.MaxValue);
                }

            }

            OnChangeLookValue();
        }
        protected override void InputAction_canceled(InputAction.CallbackContext obj)
        {
            base.InputAction_canceled(obj);

            OnChangeLookValue();
        }

        protected void OnChangeLookValue()
        {
            Log("On Changed Value - Pitch : " + _Picth + " Yaw :" + _Yaw);

            OnChangedLookValue?.Invoke(this, _Picth, _Yaw);
        }
    }
}
