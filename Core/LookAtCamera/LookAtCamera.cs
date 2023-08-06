using UnityEngine;
using UnityEngine.Animations;

namespace StudioScor.Utilities
{
    [RequireComponent(typeof(RotationConstraint))]
    public class LookAtCamera : BaseMonoBehaviour
    {
        [Header(" [ Look At Camera Component ] ")]
        [SerializeField] private RotationConstraint constraint;
        [SerializeField] private Camera targetCamera;

        private void Reset()
        {
#if UNITY_EDITOR
            if(gameObject.TryGetComponentInParentOrChildren(out constraint))
            {
                constraint.constraintActive = true;
            }
#endif
        }

        private void Awake()
        {
            if (!targetCamera)
                targetCamera = Camera.main;

            ConstraintSource source = new()
            {
                sourceTransform = targetCamera.transform,
                weight = 1f,
            };

            if (constraint.sourceCount > 0)
            {
                constraint.SetSource(0, source);
            }
            else
            {
                constraint.AddSource(source);
            }
        }
    }

}
