using UnityEngine;
namespace StudioScor.Utilities.Editor
{

    public abstract class DictionaryContainerEditor<TKey, TValue> : UnityEditor.Editor
    {
        public abstract string KeyName(TKey key);
        public abstract string ValueName(TValue value);

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying)
                return;

            GUILayout.Space(5f);
            SEditorUtility.GUI.DrawLine(4f);
            GUILayout.Space(5f);
            var dictionary = target as DictionaryContainer<TKey, TValue>;

            if (dictionary.Container is null)
                return;

            foreach (var item in dictionary.Container)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(KeyName(item.Key));
                GUILayout.FlexibleSpace();
                GUILayout.Label(ValueName(item.Value));
                GUILayout.Space(5f);
                GUILayout.EndHorizontal();
                SEditorUtility.GUI.DrawLine();
            }
        }
    }


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