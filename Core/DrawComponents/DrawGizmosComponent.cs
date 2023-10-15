using UnityEngine;

namespace StudioScor.Utilities
{
    public class DrawGizmosComponent : BaseMonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private Mesh targetMesh;
        [SerializeField] private bool isAlwaysDraw = false;
        [SerializeField] private Color color = new Color(0, 1, 0, 0.2f);
        [SerializeField] private Color lineColor = new Color(0, 1, 0, 0.5f);
#endif

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
        private void DrawGizmos()
        {
#if UNITY_EDITOR
            if (!targetMesh)
                return; 


            Matrix4x4 matrix = new();

            matrix.SetTRS(transform.position, transform.rotation, transform.localScale);

            Gizmos.matrix = matrix;

            Gizmos.color = color;
            Gizmos.DrawMesh(targetMesh);

            Gizmos.color = lineColor;
            Gizmos.DrawWireMesh(targetMesh);
#endif
        }
    }
}