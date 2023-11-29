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

        [Conditional("UNITY_EDITOR")]
        protected virtual void DrawGizmos()
        {
#if UNITY_EDITOR
            SUtility.SGizmos.DrawArrow(axisTarget.position, axisTarget.forward, gizmosColor);
#endif
        }

        private void Update()
        {
            axisTarget.eulerAngles = new Vector3(LookAxis.Pitch, LookAxis.Yaw);
        }
    }
}
