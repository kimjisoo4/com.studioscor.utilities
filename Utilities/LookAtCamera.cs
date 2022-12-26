using UnityEngine;
using UnityEngine.Animations;

namespace StudioScor.Utilities
{
    [RequireComponent(typeof(RotationConstraint))]
    public class LookAtCamera : BaseMonoBehaviour
    {
        [Header(" [ Look At Camera Component ] ")]
        [SerializeField] private RotationConstraint _Constraint;

#if UNITY_EDITOR
        private void Reset()
        {
            gameObject.TryGetComponentInParentOrChildren(out _Constraint);
        }
#endif

        private void Awake()
        {
            ConstraintSource source = new()
            {
                sourceTransform = Camera.main.transform,
                weight = 1f,
            };

            if (_Constraint.sourceCount > 0)
            {
                _Constraint.SetSource(0, source);
            }
            else
            {
                _Constraint.AddSource(source);
            }
        }
    }

}
