using StudioScor.Utilities;
using System.Diagnostics;
using UnityEngine;

namespace StudioScor.InputSystem
{
    public class LookAxisComponent : BaseMonoBehaviour
    {
        [Header(" [ Target ] ")]
        [SerializeField] private Transform axisTarget;
        
        [Header(" [ Look Axis] ")]
        [SerializeField] private LookAxis lookAxis;
        [SerializeField] private float followSpeed = 10f;

        public LookAxis LookAxis => lookAxis;


#if UNITY_EDITOR
        [Header(" [ Draw Gizmos ] ")]
        [SerializeField] private bool isAlwaysDraw = false;
        [SerializeField] private Color gizmosColor = new Color(1f, 0f, 0f, 1f);
#endif

        private void Reset()
        {
#if UNITY_EDITOR
            axisTarget = transform;
#endif
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (isAlwaysDraw)
                DrawGizmos();
#endif
        }
        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!isAlwaysDraw)
                DrawGizmos();
#endif
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected virtual void DrawGizmos()
        {
#if UNITY_EDITOR
            SUtility.SGizmos.DrawArrow(axisTarget.position, axisTarget.forward, gizmosColor);
#endif
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            UpdateLookAxis(deltaTime);
        }

        private void UpdateLookAxis(float deltaTime)
        {
            if (followSpeed <= 0f)
            {
                axisTarget.eulerAngles = new Vector3(LookAxis.Pitch, LookAxis.Yaw);
            }
            else
            {
                float speed = followSpeed * deltaTime;

                float pitch = Mathf.MoveTowardsAngle(axisTarget.eulerAngles.x, LookAxis.Pitch, speed);
                float yaw = Mathf.MoveTowardsAngle(axisTarget.eulerAngles.y, LookAxis.Yaw, speed);

                axisTarget.eulerAngles = new Vector3(pitch, yaw);
            }
        }

        public void SetLookAxis(float pitch, float yaw)
        {
            LookAxis.SetLookAxis(pitch, yaw);

            axisTarget.eulerAngles = new Vector3(pitch, yaw);
        }
    }
}
