using UnityEngine;

using StudioScor.Utilities;

namespace StudioScor.Editor
{
    public class DrawTriggerCollisions : BaseMonoBehaviour
    {
        [Header(" [ Draw Trigger Collisions ] ")]
        [SerializeField] private Color _Color = new Color(1f, 0f, 0f, 0.5f);
        [SerializeField] private Color _SelectedColor = new Color(1f, 0f, 0f, 0.8f);

        private void OnDrawGizmos()
        {
            Matrix4x4 matrix = new();

            matrix.SetTRS(transform.position, transform.rotation, transform.localScale);

            Gizmos.matrix = matrix;

            Gizmos.color = _Color;

            var boxCollisions = GetComponents<BoxCollider>();

            foreach (var boxCollision in boxCollisions)
            {
                if(boxCollision.isTrigger)
                    Gizmos.DrawCube(boxCollision.center, boxCollision.size);
            }

            var sphereCollisions = GetComponents<SphereCollider>();

            foreach (var sphereCollision in sphereCollisions)
            {
                if(sphereCollision.isTrigger)
                    Gizmos.DrawSphere(sphereCollision.center, sphereCollision.radius);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Matrix4x4 matrix = new();

            matrix.SetTRS(transform.position, transform.rotation, transform.localScale);

            Gizmos.matrix = matrix;

            Gizmos.color = _SelectedColor;

            var boxCollisions = GetComponents<BoxCollider>();

            foreach (var boxCollision in boxCollisions)
            {
                if (boxCollision.isTrigger)
                    Gizmos.DrawCube(boxCollision.center, boxCollision.size);
            }

            var sphereCollisions = GetComponents<SphereCollider>();

            foreach (var sphereCollision in sphereCollisions)
            {
                if (sphereCollision.isTrigger)
                    Gizmos.DrawSphere(sphereCollision.center, sphereCollision.radius);
            }
        }
    }

}