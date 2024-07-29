using UnityEngine;
using UnityEngine.Animations;

namespace StudioScor.Utilities
{
    public class LookAtCamera : BaseStateMono
    {
        [Header(" [ Look At Camera Component ] ")]
        [SerializeField] private RotationConstraint _constraint;
        [SerializeField] private Camera _targetCamera;
        protected override void Reset()
        {
#if UNITY_EDITOR
            base.Reset();

            if (gameObject.TryGetComponentInParentOrChildren(out _constraint))
            {
                _constraint.constraintActive = true;
            }
#endif
        }

        private void OnEnable()
        {
            EnterLookAtCamera();
        }

        private void OnDisable()
        {
            ExitLookAtCamera();
        }

        [ContextMenu("On Look At Camera", false, 1000000)]
        public void OnLookAtCamera()
        {
            ForceEnterState();
        }
        [ContextMenu("End Look At Camera", false, 1000000)]
        public void EndLookAtCamera()
        {
            ForceExitState();
        }

        protected virtual void EnterLookAtCamera()
        {
            if (!_targetCamera)
                _targetCamera = Camera.main;

            ConstraintSource source = new()
            {
                sourceTransform = _targetCamera.transform,
                weight = 1f,
            };

            if (_constraint.sourceCount > 0)
            {
                _constraint.SetSource(0, source);
            }
            else
            {
                _constraint.AddSource(source);
            }
        }

        protected virtual void ExitLookAtCamera()
        {
            if (_constraint.sourceCount > 0)
            {
                _constraint.RemoveSource(0);
            }
        }
    }
}