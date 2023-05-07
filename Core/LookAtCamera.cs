using UnityEngine;
using UnityEngine.Animations;

namespace StudioScor.Utilities
{
    [RequireComponent(typeof(RotationConstraint))]
    public class LookAtCamera : BaseMonoBehaviour
    {
        [Header(" [ Look At Camera Component ] ")]
        [SerializeField] private RotationConstraint _Constraint;
        [SerializeField] private Camera _Camera;

#if UNITY_EDITOR
        private void Reset()
        {
            if(gameObject.TryGetComponentInParentOrChildren(out _Constraint))
            {
                _Constraint.constraintActive = true;
            }
        }
#endif

        private void Awake()
        {
            if (!_Camera)
                _Camera = Camera.main;

            ConstraintSource source = new()
            {
                sourceTransform = _Camera.transform,
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
