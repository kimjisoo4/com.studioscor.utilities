namespace StudioScor.Utilities
{
    using UnityEngine;
    using System.Collections.Generic;

    public class FieldOfViewToCone : MonoBehaviour
    {
        [Header("[ Setting ]")]
        [Range(0f, 360f)]
        [SerializeField]
        private float _Angle = 90f;
        [SerializeField]
        private float _Distance = 5f;
        [Header("[ Check Layer ]")]
        [SerializeField]
        private LayerMask _LayerMask;

#if UNITY_EDITOR
        [Header("[ Debug ]")]
        [SerializeField] private bool _UseDrawGizmo = true;
#endif

        #region Draw Gizmose
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


            Vector3 distance = new Vector3(0, 0, _Distance);
            float halfAngle = _Angle * 0.5f;

            Gizmos.DrawLine(Vector3.zero, distance);

            List<Vector3> lines = new List<Vector3>();

            // Line //
            for (int i = 0; i < 17; i++)
            {
                lines.Add(Quaternion.Euler(0, 0, 22.5f * i) * (Quaternion.Euler(halfAngle, 0, 0) * distance));

                Gizmos.DrawLine(Vector3.zero, lines[i]);
            }
            for (int i = 0; i < lines.Count - 1; i++)
            {
                Gizmos.DrawLine(lines[i], lines[i + 1]);
            }

            // Circle //
            int circleCount = Mathf.RoundToInt(halfAngle / 20f);
            float tickCicleAngle = halfAngle / circleCount;

            for (int j = 0; j < circleCount; j++)
            {
                List<Vector3> circleLines = new List<Vector3>();

                for (int i = 0; i < 17; i++)
                {
                    circleLines.Add(Quaternion.Euler(0, 0, 22.5f * i) * (Quaternion.Euler(tickCicleAngle * j, 0, 0) * distance));
                }

                for (int i = 0; i < circleLines.Count - 1; i++)
                {
                    Gizmos.DrawLine(circleLines[i], circleLines[i + 1]);
                }
            }


            // Arc //
            int count = Mathf.RoundToInt(_Angle / 10f);
            float tickAngle = _Angle / count;

            for (int i = 1; i <= count; i++)
            {
                float startAngle = (tickAngle * (i - 1)) - halfAngle;
                float endAngle = (tickAngle * i) - halfAngle;

                for (int j = 0; j < 8; j++)
                {
                    Vector3 startPosition = Quaternion.Euler(0, 0, 22.5f * j) * Quaternion.Euler(0, startAngle, 0) * distance;
                    Vector3 endPosition = Quaternion.Euler(0, 0, 22.5f * j) * Quaternion.Euler(0, endAngle, 0) * distance;

                    Gizmos.DrawLine(startPosition, endPosition);
                }
            }
#endif
        }

#endregion
        
        public bool Cast(out List<Collider> Hits)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _Distance, _LayerMask);

            Hits = new List<Collider>();

            foreach (Collider hit in hits)
            {
                Vector3 direction = transform.Direction(hit.transform);

                float angle = Vector3.Angle(transform.forward, direction);

                if (angle > 180)
                    angle -= 360f;

                if (Mathf.Abs(angle) <= _Angle * 0.5f)
                {
                    Hits.Add(hit);
                }
            }

            return Hits.Count > 0;
        }
    }
}