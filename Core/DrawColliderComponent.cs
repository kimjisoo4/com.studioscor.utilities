using UnityEngine;

namespace StudioScor.Utilities
{
    public class DrawColliderComponent : BaseMonoBehaviour
    {
        #region EDITOR ONLY

#if UNITY_EDITOR
        private enum EColliderType
        {
            Collider,
            Trigger,
            Both,
        }

        [SerializeField] private EColliderType drawType = EColliderType.Both;
        [SerializeField] private bool isAlwaysDraw = false;
        [SerializeField] private Color color = new Color(0, 1, 0, 0.2f);
#endif
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void DrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = color;

            var colliders = GetComponentsInChildren<Collider>();

            foreach (var collider in colliders)
            {
                switch (drawType)
                {
                    case EColliderType.Collider:
                        if (collider.isTrigger)
                            continue;
                        break;
                    case EColliderType.Trigger:
                        if (!collider.isTrigger)
                            continue;
                        break;
                    default:
                        break;
                }

                Matrix4x4 matrix = new();

                matrix.SetTRS(collider.transform.position, collider.transform.rotation, collider.transform.localScale);

                Gizmos.matrix = matrix;

                if(collider is BoxCollider boxCollider)
                {
                    Gizmos.DrawCube(boxCollider.center, boxCollider.size);
                }
                else if(collider is SphereCollider sphereCollider)
                {
                    Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
                }
                else if(collider is MeshCollider meshCollider)
                {
                    Gizmos.DrawMesh(meshCollider.sharedMesh);
                }
                else if(collider is CapsuleCollider capsuleCollider)
                {
                    float radius = capsuleCollider.radius;
                    float height = capsuleCollider.height;

                    bool isRadius = radius > height * 0.5;

                    float offset = isRadius ? 0f : (capsuleCollider.height * 0.5f) - radius;

                    Vector3 start = capsuleCollider.center + Vector3.up * offset;
                    Vector3 end = capsuleCollider.center - Vector3.up * offset;

                    Gizmos.DrawSphere(start, capsuleCollider.radius);
                    Gizmos.DrawSphere(end, capsuleCollider.radius);

                    Gizmos.DrawLine(start + Vector3.right * radius, end + Vector3.right * radius);
                    Gizmos.DrawLine(start + Vector3.left * radius, end + Vector3.left * radius);
                    Gizmos.DrawLine(start + Vector3.forward * radius, end + Vector3.forward * radius);
                    Gizmos.DrawLine(start + Vector3.back * radius, end + Vector3.back * radius);
                }
            }
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

        #endregion
    }
}