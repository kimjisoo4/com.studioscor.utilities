using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public class FieldOfViewToSphereCastAll : MonoBehaviour
    {
        [Header("[ Setting ]")]
        [Range(0f, 360f)]
        [SerializeField]
        private float _HorizontalAngle = 90f;
        [Range(0f, 360f)]
        [SerializeField]
        private float _VerticalAngle = 90f;
        [SerializeField]
        private float _Radius = 5f;

        [Header("[ Check Layer ]")]
        [SerializeField]
        private LayerMask _LayerMask;

#if UNITY_EDITOR
        [Header("[ Debug ]")]
        [SerializeField]
        private bool _UseDrawGizmo = true;
#endif

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!_UseDrawGizmo)
                return;

            if (Cast(out List<Collider> hits))
            {
                Gizmos.color = Color.green;

                foreach (Collider hit in hits)
                {
                    Gizmos.DrawSphere(hit.transform.position, 0.1f);
                }
            }
            else
            {
                Gizmos.color = Color.red;
            }


            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetTRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.matrix = matrix;

            Gizmos.DrawWireSphere(Vector3.zero, _Radius);

            Vector3 endDistance = new Vector3(0, 0, _Radius);

            Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, -_HorizontalAngle * 0.5f, 0) * endDistance);
            Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, _HorizontalAngle * 0.5f, 0) * endDistance);

            Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(-_VerticalAngle * 0.5f, 0, 0) * endDistance);
            Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(_VerticalAngle * 0.5f, 0, 0) * endDistance);

            Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(_VerticalAngle * 0.5f, -_HorizontalAngle * 0.5f, 0) * endDistance);
            Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(_VerticalAngle * 0.5f, _HorizontalAngle * 0.5f, 0) * endDistance);
            Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(_VerticalAngle * 0.5f, 0, 0) * endDistance);

            Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(-_VerticalAngle * 0.5f, -_HorizontalAngle * 0.5f, 0) * endDistance);
            Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(-_VerticalAngle * 0.5f, _HorizontalAngle * 0.5f, 0) * endDistance);
            Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(-_VerticalAngle * 0.5f, 0, 0) * endDistance);


            DrawArc();
#endif
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void DrawArc()
        {
#if UNITY_EDITOR
            Vector3 endDistance = new Vector3(0, 0, _Radius);

            int horizontalCount = Mathf.RoundToInt(_HorizontalAngle / 10f);

            float horizontalTickAngle = _HorizontalAngle / horizontalCount;

            for (int i = 1; i <= horizontalCount; i++)
            {
                float startAngle = (-_HorizontalAngle * 0.5f) + (horizontalTickAngle * (i - 1));
                float endAngle = (-_HorizontalAngle * 0.5f) + (horizontalTickAngle * i);

                for (int j = -1; j <= 1; j++)
                {
                    Vector3 startPosition = Quaternion.Euler(_VerticalAngle * j * 0.5f, startAngle, 0) * endDistance;
                    Vector3 endPosition = Quaternion.Euler(_VerticalAngle * j * 0.5f, endAngle, 0) * endDistance;

                    Gizmos.DrawLine(startPosition,
                                    endPosition);
                }
            }

            int verticalCount = Mathf.RoundToInt(_VerticalAngle / 10f);
            float verticalTickAngle = _VerticalAngle / verticalCount;

            for (int i = 1; i <= verticalCount; i++)
            {
                float startAngle = (-_VerticalAngle * 0.5f) + (verticalTickAngle * (i - 1));
                float endAngle = (-_VerticalAngle * 0.5f) + (verticalTickAngle * i);

                for (int j = -1; j <= 1; j++)
                {
                    Vector3 startPosition = Quaternion.Euler(startAngle, _HorizontalAngle * j * 0.5f, 0) * endDistance;
                    Vector3 endPosition = Quaternion.Euler(endAngle, _HorizontalAngle * j * 0.5f, 0) * endDistance;

                    Gizmos.DrawLine(startPosition,
                                    endPosition);
                }
            }
#endif
        }


        public bool Cast(out List<Collider> Hits)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _Radius, _LayerMask);

            Hits = new List<Collider>();

            foreach (Collider hit in hits)
            {
                Vector3 direction = hit.transform.position - transform.position;

                Vector3 angle = Quaternion.FromToRotation(transform.forward, direction).eulerAngles;

                if (angle.y > 180)
                    angle.y -= 360f;

                if (angle.x > 180)
                    angle.x -= 360f;

                if (Mathf.Abs(angle.y) <= _HorizontalAngle * 0.5f && Mathf.Abs(angle.x) <= _VerticalAngle * 0.5f)
                {
                    Hits.Add(hit);
                }
            }

            return Hits.Count > 0;
        }
    }
}