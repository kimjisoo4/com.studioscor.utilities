using UnityEngine;
using StudioScor.Utilities;

namespace StudioScor.InputSystem
{

    public class LookAxisComponent : BaseMonoBehaviour
    {
        [Header(" [ Look Axis ] ")]
        [SerializeField] private float threshold = 0.01f;

        [Space(5f)]
        [SerializeField] private float speed = 1f;
        [SerializeField] private float pitchSpeed = 1f;
        [SerializeField] private float yawSpeed = 1f;

        [Space(5f)]
        [SerializeField, Range(-89f, 89f)] private float upClamp = 80f;
        [SerializeField, Range(-89f, 89f)] private float downClamp = -80f;
        [Space(5f)]
        [SerializeField] private bool ignorePitch = false;
        [SerializeField] private bool ignoreYaw = false;
        [Space(5f)]
        [SerializeField] private bool inversePicth = false;
        [SerializeField] private bool inverseYaw = false;

        [SerializeField][SReadOnly] private float picth;
        [SerializeField][SReadOnly] private float yaw;

        public float Speed => speed;
        public float PitchSpeed => pitchSpeed;
        public float YawSpeed => yawSpeed;
        public float UpClamp => upClamp;
        public float DownClamp => downClamp;
        public bool IgnorePitch => ignorePitch;
        public bool IgnoreYaw => ignoreYaw;
        public bool InversePicth => inversePicth;
        public bool InverseYaw => inverseYaw;
        public float Yaw => yaw;
        public float Pitch => picth;


        public void SetIgnoreYaw(bool isIgnore)
        {
            ignoreYaw = isIgnore;
        }
        public void SetIgnorePitch(bool isIgnore)
        {
            ignorePitch = isIgnore;
        }

        public void SetLookAxis(float newPicth, float newYaw)
        {
            if(!IgnorePitch)
                picth = newPicth;

            if(!ignoreYaw)
                yaw = newYaw;
        }

        public void SetLookAxis(Transform transform)
        {
            Vector3 eulerAngles = transform.eulerAngles;

            float picth = eulerAngles.x;
            float yaw = eulerAngles.y;


            picth = SUtility.ClampAngle(picth, DownClamp, UpClamp);
            yaw = SUtility.ClampAngle(yaw, float.MinValue, float.MaxValue);

            Log($"Picth : {picth:N2} || Yaw : {yaw:N2}");

            SetLookAxis(picth, yaw);
        }

        public void InputLookAxis(Vector2 deltaAxis, float multiplie = 1f)
        {
            if (deltaAxis.sqrMagnitude < threshold)
                return;

            float picth = this.picth;
            float yaw = this.yaw;

            if (!IgnorePitch)
            {
                float inversePicth = InversePicth ? -1f : 1f;

                picth += deltaAxis.y * multiplie * Speed * PitchSpeed * inversePicth;
                picth = SUtility.ClampAngle(picth, DownClamp, UpClamp);
            }

            if (!IgnoreYaw)
            {
                float inverseYaw = InverseYaw ? -1f : 1f;

                yaw += deltaAxis.x * multiplie * Speed * YawSpeed * inverseYaw;
                yaw = SUtility.ClampAngle(yaw, float.MinValue, float.MaxValue);
            }

            SetLookAxis(picth, yaw);
        }
    }
}
